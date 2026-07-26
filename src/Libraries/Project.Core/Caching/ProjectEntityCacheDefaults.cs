namespace Project.Core.Caching;

/// <summary>
/// Represents default values related to caching entities
/// </summary>
public static partial class ProjectEntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Gets an entity type name used in cache keys
    /// </summary>
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    /// <summary>
    /// Gets a key for caching entity by identifier
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ByIdCacheKey => new($"Project.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

    /// <summary>
    /// Gets a key for caching entities by identifiers
    /// </summary>
    /// <remarks>
    /// {0} : entity ids
    /// </remarks>
    public static CacheKey ByIdsCacheKey => new($"Project.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

    /// <summary>
    /// Gets a key for caching all entities
    /// </summary>
    public static CacheKey AllCacheKey => new($"Project.{EntityTypeName}.all.", AllPrefix, Prefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string Prefix => $"Project.{EntityTypeName}.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string ByIdPrefix => $"Project.{EntityTypeName}.byid.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string ByIdsPrefix => $"Project.{EntityTypeName}.byids.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string AllPrefix => $"Project.{EntityTypeName}.all.";
}