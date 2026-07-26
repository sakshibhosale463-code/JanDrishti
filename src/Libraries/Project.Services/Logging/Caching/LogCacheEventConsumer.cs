using Project.Core.Domain.Logging;
using Project.Services.Caching;

namespace Project.Services.Logging.Caching;

/// <summary>
/// Represents a log cache event consumer
/// </summary>
public partial class LogCacheEventConsumer : CacheEventConsumer<Log>
{
}