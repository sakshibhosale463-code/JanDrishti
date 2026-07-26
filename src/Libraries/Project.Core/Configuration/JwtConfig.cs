namespace Project.Core.Configuration;

/// <summary>
/// Represents jwt configuration
/// </summary>
public class JwtConfig : IConfig
{
    /// <summary>
    /// Gets or sets the symmetric security key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the issuer
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the audience
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Gets or sets the token expiration in minitues
    /// </summary>
    public int TokenExpirationInMinitues { get; set; }
}
