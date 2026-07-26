using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Project.Web.Framework.Models;

namespace Project.Web.Framework;

[ApiController]
[ApiVersion("1")]
[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class BaseController : Controller
{
    #region Methods

    /// <summary>
    /// Returns paged list response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public IActionResult PagedList<T>(BaseResponseListModel<T> data, string message = null) where T : BaseModel
    {
        ArgumentNullException.ThrowIfNull(data);

        data.Status = HttpStatusCode.OK;
        data.Message = message ?? string.Empty;

        return Ok(data);
    }

    /// <summary>
    /// Returns success response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public IActionResult Success<T>(T data, string message = null)
    {
        ArgumentNullException.ThrowIfNull(data);

        var model = new BaseResponseModel<T>
        {
            Data = data,
            Status = HttpStatusCode.OK,
            Message = message ?? string.Empty
        };
        return Ok(model);
    }

    /// <summary>
    /// Returns error response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public IActionResult Error<T>(T data, string message = null)
    {
        var model = new BaseResponseModel<T>
        {
            Data = data,
            Status = HttpStatusCode.BadRequest,
            Message = message ?? string.Empty
        };
        return Ok(model);
    }

    /// <summary>
    /// return store id from header
    /// </summary>
    protected long StoreId
    {
        get
        {
            long store = 0;
            if (Request.Headers.TryGetValue("StoreId", out StringValues data))
            {
                long.TryParse(data.FirstOrDefault(), out store);
            }
            return store;
        }
    }

    #endregion
}
