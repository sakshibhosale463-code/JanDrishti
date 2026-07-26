using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Catalog;

namespace Project.Data.Mapping.Builders.Catalog;

/// <summary>
/// Represents a user role entity builder
/// </summary>
public class UserRoleBuilder : ProjectEntityBuilder<UserRole>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table) => table
        .WithColumn(nameof(UserRole.Name)).AsString(100).NotNullable();

    #endregion
}
