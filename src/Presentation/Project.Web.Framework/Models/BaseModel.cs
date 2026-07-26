namespace Project.Web.Framework.Models;

/// <summary>
/// Represents base nopCommerce model
/// </summary>
public partial record BaseModel
{
    /// <summary>
    /// Gets or sets model identifier
    /// </summary>
    public virtual long Id { get; set; }
}
