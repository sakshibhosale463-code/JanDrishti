namespace Project.Core.Http;

/// <summary>
/// Represents default values related to cookies
/// </summary>
public static partial class ProjectCookieDefaults
{
    /// <summary>
    /// Gets the cookie name prefix
    /// </summary>
    public static string Prefix => ".Universe";

    /// <summary>
    /// Gets a cookie name of the culture
    /// </summary>
    public static string CultureCookie => ".Culture";
}