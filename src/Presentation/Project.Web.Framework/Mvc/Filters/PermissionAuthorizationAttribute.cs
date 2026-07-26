using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Services.Security;
using Project.Web.Framework.Models;
using System.Net;

namespace Project.Web.Framework.Mvc.Filters;

/// <summary>
/// Represents a filter attribute that confirms access to the api endpoint
/// </summary>
public sealed class PermissionAuthorizationAttribute : TypeFilterAttribute
{
    #region Ctor

    /// <summary>
    /// Create instance of the filter attribute
    /// </summary>
    /// <param name="ignore">Whether to ignore the execution of filter actions</param>
    public PermissionAuthorizationAttribute(string permissionName, bool addPermissionRequest = false, bool ignore = false, string deniedMessage = null) : base(typeof(PermissionAuthorizeFilter))
    {
        IgnoreFilter = ignore;
        PermissionName = permissionName;
        DeniedMessage = deniedMessage;
        Arguments = new object[] { ignore };
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to ignore the execution of filter actions
    /// </summary>
    public bool IgnoreFilter { get; }

    /// <summary>
    /// Gets a value indicating permission name for that perticular endpoint
    /// </summary>
    public string PermissionName { get; }

    /// <summary>
    /// Gets a value indicating access denied message for that perticular endpoint
    /// </summary>
    public string DeniedMessage { get; }

    /// <summary>
    /// Gets a value indicating permission name request will
    /// </summary>
    public bool AddPermissionRequest { get; }

    #endregion

    #region Nested filter

    /// <summary>
    /// Represents a filter that confirms access to the admin panel
    /// </summary>
    private class PermissionAuthorizeFilter : IAsyncAuthorizationFilter
    {
        #region Fields

        private readonly bool _ignoreFilter;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public PermissionAuthorizeFilter(bool ignoreFilter, IPermissionService permissionService)
        {
            _ignoreFilter = ignoreFilter;
            _permissionService = permissionService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized
        /// </summary>
        /// <param name="context">Authorization filter context</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task AuthorizeAdminAsync(AuthorizationFilterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            //check whether this filter has been overridden for the action
            var actionFilter = context.ActionDescriptor.FilterDescriptors
                .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                .Select(filterDescriptor => filterDescriptor.Filter)
                .OfType<PermissionAuthorizationAttribute>()
                .FirstOrDefault();

            //ignore filter (the action is available even if a customer hasn't access to the admin area)
            if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                return;

            //there is AdminAuthorizeFilter, so check access
            if (context.Filters.Any(filter => filter is PermissionAuthorizeFilter))
            {
                //authorize permission of access to the endpoint
                if (!await _permissionService.AuthorizeAsync(actionFilter.PermissionName))
                {
                    var message = $"You do not have the required permission to perform this action: {actionFilter.PermissionName}.";
                    if (!string.IsNullOrWhiteSpace(actionFilter.DeniedMessage))
                        message = actionFilter.DeniedMessage;

                    var responseModel = new BaseResponseModel<string>
                    {
                        Data = string.Empty,
                        Message = message,
                        Status = HttpStatusCode.Unauthorized
                    };
                    context.Result = new OkObjectResult(responseModel);
                    return;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized
        /// </summary>
        /// <param name="context">Authorization filter context</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await AuthorizeAdminAsync(context);
        }

        #endregion
    }

    #endregion
}
