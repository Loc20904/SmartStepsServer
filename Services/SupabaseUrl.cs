namespace SmartStepsServer.Services;

public static class SupabaseUrl
{
    public static Uri CreateBaseUri(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException("SupabaseStorage:Url must be an absolute URL.");
        }

        var builder = new UriBuilder(uri)
        {
            Path = string.Empty,
            Query = string.Empty,
            Fragment = string.Empty
        };

        return new Uri(builder.Uri.ToString().TrimEnd('/') + "/");
    }
}
