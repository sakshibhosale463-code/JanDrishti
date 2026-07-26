using Project.Core.Domain.Localization;
using Project.Services.Caching;

namespace Project.Services.Localization.Caching;

/// <summary>
/// Represents a language cache event consumer
/// </summary>
public partial class LanguageCacheEventConsumer : CacheEventConsumer<Language>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(Language entity)
    {
        await RemoveAsync(ProjectLocalizationDefaults.LocaleStringResourcesAllPublicCacheKey, entity);
        await RemoveAsync(ProjectLocalizationDefaults.LocaleStringResourcesAllAdminCacheKey, entity);
        await RemoveAsync(ProjectLocalizationDefaults.LocaleStringResourcesAllCacheKey, entity);
        await RemoveByPrefixAsync(ProjectLocalizationDefaults.LocaleStringResourcesByNamePrefix, entity);
    }
}