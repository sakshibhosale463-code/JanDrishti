using Project.Core.Domain.Security;

namespace Project.Services.Permissions
{
    public partial interface IPermissionRecordService
    {
        Task<PermissionRecord> GetPermissionRecordByIdAsync(long id);
    }
}
