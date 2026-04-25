namespace Crittr.App;

/// <summary>
/// Static assets from this Razor Class Library are published under <c>_content/Crittr.App/</c>
/// for Blazor WebAssembly and Blazor Hybrid hosts.
/// </summary>
public static class AppAssets
{
    public const string ContentBase = "_content/Crittr.App";

    public static string Url(string relativePath)
    {
        relativePath = relativePath.TrimStart('/');
        return $"{ContentBase}/{relativePath}";
    }

    private static readonly HashSet<string> AllowedExternalHosts = new(StringComparer.OrdinalIgnoreCase)
    {
        "crittr.ca",
        "www.crittr.ca",
        "cdn.crittr.ca",
    };

    /// <summary>
    /// Maps relative paths to the RCL static-asset path. Absolute http(s) URLs are only allowed if their host
    /// is in <see cref="AllowedExternalHosts"/>; otherwise the default critter icon is returned. This prevents
    /// referrer leaks / pixel-tracking via attacker-controlled IconUrl values stored in the DB.
    /// </summary>
    public static string Resolve(string? pathOrUrl)
    {
        if (string.IsNullOrWhiteSpace(pathOrUrl))
            return Url("img/critters/default.svg");
        var p = pathOrUrl.Trim();
        if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            if (Uri.TryCreate(p, UriKind.Absolute, out var uri) && AllowedExternalHosts.Contains(uri.Host))
                return p;
            return Url("img/critters/default.svg");
        }
        return Url(p.TrimStart('/'));
    }
}
