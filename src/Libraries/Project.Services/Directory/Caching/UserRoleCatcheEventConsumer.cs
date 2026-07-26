using Project.Core.Domain.Catalog;
using Project.Services.Caching;

namespace Project.Services.Directory.Caching
{

    public class UserRoleCatcheEventConsumer : CacheEventConsumer<UserRole>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserRole entity)
        {
            await RemoveByPrefixAsync(DirectoryDefault.UserRoleSelectDataAllCacheKeyPrefix);
        }
    }
}