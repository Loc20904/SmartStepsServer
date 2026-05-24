namespace SmartStepsServer.Services;

public interface ISupabaseAuthService
{
    Task<SupabaseUser?> GetUserAsync(string accessToken, CancellationToken cancellationToken);
}

public sealed record SupabaseUser(string Id, string? Email);
