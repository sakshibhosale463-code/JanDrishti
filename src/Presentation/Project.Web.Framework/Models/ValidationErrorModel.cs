namespace Project.Web.Framework.Models;

/// <summary>
/// Represents validation error model
/// </summary>
public partial record ValidationErrorModel
{
    #region Constructor

    public ValidationErrorModel()
    {
        Errors = new List<string>();
    }

    #endregion

    #region Properties

    public string Key { get; set; }
    public List<string> Errors { get; set; }

    #endregion
}
