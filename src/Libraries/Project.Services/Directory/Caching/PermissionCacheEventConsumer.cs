using Project.Core.Domain.Users;
using Project.Services.Caching;
using Project.Services.Permissions;
using Project.Services.Security;

namespace Project.Services.Directory.Caching
{
    public class PermissionCacheEventConsumer : CacheEventConsumer<UserPermissionRecordMapping>
    {
        private readonly IPermissionRecordService _permissionRecordService;
        public PermissionCacheEventConsumer(IPermissionRecordService permissionRecordService) 
        {
            _permissionRecordService = permissionRecordService;
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserPermissionRecordMapping entity)
        {
            var permissionName=(await _permissionRecordService.GetPermissionRecordByIdAsync(entity.PermissionRecordId)).SystemName;
            await RemoveByPrefixAsync(ProjectSecurityDefaults.PermissionAllowedPrefix, permissionName, entity.UserId);
            await RemoveByPrefixAsync(ProjectSecurityDefaults.PermissionRecordsAllPrefix, entity.UserId);
        }
    }
}
