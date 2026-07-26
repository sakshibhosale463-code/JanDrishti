using Project.Core.Domain.Localization;
using Project.Services.Caching;

namespace Project.Services.Localization.Caching;

/// <summary>
/// Represents a localized property cache event consumer
/// </summary>
public partial class LocalizedPropertyCacheEventConsumer : CacheEventConsumer<LocalizedProperty>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(LocalizedProperty entity)
    {
        await RemoveAsync(ProjectLocalizationDefaults.LocalizedPropertyCacheKey, entity.LanguageId, entity.EntityId, entity.LocaleKeyGroup, entity.LocaleKey);
        await RemoveAsync(ProjectLocalizationDefaults.LocalizedPropertiesCacheKey, entity.EntityId, entity.LocaleKeyGroup, entity.LocaleKey);
        await RemoveAsync(ProjectLocalizationDefaults.LocalizedPropertyLookupCacheKey, entity.LanguageId);
    }
}