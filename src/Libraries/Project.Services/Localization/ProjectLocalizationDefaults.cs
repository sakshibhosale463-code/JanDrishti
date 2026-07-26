using Project.Core.Caching;
using Project.Core.Domain.Localization;

namespace Project.Services.Localization;

/// <summary>
/// Represents default values related to localization services
/// </summary>
public static partial class ProjectLocalizationDefaults
{
    #region Locales

    /// <summary>
    /// Gets a prefix of locale resources for the admin area
    /// </summary>
    public static string AdminLocaleStringResourcesPrefix => "Admin.";

    /// <summary>
    /// Gets a prefix of locale resources for enumerations 
    /// </summary>
    public static string EnumLocaleStringResourcesPrefix => "Enums.";

    /// <summary>
    /// Gets a prefix of locale resources for permissions 
    /// </summary>
    public static string PermissionLocaleStringResourcesPrefix => "Security.Permission.";

    /// <summary>
    /// Gets a prefix of locale resources for plugin friendly names 
    /// </summary>
    public static string PluginNameLocaleStringResourcesPrefix => "Plugins.FriendlyName.";

    #endregion

    #region Caching defaults

    #region Languages

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {1} : show hidden records?
    /// </remarks>
    public static CacheKey LanguagesAllCacheKey => new("Nop.language.all.{0}", LanguagesByStorePrefix, ProjectEntityCacheDefaults<Language>.AllPrefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static string LanguagesByStorePrefix => "Nop.language.all";

    #endregion

    #region Locales

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// </remarks>
    public static CacheKey LocaleStringResourcesAllPublicCacheKey => new("Nop.localestringresource.bylanguage.public.{0}", ProjectEntityCacheDefaults<LocaleStringResource>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// </remarks>
    public static CacheKey LocaleStringResourcesAllAdminCacheKey => new("Nop.localestringresource.bylanguage.admin.{0}", ProjectEntityCacheDefaults<LocaleStringResource>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// </remarks>
    public static CacheKey LocaleStringResourcesAllCacheKey => new("Nop.localestringresource.bylanguage.{0}", ProjectEntityCacheDefaults<LocaleStringResource>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// {1} : resource key
    /// </remarks>
    public static CacheKey LocaleStringResourcesByNameCacheKey => new("Nop.localestringresource.byname.{0}-{1}", LocaleStringResourcesByNamePrefix, ProjectEntityCacheDefaults<LocaleStringResource>.Prefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// </remarks>
    public static string LocaleStringResourcesByNamePrefix => "Nop.localestringresource.byname.{0}";

    #endregion

    #region Localized properties

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// {1} : entity ID
    /// {2} : locale key group
    /// {3} : locale key
    /// </remarks>
    public static CacheKey LocalizedPropertyCacheKey => new("Nop.localizedproperty.value.{0}-{1}-{2}-{3}");

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : entity ID
    /// {1} : locale key group
    /// {2} : locale key
    /// </remarks>
    public static CacheKey LocalizedPropertiesCacheKey => new("Nop.localizedproperty.all.{0}-{1}-{2}");

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// </remarks>
    public static CacheKey LocalizedPropertyLookupCacheKey => new("Nop.localizedproperty.value.{0}");

    #endregion

    #endregion
}