using System.Collections.Concurrent;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Expressions;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using Project.Core;
using Project.Core.Infrastructure;
using Project.Data.Extensions;
using Project.Data.Migrations;

namespace Project.Data.Mapping;

/// <summary>
/// Provides an access to entity mapping information
/// </summary>
public static class ProjectMappingSchema
{
    #region Fields

    private static ConcurrentDictionary<Type, ProjectEntityDescriptor> EntityDescriptors { get; } = new();

    #endregion

    /// <summary>
    /// Returns mapped entity descriptor
    /// </summary>
    /// <param name="entityType">Type of entity</param>
    /// <returns>Mapped entity descriptor</returns>
    public static ProjectEntityDescriptor GetEntityDescriptor(Type entityType)
    {
        if (!typeof(BaseEntity).IsAssignableFrom(entityType))
            return null;

        return EntityDescriptors.GetOrAdd(entityType, t =>
        {
            var tableName = NameCompatibilityManager.GetTableName(t);
            var expression = new CreateTableExpression { TableName = tableName };
            var builder = new CreateTableExpressionBuilder(expression, new NullMigrationContext());
            builder.RetrieveTableExpressions(t);

            return new ProjectEntityDescriptor
            {
                EntityName = tableName,
                SchemaName = builder.Expression.SchemaName,
                Fields = builder.Expression.Columns.Select(column => new ProjectEntityFieldDescriptor
                {
                    Name = column.Name,
                    IsPrimaryKey = column.IsPrimaryKey,
                    IsNullable = column.IsNullable,
                    Size = column.Size,
                    Precision = column.Precision,
                    IsIdentity = column.IsIdentity,
                    Type = column.Type ?? System.Data.DbType.String
                }).ToList()
            };
        });
    }

    /// <summary>
    /// Get or create mapping schema with specified configuration name
    /// </summary>
    public static MappingSchema GetMappingSchema(string configurationName, IDataProvider mappings)
    {

        if (Singleton<MappingSchema>.Instance is null)
        {
            Singleton<MappingSchema>.Instance = new MappingSchema(configurationName, mappings.MappingSchema);
            Singleton<MappingSchema>.Instance.AddMetadataReader(new FluentMigratorMetadataReader());
        }

        return Singleton<MappingSchema>.Instance;
    }
}
