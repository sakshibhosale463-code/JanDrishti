using Project.Core.Domain.Localization;
using Project.Services.Caching;

namespace Project.Services.Localization.Caching;

/// <summary>
/// Represents a locale string resource cache event consumer
/// </summary>
public partial class LocaleStringResourceCacheEventConsumer : CacheEventConsumer<LocaleStringResource>
{
    /// <summary>
    /// Clear cache by entity event type
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(LocaleStringResource entity)
    {
        await RemoveAsync(ProjectLocalizationDefaults.LocaleStringResourcesAllPublicCacheKey, entity.LanguageId);
        await RemoveAsync(ProjectLocalizationDefaults.LocaleStringResourcesAllAdminCacheKey, entity.LanguageId);
        await RemoveAsync(ProjectLocalizationDefaults.LocaleStringResourcesAllCacheKey, entity.LanguageId);
        await RemoveByPrefixAsync(ProjectLocalizationDefaults.LocaleStringResourcesByNamePrefix, entity.LanguageId);
    }
}