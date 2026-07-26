using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Http;
public interface IHttpService
{
    #region Methods

    /// <summary>
    /// Send http get request
    /// </summary>
    /// <param name="requestUri">Request uri</param>
    /// <param name="parameters">Dictionary parameters</param>
    /// <param name="headers">Dictionary headers</param>
    /// <returns></returns>
    Task<HttpResponseMessage> GetAsync(string requestUri, IDictionary<string, string> parameters, IDictionary<string, string> headers = null);

    /// <summary>
    /// Send http post request
    /// </summary>
    /// <param name="requestUri">Request uri</param>
    /// <param name="httpContent">Dictionary parameters</param>
    /// <param name="headers">Dictionary headers</param>
    /// <returns></returns>
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent httpContent = null, IDictionary<string, string> headers = null);

    #endregion
}
