using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SmartStepsServer.Options;

namespace SmartStepsServer.Services;

public sealed class SupabaseAuthService : ISupabaseAuthService
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseStorageOptions _options;
    private readonly ILogger<SupabaseAuthService> _logger;

    public SupabaseAuthService(
        HttpClient httpClient,
        IOptions<SupabaseStorageOptions> options,
        ILogger<SupabaseAuthService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<SupabaseUser?> GetUserAsync(string accessToken, CancellationToken cancellationToken)
    {
        var baseUri = SupabaseUrl.CreateBaseUri(_options.Url);
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(baseUri, "auth/v1/user"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("apikey", _options.ServiceRoleKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "Supabase Auth user lookup failed with status {StatusCode}: {Body}",
                (int)response.StatusCode,
                body);
            throw new SupabaseAuthException("Supabase Auth user lookup failed.");
        }

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        if (!TryGetString(root, "id", out var id) || string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        TryGetString(root, "email", out var email);
        return new SupabaseUser(id, email);
    }

    private static bool TryGetString(JsonElement element, string propertyName, out string? value)
    {
        value = null;
        if (!element.TryGetProperty(propertyName, out var property)
            || property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = property.GetString();
        return true;
    }
}

public sealed class SupabaseAuthException : Exception
{
    public SupabaseAuthException(string message) : base(message)
    {
    }
}
