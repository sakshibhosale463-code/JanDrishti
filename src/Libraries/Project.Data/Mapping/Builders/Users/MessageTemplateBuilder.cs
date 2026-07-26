using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Users;

namespace Project.Data.Mapping.Builders.Users;
public class MessageTemplateBuilder : ProjectEntityBuilder<MessageTemplate>
{
    #region Methods
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MessageTemplate.Name)).AsString().Nullable()
            .WithColumn(nameof(MessageTemplate.Subject)).AsString().Nullable()
            .WithColumn(nameof(MessageTemplate.Body)).AsString().Nullable();
    }

    #endregion
}