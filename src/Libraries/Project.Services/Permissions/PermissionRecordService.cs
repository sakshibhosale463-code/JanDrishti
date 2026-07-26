using Project.Core.Domain.Security;
using Project.Data;

namespace Project.Services.Permissions
{
    public partial class PermissionRecordService : IPermissionRecordService
    {
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        public PermissionRecordService(IRepository<PermissionRecord> permissionRecordRepository)
        {
            _permissionRecordRepository = permissionRecordRepository;
        }
        public async Task<PermissionRecord> GetPermissionRecordByIdAsync(long id)
        {
            return await _permissionRecordRepository.GetByIdAsync(id);
        }
    }
}
