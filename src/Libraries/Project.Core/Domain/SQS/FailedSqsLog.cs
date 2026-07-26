using Project.Core.Domain.Common;
using Project.Core;

namespace Project.Core.Domain.SQS
{

    public partial class FailedSqsLog : BaseEntity, ISoftDeletedEntity
    {
        public bool Deleted { get; set; }
        public string MessageBody { get; set; }
        public string Exception { get; set; }
        public string Group { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}