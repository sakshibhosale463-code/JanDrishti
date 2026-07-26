using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Master;

namespace Project.Data.Mapping.Builders.Notification;

public class NotificationMasterBuilder : ProjectEntityBuilder<NotificationMaster>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(NotificationMaster.Title)).AsString(250).NotNullable()
            .WithColumn(nameof(NotificationMaster.Message)).AsString(1000).Nullable()
            .WithColumn(nameof(NotificationMaster.NotificationType)).AsString(100).Nullable()
            .WithColumn(nameof(NotificationMaster.IsRead)).AsBoolean().NotNullable()
            .WithColumn(nameof(NotificationMaster.CreatedOnUtc)).AsDateTime().Nullable();
    }
}