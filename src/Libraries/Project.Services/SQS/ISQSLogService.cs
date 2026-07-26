using Project.Core.Domain.SQS;

namespace Project.Data.Mapping.Builders.SQS
{
    public partial interface ISQSLogService
    {
        /// <summary>
        /// insert failed sqs 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task InsertFailedSqsLogAsync(FailedSqsLog model);
        /// <summary>
        /// get all failed sqs list
        /// </summary>
        /// <returns></returns>
        Task<List<FailedSqsLog>> GetAllFailedSqsListAsync();
        /// <summary>
        /// delete failed sqs entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task DeleteFailedSqs(FailedSqsLog model);
    }
}
