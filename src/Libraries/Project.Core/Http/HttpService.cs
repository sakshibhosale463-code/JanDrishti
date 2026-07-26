using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Configuration;

namespace Project.Core.Http;
public class HttpService : IHttpService
{
    #region Fields

    private readonly HttpClient _httpClient;
    private readonly AppSettings _appSettings;

    #endregion

    #region Constructor

    public HttpService(HttpClient httpClient,
        AppSettings appSettings)
    {
        _httpClient = httpClient;
        _appSettings = appSettings;
    }

    #endregion

    #region Utilities

    protected FormUrlEncodedContent PrepareUrlContent(IDictionary<string, string> pairs)
    {
        if (pairs != null && pairs.Any())
        {
            var formUrl = new FormUrlEncodedContent(pairs);
            return formUrl;
        }
        return null;
    }

    protected void AddRequestHeaders(HttpRequestMessage httpRequestMessage, IDictionary<string, string> pairs)
    {
        if (pairs != null && pairs.Any())
        {
            foreach (var header in pairs)
                httpRequestMessage.Headers.Add(header.Key, header.Value);
        }
    }

    protected void SetDefaultHeaders(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        try
        {
            //_httpClient.Timeout = TimeSpan.FromMinutes(10);
            var response = await _httpClient.SendAsync(request);
            return response;
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Send http get request
    /// </summary>
    /// <param name="requestUri">Request uri</param>
    /// <param name="parameters">Dictionary parameters</param>
    /// <param name="headers">Dictionary headers</param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> GetAsync(string requestUri,
        IDictionary<string, string> parameters,
        IDictionary<string, string> headers = null)
    {
        if (string.IsNullOrWhiteSpace(requestUri))
            throw new ArgumentNullException(nameof(requestUri));

        if (parameters != null && parameters.Any())
        {
            var formUrl = PrepareUrlContent(parameters);
            requestUri += $"?{await formUrl.ReadAsStringAsync()}";
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        if (headers != null && headers.Any())
            AddRequestHeaders(requestMessage, headers);

        SetDefaultHeaders(requestMessage);

        var responseMessage = await SendAsync(requestMessage);

        return responseMessage;
    }

    /// <summary>
    /// Send http post request
    /// </summary>
    /// <param name="requestUri">Request uri</param>
    /// <param name="httpContent">Dictionary parameters</param>
    /// <param name="headers">Dictionary headers</param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> PostAsync(string requestUri,
        HttpContent httpContent = null,
        IDictionary<string, string> headers = null)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

        if (headers != null && headers.Any())
            AddRequestHeaders(requestMessage, headers);

        if (httpContent != null)
            requestMessage.Content = httpContent;

        SetDefaultHeaders(requestMessage);

        var responseMessage = await SendAsync(requestMessage);

        return responseMessage;
    }

    #endregion
}
