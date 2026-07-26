using Project.Core.Caching;
using Project.Core.Domain.Security;

namespace Project.Services.Security;

/// <summary>
/// Represents default values related to security services
/// </summary>
public static partial class ProjectSecurityDefaults
{
    #region Caching defaults

    #region Access control list

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : entity ID
    /// {1} : entity name
    /// </remarks>
    public static CacheKey AclRecordCacheKey => new("Project.aclrecord.{0}-{1}");

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : entity name
    /// </remarks>
    public static CacheKey EntityAclRecordExistsCacheKey => new("Project.aclrecord.exists.{0}");

    #endregion

    #region Permissions

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : permission system name
    /// {1} : customer role ID
    /// </remarks>
    public static CacheKey PermissionAllowedCacheKey => new("Project.permissionrecord.allowed.{0}-{1}", PermissionAllowedPrefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    /// <remarks>
    /// {0} : permission system name
    /// </remarks>
    public static string PermissionAllowedPrefix => "Project.permissionrecord.allowed.{0}-{1}";

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : customer role ID
    /// </remarks>
    public static CacheKey PermissionRecordsAllCacheKey => new("Project.permissionrecord.all.{0}", ProjectEntityCacheDefaults<PermissionRecord>.AllPrefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : customer role ID
    /// </remarks>
    public static string PermissionRecordsAllPrefix => new("Project.permissionrecord.all.{0}");
    #endregion

    #endregion

    #region Permissions


    #endregion
}