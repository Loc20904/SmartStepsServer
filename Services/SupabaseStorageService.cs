using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SmartStepsServer.Options;

namespace SmartStepsServer.Services;

public sealed class SupabaseStorageService : ISupabaseStorageService
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseStorageOptions _options;
    private readonly ILogger<SupabaseStorageService> _logger;

    public SupabaseStorageService(
        HttpClient httpClient,
        IOptions<SupabaseStorageOptions> options,
        ILogger<SupabaseStorageService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> CreateSignedUrlAsync(
        string bucket,
        string objectPath,
        int expiresInSeconds,
        CancellationToken cancellationToken)
    {
        var baseUri = SupabaseUrl.CreateBaseUri(_options.Url);
        var endpoint = new Uri(
            baseUri,
            $"storage/v1/object/sign/{Uri.EscapeDataString(bucket)}/{EncodeStoragePath(objectPath)}");

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ServiceRoleKey);
        request.Headers.Add("apikey", _options.ServiceRoleKey);
        request.Content = JsonContent.Create(new { expiresIn = expiresInSeconds });

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "Supabase Storage signed URL request failed with status {StatusCode}: {Body}",
                (int)response.StatusCode,
                body);
            throw new SupabaseStorageException(
                "Supabase Storage signed URL request failed.",
                response.StatusCode,
                body,
                bucket,
                objectPath,
                endpoint);
        }

        using var document = JsonDocument.Parse(body);
        if (!TryGetSignedUrl(document.RootElement, out var signedUrl)
            || string.IsNullOrWhiteSpace(signedUrl))
        {
            throw new SupabaseStorageException("Supabase Storage response did not include a signed URL.");
        }

        return NormalizeSignedUrl(baseUri, signedUrl);
    }

    private static string EncodeStoragePath(string objectPath)
    {
        return string.Join(
            "/",
            objectPath
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(Uri.EscapeDataString));
    }

    private static bool TryGetSignedUrl(JsonElement root, out string? signedUrl)
    {
        signedUrl = null;
        foreach (var propertyName in new[] { "signedURL", "signedUrl", "signed_url" })
        {
            if (root.TryGetProperty(propertyName, out var property)
                && property.ValueKind == JsonValueKind.String)
            {
                signedUrl = property.GetString();
                return true;
            }
        }

        return false;
    }

    private static string NormalizeSignedUrl(Uri baseUri, string signedUrl)
    {
        if (Uri.TryCreate(signedUrl, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.ToString();
        }

        var relativePath = signedUrl.TrimStart('/');
        if (relativePath.StartsWith("object/", StringComparison.OrdinalIgnoreCase))
        {
            relativePath = "storage/v1/" + relativePath;
        }

        return new Uri(baseUri, relativePath).ToString();
    }
}

public sealed class SupabaseStorageException : Exception
{
    public SupabaseStorageException(string message) : base(message)
    {
    }

    public SupabaseStorageException(
        string message,
        HttpStatusCode statusCode,
        string responseBody,
        string bucket,
        string objectPath,
        Uri requestUri) : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        Bucket = bucket;
        ObjectPath = objectPath;
        RequestUri = requestUri;
    }

    public HttpStatusCode? StatusCode { get; }

    public string? ResponseBody { get; }

    public string? Bucket { get; }

    public string? ObjectPath { get; }

    public Uri? RequestUri { get; }
}
