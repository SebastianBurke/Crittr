namespace Crittr.Maui;

/// <summary>
/// API base URL for the Crittr.Server instance. Android emulators cannot reach the host loopback;
/// use 10.0.2.2 instead of localhost.
/// </summary>
public static class ApiConfiguration
{
#if ANDROID
    public const string BaseUrl = "https://10.0.2.2:7282/";
#else
    public const string BaseUrl = "https://localhost:7282/";
#endif
}
