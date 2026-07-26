using Project.Core.Domain.SQS;

namespace Project.Data.Mapping.Builders.SQS
{
    public partial class SQSLogService : ISQSLogService
    {
        #region Fields
        private readonly IRepository<FailedSqsLog> _failedSqsLogRepository;
        #endregion

        #region Constructor
        public SQSLogService(IRepository<FailedSqsLog> failedSqsLogRepository)
        {
            _failedSqsLogRepository = failedSqsLogRepository;
        }
        #endregion

        #region Methods 

        public async Task InsertFailedSqsLogAsync(FailedSqsLog failedSqsLog)
        {
            await _failedSqsLogRepository.InsertAsync(failedSqsLog);
        }

        public async Task<List<FailedSqsLog>> GetAllFailedSqsListAsync()
        {
            var query= _failedSqsLogRepository.Table.Where(x => x.Deleted == false);
            return await query.ToListAsync();
        }
        public async Task DeleteFailedSqs(FailedSqsLog model)
        {
            await _failedSqsLogRepository.DeleteAsync(model);
        }
        #endregion
    }
}

