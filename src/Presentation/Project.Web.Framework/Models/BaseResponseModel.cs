using System.Net;

namespace Project.Web.Framework.Models;

/// <summary>
/// Represents base response model
/// </summary>
public partial class BaseResponseModel<T>
{
    #region Properties

    /// <summary>
    /// Gets or sets the data
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Gets or sets the http status
    /// </summary>
    public HttpStatusCode Status { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; }

    #endregion
}
