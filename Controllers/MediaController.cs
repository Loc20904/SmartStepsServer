using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartStepsServer.Data;
using SmartStepsServer.Options;
using SmartStepsServer.Services;

namespace SmartStepsServer.Controllers;

[ApiController]
[Route("api/media")]
public sealed class MediaController : ControllerBase
{
    private readonly SmartStepsDbContext _dbContext;
    private readonly ISupabaseAuthService _authService;
    private readonly ISupabaseStorageService _storageService;
    private readonly SupabaseStorageOptions _options;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<MediaController> _logger;

    public MediaController(
        SmartStepsDbContext dbContext,
        ISupabaseAuthService authService,
        ISupabaseStorageService storageService,
        IOptions<SupabaseStorageOptions> options,
        IWebHostEnvironment environment,
        ILogger<MediaController> logger)
    {
        _dbContext = dbContext;
        _authService = authService;
        _storageService = storageService;
        _options = options.Value;
        _environment = environment;
        _logger = logger;
    }

    [HttpGet("storage-config")]
    [ProducesResponseType(typeof(StorageConfigDiagnosticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<StorageConfigDiagnosticsResponse> GetStorageConfig()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        var validationErrors = ValidateOptions(_options);
        return Ok(new StorageConfigDiagnosticsResponse
        {
            EnvironmentName = _environment.EnvironmentName,
            Effective = new EffectiveStorageConfig
            {
                Url = _options.Url,
                Bucket = _options.Bucket,
                SignedUrlExpiresInSeconds = _options.SignedUrlExpiresInSeconds,
                RequireAuthenticatedUser = _options.RequireAuthenticatedUser,
                ServiceRoleKeyConfigured = !string.IsNullOrWhiteSpace(_options.ServiceRoleKey)
            },
            EnvironmentVariables = new StorageEnvironmentVariables
            {
                Url = ReadNonSecretEnvironmentVariable("SupabaseStorage__Url"),
                Bucket = ReadNonSecretEnvironmentVariable("SupabaseStorage__Bucket"),
                SignedUrlExpiresInSeconds = ReadNonSecretEnvironmentVariable("SupabaseStorage__SignedUrlExpiresInSeconds"),
                RequireAuthenticatedUser = ReadNonSecretEnvironmentVariable("SupabaseStorage__RequireAuthenticatedUser"),
                ServiceRoleKey = ReadSecretEnvironmentVariable("SupabaseStorage__ServiceRoleKey")
            },
            ValidationErrors = validationErrors
        });
    }

    [HttpPost("signed-url")]
    [ProducesResponseType(typeof(CreateSignedMediaUrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<CreateSignedMediaUrlResponse>> CreateSignedUrl(
        CreateSignedMediaUrlRequest request,
        CancellationToken cancellationToken)
    {
        var validationErrors = ValidateOptions(_options);
        if (validationErrors.Count > 0)
        {
            if (_environment.IsDevelopment())
            {
                return await CreateDevelopmentMediaResponseAsync(request.StepId, cancellationToken);
            }

            _logger.LogError(
                "Supabase Storage is not configured correctly: {Errors}",
                string.Join("; ", validationErrors));
            return Problem(
                title: "Supabase Storage is not configured.",
                detail: string.Join("; ", validationErrors),
                statusCode: StatusCodes.Status500InternalServerError);
        }

        if (_options.RequireAuthenticatedUser)
        {
            var accessToken = GetBearerToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Unauthorized(new { message = "Missing Supabase access token." });
            }

            try
            {
                var user = await _authService.GetUserAsync(accessToken, cancellationToken);
                if (user is null)
                {
                    return Unauthorized(new { message = "Invalid Supabase access token." });
                }
            }
            catch (SupabaseAuthException ex)
            {
                _logger.LogError(ex, "Could not validate Supabase access token.");
                return Problem(
                    title: "Could not validate access token.",
                    statusCode: StatusCodes.Status502BadGateway);
            }
        }

        var media = await _dbContext.SituationSteps
            .AsNoTracking()
            .Where(step => step.StepId == request.StepId)
            .Select(step => new
            {
                step.StepId,
                step.MediaUrl,
                SituationStatus = step.Situation.Status
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (media is null
            || !string.Equals(media.SituationStatus, "Published", StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrWhiteSpace(media.MediaUrl))
        {
            return NotFound(new { message = "Media was not found." });
        }

        if (!TryGetObjectPath(media.MediaUrl, _options, out var objectPath))
        {
            _logger.LogError(
                "Invalid media URL/path for step {StepId}: {MediaUrl}",
                media.StepId,
                media.MediaUrl);
            return Problem(
                title: "Media path is invalid.",
                detail: $"Step {media.StepId} has MediaUrl '{media.MediaUrl}', which is not a valid object path for bucket '{_options.Bucket}'.",
                statusCode: StatusCodes.Status500InternalServerError);
        }

        try
        {
            var signedUrl = await _storageService.CreateSignedUrlAsync(
                _options.Bucket,
                objectPath,
                _options.SignedUrlExpiresInSeconds,
                cancellationToken);

            return Ok(new CreateSignedMediaUrlResponse
            {
                StepId = media.StepId,
                Bucket = _options.Bucket,
                Path = objectPath,
                SignedUrl = signedUrl,
                ExpiresInSeconds = _options.SignedUrlExpiresInSeconds,
                ExpiresAtUtc = DateTime.UtcNow.AddSeconds(_options.SignedUrlExpiresInSeconds)
            });
        }
        catch (SupabaseStorageException ex)
        {
            _logger.LogError(ex, "Could not create signed URL for step {StepId}.", media.StepId);
            return Problem(
                title: "Could not create signed media URL.",
                detail: BuildStorageFailureDetail(ex),
                statusCode: StatusCodes.Status502BadGateway);
        }
    }

    [HttpGet("development/{stepId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevelopmentMedia(
        int stepId,
        CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        var mediaUrl = await GetPublishedMediaUrlAsync(stepId, cancellationToken);
        if (!TryResolveDevelopmentMediaPath(mediaUrl, out var mediaPath))
        {
            return NotFound(new { message = "Development media was not found." });
        }

        return PhysicalFile(mediaPath, "video/mp4", enableRangeProcessing: true);
    }

    [HttpPost("signed-voice-url")]
    [ProducesResponseType(typeof(CreateSignedVoiceUrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<CreateSignedVoiceUrlResponse>> CreateSignedVoiceUrl(
        CreateSignedVoiceUrlRequest request,
        CancellationToken cancellationToken)
    {
        var validationErrors = ValidateOptions(_options);
        if (validationErrors.Count > 0)
        {
            _logger.LogError(
                "Supabase Storage is not configured correctly: {Errors}",
                string.Join("; ", validationErrors));
            return Problem(
                title: "Supabase Storage is not configured.",
                detail: string.Join("; ", validationErrors),
                statusCode: StatusCodes.Status500InternalServerError);
        }

        if (_options.RequireAuthenticatedUser)
        {
            var accessToken = GetBearerToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Unauthorized(new { message = "Missing Supabase access token." });
            }

            try
            {
                var user = await _authService.GetUserAsync(accessToken, cancellationToken);
                if (user is null)
                {
                    return Unauthorized(new { message = "Invalid Supabase access token." });
                }
            }
            catch (SupabaseAuthException ex)
            {
                _logger.LogError(ex, "Could not validate Supabase access token.");
                return Problem(
                    title: "Could not validate access token.",
                    statusCode: StatusCodes.Status502BadGateway);
            }
        }

        if (!TryGetObjectPath(request.MediaUrl, _options, out var objectPath))
        {
            _logger.LogError("Invalid voice media URL/path: {MediaUrl}", request.MediaUrl);
            return Problem(
                title: "Voice media path is invalid.",
                detail: $"MediaUrl '{request.MediaUrl}' is not a valid object path for bucket '{_options.Bucket}'.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var isPublishedFlashcardVoice = await _dbContext.Flashcards
            .AsNoTracking()
            .AnyAsync(flashcard =>
                flashcard.Situation.Status == "Published" &&
                flashcard.Situation.Island.Status == "Active" &&
                (flashcard.QuestionVoiceUrl == objectPath ||
                    flashcard.OptionAVoiceUrl == objectPath ||
                    flashcard.OptionBVoiceUrl == objectPath),
                cancellationToken);

        if (!isPublishedFlashcardVoice)
        {
            return NotFound(new { message = "Voice media was not found." });
        }

        try
        {
            var signedUrl = await _storageService.CreateSignedUrlAsync(
                _options.Bucket,
                objectPath,
                _options.SignedUrlExpiresInSeconds,
                cancellationToken);

            return Ok(new CreateSignedVoiceUrlResponse
            {
                Bucket = _options.Bucket,
                Path = objectPath,
                SignedUrl = signedUrl,
                ExpiresInSeconds = _options.SignedUrlExpiresInSeconds,
                ExpiresAtUtc = DateTime.UtcNow.AddSeconds(_options.SignedUrlExpiresInSeconds)
            });
        }
        catch (SupabaseStorageException ex)
        {
            _logger.LogError(ex, "Could not create signed voice URL for {MediaUrl}.", request.MediaUrl);
            return Problem(
                title: "Could not create signed voice URL.",
                detail: BuildStorageFailureDetail(ex),
                statusCode: StatusCodes.Status502BadGateway);
        }
    }

    private string? GetBearerToken()
    {
        var authorization = Request.Headers.Authorization.ToString();
        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authorization["Bearer ".Length..].Trim();
        }

        return null;
    }

    private async Task<ActionResult<CreateSignedMediaUrlResponse>> CreateDevelopmentMediaResponseAsync(
        int stepId,
        CancellationToken cancellationToken)
    {
        var mediaUrl = await GetPublishedMediaUrlAsync(stepId, cancellationToken);
        if (!TryResolveDevelopmentMediaPath(mediaUrl, out var mediaPath))
        {
            return NotFound(new { message = "Development media was not found." });
        }

        var requestBaseUrl = $"{Request.Scheme}://{Request.Host}";
        return Ok(new CreateSignedMediaUrlResponse
        {
            StepId = stepId,
            Bucket = "local-development",
            Path = Path.GetFileName(mediaPath),
            SignedUrl = $"{requestBaseUrl}/api/media/development/{stepId}",
            ExpiresInSeconds = _options.SignedUrlExpiresInSeconds,
            ExpiresAtUtc = DateTime.UtcNow.AddSeconds(_options.SignedUrlExpiresInSeconds)
        });
    }

    private async Task<string?> GetPublishedMediaUrlAsync(
        int stepId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.SituationSteps
            .AsNoTracking()
            .Where(step =>
                step.StepId == stepId
                && step.Situation.Status == "Published"
                && step.Situation.Island.Status == "Active")
            .Select(step => step.MediaUrl)
            .SingleOrDefaultAsync(cancellationToken);
    }

    private bool TryResolveDevelopmentMediaPath(
        string? mediaUrl,
        out string mediaPath)
    {
        mediaPath = string.Empty;
        if (string.IsNullOrWhiteSpace(mediaUrl)
            || string.IsNullOrWhiteSpace(_options.DevelopmentMediaRoot))
        {
            return false;
        }

        var rootPath = Path.GetFullPath(
            Path.Combine(_environment.ContentRootPath, _options.DevelopmentMediaRoot));
        var fileName = Path.GetFileName(mediaUrl.Replace('\\', '/'));
        var candidatePath = Path.GetFullPath(Path.Combine(rootPath, fileName));

        if (!candidatePath.StartsWith(rootPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            || !System.IO.File.Exists(candidatePath))
        {
            return false;
        }

        mediaPath = candidatePath;
        return true;
    }

    private static List<string> ValidateOptions(SupabaseStorageOptions options)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(options);
        Validator.TryValidateObject(options, context, results, validateAllProperties: true);

        if (!Uri.TryCreate(options.Url, UriKind.Absolute, out _))
        {
            results.Add(new ValidationResult("SupabaseStorage:Url must be an absolute URL."));
        }

        return results
            .Where(result => !string.IsNullOrWhiteSpace(result.ErrorMessage))
            .Select(result => result.ErrorMessage!)
            .ToList();
    }

    private static bool TryGetObjectPath(
        string mediaUrl,
        SupabaseStorageOptions options,
        out string objectPath)
    {
        objectPath = string.Empty;
        var value = mediaUrl.Trim();

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            if (!Uri.TryCreate(options.Url, UriKind.Absolute, out var supabaseUri)
                || !string.Equals(uri.Host, supabaseUri.Host, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return TryExtractObjectPath(uri.AbsolutePath, options.Bucket, out objectPath);
        }

        var queryIndex = value.IndexOfAny(new[] { '?', '#' });
        if (queryIndex >= 0)
        {
            value = value[..queryIndex];
        }

        value = value.Replace('\\', '/').TrimStart('/');
        if (TryExtractObjectPath(value, options.Bucket, out objectPath))
        {
            return true;
        }

        if (value.StartsWith(options.Bucket + "/", StringComparison.OrdinalIgnoreCase))
        {
            value = value[(options.Bucket.Length + 1)..];
        }

        if (!IsSafeObjectPath(value))
        {
            return false;
        }

        objectPath = value;
        return true;
    }

    private static bool TryExtractObjectPath(
        string path,
        string bucket,
        out string objectPath)
    {
        objectPath = string.Empty;
        var segments = path
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(Uri.UnescapeDataString)
            .ToArray();

        for (var index = 0; index <= segments.Length - 5; index++)
        {
            var isStorageObjectUrl =
                string.Equals(segments[index], "storage", StringComparison.OrdinalIgnoreCase)
                && string.Equals(segments[index + 1], "v1", StringComparison.OrdinalIgnoreCase)
                && string.Equals(segments[index + 2], "object", StringComparison.OrdinalIgnoreCase)
                && IsStorageAccessSegment(segments[index + 3])
                && string.Equals(segments[index + 4], bucket, StringComparison.OrdinalIgnoreCase);

            if (!isStorageObjectUrl)
            {
                continue;
            }

            var candidate = string.Join("/", segments.Skip(index + 5));
            if (!IsSafeObjectPath(candidate))
            {
                return false;
            }

            objectPath = candidate;
            return true;
        }

        return false;
    }

    private static bool IsStorageAccessSegment(string segment)
    {
        return string.Equals(segment, "public", StringComparison.OrdinalIgnoreCase)
            || string.Equals(segment, "sign", StringComparison.OrdinalIgnoreCase)
            || string.Equals(segment, "authenticated", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsSafeObjectPath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var segments = value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments.Length > 0
            && segments.All(segment => segment is not "." and not "..");
    }

    private static string BuildStorageFailureDetail(SupabaseStorageException exception)
    {
        var parts = new List<string> { exception.Message };

        if (exception.StatusCode is { } statusCode)
        {
            parts.Add($"Supabase returned {(int)statusCode} ({statusCode}).");
        }

        if (!string.IsNullOrWhiteSpace(exception.Bucket)
            && !string.IsNullOrWhiteSpace(exception.ObjectPath))
        {
            parts.Add($"Bucket: '{exception.Bucket}', path: '{exception.ObjectPath}'.");
        }

        if (exception.RequestUri is { } requestUri)
        {
            parts.Add($"Request URL: '{requestUri}'.");
        }

        if (!string.IsNullOrWhiteSpace(exception.ResponseBody))
        {
            parts.Add($"Response: {Truncate(exception.ResponseBody, 1000)}");
        }

        return string.Join(" ", parts);
    }

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength
            ? value
            : value[..maxLength] + "...";
    }

    private static EnvironmentVariableDiagnostics ReadNonSecretEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return new EnvironmentVariableDiagnostics
        {
            IsSet = !string.IsNullOrWhiteSpace(value),
            Value = value
        };
    }

    private static EnvironmentVariableDiagnostics ReadSecretEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return new EnvironmentVariableDiagnostics
        {
            IsSet = !string.IsNullOrWhiteSpace(value)
        };
    }
}

public sealed class CreateSignedMediaUrlRequest
{
    [Range(1, int.MaxValue)]
    public int StepId { get; set; }
}

public sealed class CreateSignedMediaUrlResponse
{
    public int StepId { get; set; }

    public string Bucket { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string SignedUrl { get; set; } = string.Empty;

    public int ExpiresInSeconds { get; set; }

    public DateTime ExpiresAtUtc { get; set; }
}

public sealed class CreateSignedVoiceUrlRequest
{
    [Required]
    public string MediaUrl { get; set; } = string.Empty;
}

public sealed class CreateSignedVoiceUrlResponse
{
    public string Bucket { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string SignedUrl { get; set; } = string.Empty;

    public int ExpiresInSeconds { get; set; }

    public DateTime ExpiresAtUtc { get; set; }
}

public sealed class StorageConfigDiagnosticsResponse
{
    public string EnvironmentName { get; set; } = string.Empty;

    public EffectiveStorageConfig Effective { get; set; } = new();

    public StorageEnvironmentVariables EnvironmentVariables { get; set; } = new();

    public List<string> ValidationErrors { get; set; } = new();
}

public sealed class EffectiveStorageConfig
{
    public string Url { get; set; } = string.Empty;

    public string Bucket { get; set; } = string.Empty;

    public int SignedUrlExpiresInSeconds { get; set; }

    public bool RequireAuthenticatedUser { get; set; }

    public bool ServiceRoleKeyConfigured { get; set; }
}

public sealed class StorageEnvironmentVariables
{
    public EnvironmentVariableDiagnostics Url { get; set; } = new();

    public EnvironmentVariableDiagnostics Bucket { get; set; } = new();

    public EnvironmentVariableDiagnostics SignedUrlExpiresInSeconds { get; set; } = new();

    public EnvironmentVariableDiagnostics RequireAuthenticatedUser { get; set; } = new();

    public EnvironmentVariableDiagnostics ServiceRoleKey { get; set; } = new();
}

public sealed class EnvironmentVariableDiagnostics
{
    public bool IsSet { get; set; }

    public string? Value { get; set; }
}
