using System.ComponentModel.DataAnnotations;

namespace SmartStepsServer.Options;

public sealed class SupabaseStorageOptions
{
    public const string SectionName = "SupabaseStorage";

    [Required]
    public string Url { get; set; } = string.Empty;

    [Required]
    public string ServiceRoleKey { get; set; } = string.Empty;

    [Required]
    public string Bucket { get; set; } = string.Empty;

    [Range(30, 3600)]
    public int SignedUrlExpiresInSeconds { get; set; } = 300;

    public bool RequireAuthenticatedUser { get; set; } = true;

    public string? DevelopmentMediaRoot { get; set; }
}
