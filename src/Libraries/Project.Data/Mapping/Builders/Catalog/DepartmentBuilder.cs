using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Catalog;

namespace Project.Data.Mapping.Builders.Catalog;

/// <summary>
/// Represents a location entity builder
/// </summary>
public class DepartmentBuilder : ProjectEntityBuilder<Department>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Department.Name)).AsString(250).NotNullable()
            .WithColumn(nameof(Department.Code)).AsString(250).NotNullable()
            .WithColumn(nameof(Department.Remark)).AsString(500).Nullable();
    }

    #endregion
}
