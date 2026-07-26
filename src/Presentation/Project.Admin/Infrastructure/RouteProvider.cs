using Project.Services.Installation;
using Project.Web.Framework.Mvc.Routing;

namespace Project.Admin.Infrastructure;

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
        //restart application (AJAX)
        endpointRouteBuilder.MapControllerRoute(name: "InstallationRestartApplication",
            pattern: $"{ProjectInstallationDefaults.InstallPath}/restartapplication",
            defaults: new { controller = "Install", action = "RestartApplication" });

        //install
        endpointRouteBuilder.MapControllerRoute(name: "Installation",
            pattern: $"{ProjectInstallationDefaults.InstallPath}",
            defaults: new { controller = "Install", action = "Index" });

        endpointRouteBuilder.MapControllerRoute(name: "InstallationChangeLanguage",
            pattern: $"{ProjectInstallationDefaults.InstallPath}/ChangeLanguage/{{language}}",
            defaults: new { controller = "Install", action = "ChangeLanguage" });

        //restart application (AJAX)
        endpointRouteBuilder.MapControllerRoute(name: "InstallationRestartApplication",
            pattern: $"{ProjectInstallationDefaults.InstallPath}/restartapplication",
            defaults: new { controller = "Install", action = "RestartApplication" });
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;

    #endregion
}