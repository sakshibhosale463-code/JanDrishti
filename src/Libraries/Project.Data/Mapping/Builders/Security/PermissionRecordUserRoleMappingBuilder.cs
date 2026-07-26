using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Security;
using Project.Data.Extensions;

namespace Project.Data.Mapping.Builders.Security;

/// <summary>
/// Represents a permission record customer role mapping entity builder
/// </summary>
public partial class PermissionRecordUserRoleMappingBuilder : ProjectEntityBuilder<PermissionRecordUserRoleMapping>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(NameCompatibilityManager.GetColumnName(typeof(PermissionRecordUserRoleMapping), nameof(PermissionRecordUserRoleMapping.PermissionRecordId)))
            .AsInt64().PrimaryKey().ForeignKey<PermissionRecord>()
            .WithColumn(NameCompatibilityManager.GetColumnName(typeof(PermissionRecordUserRoleMapping), nameof(PermissionRecordUserRoleMapping.UserRoleId)))
            .AsInt64().PrimaryKey().ForeignKey<UserRole>();
    }

    #endregion
}