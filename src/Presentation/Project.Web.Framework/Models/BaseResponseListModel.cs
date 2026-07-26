using System.Net;

namespace Project.Web.Framework.Models;

/// <summary>
/// Represents base response list model
/// </summary>
public partial record BaseResponseListModel<T> where T : BaseModel
{
    #region Properties

    /// <summary>
    /// Gets or sets the data
    /// </summary>
    public IEnumerable<T> Data { get; set; }

    /// <summary>
    /// Gets or sets a number of filtered data records
    /// </summary>
    public int RecordsFiltered { get; set; }

    /// <summary>
    /// Gets or sets a number of total data records
    /// </summary>
    public int RecordsTotal { get; set; }

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
