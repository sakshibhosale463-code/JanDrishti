using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Project.Web.Framework.Mvc.Routing;

/// <summary>
/// Represents provider that provided basic routes
/// </summary>
public partial class RouteProvider : IRouteProvider
{
    #region Methods

    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDefaultControllerRoute();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;

    #endregion
}
