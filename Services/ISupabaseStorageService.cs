namespace SmartStepsServer.Services;

public interface ISupabaseStorageService
{
    Task<string> CreateSignedUrlAsync(
        string bucket,
        string objectPath,
        int expiresInSeconds,
        CancellationToken cancellationToken);
}
