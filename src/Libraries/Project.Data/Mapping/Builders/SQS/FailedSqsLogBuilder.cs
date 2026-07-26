using System.Data;
using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.SQS;

namespace Project.Data.Mapping.Builders.SQS
{

    /// <summary>
    /// Represents a type entity builder
    /// </summary>
    public partial class FailedSqsLogBuilder : ProjectEntityBuilder<FailedSqsLog>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table) => table
            .WithColumn(nameof(FailedSqsLog.Exception)).AsString().Nullable()
            .WithColumn(nameof(FailedSqsLog.MessageBody)).AsString().Nullable()
            .WithColumn(nameof(FailedSqsLog.Group)).AsString().Nullable()
            .WithColumn(nameof(FailedSqsLog.CreatedOn)).AsDateTime().Nullable()
            .WithColumn(nameof(FailedSqsLog.Deleted)).AsBoolean().NotNullable();
        #endregion
    }
}