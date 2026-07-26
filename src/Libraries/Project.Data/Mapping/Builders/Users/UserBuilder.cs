using System.Data;
using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Data.Extensions;

namespace Project.Data.Mapping.Builders.Users;

/// <summary>
/// Represents user builder
/// </summary>
public class UserBuilder : ProjectEntityBuilder<User>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(User.Name)).AsString(250).NotNullable()
            .WithColumn(nameof(User.UserName)).AsString(250).Nullable()
            .WithColumn(nameof(User.Token)).AsString(500).Nullable()
            .WithColumn(nameof(User.Password)).AsString(250).NotNullable()
            .WithColumn(nameof(User.EmailAddress)).AsString(250).NotNullable()
            .WithColumn(nameof(User.MobileNumber)).AsString(50).NotNullable()
            .WithColumn(nameof(User.RoleId)).AsInt64().NotNullable().ForeignKey<UserRole>().OnDeleteOrUpdate(Rule.None)
            .WithColumn(nameof(User.ResetToken)).AsString().Nullable()
            .WithColumn(nameof(User.ResetTokenExpiry)).AsDateTime2().Nullable()
            .WithColumn(nameof(User.UserCode)).AsString().Nullable();
    }

    #endregion
}