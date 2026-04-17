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

    /// <summary>
    /// Returns absolute http(s) URLs unchanged; otherwise maps to the RCL static-asset path.
    /// </summary>
    public static string Resolve(string? pathOrUrl)
    {
        if (string.IsNullOrWhiteSpace(pathOrUrl))
            return Url("img/critters/default.svg");
        var p = pathOrUrl.Trim();
        if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return p;
        return Url(p.TrimStart('/'));
    }
}
