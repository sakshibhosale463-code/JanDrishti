using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Users;
using Project.Data.Extensions;

namespace Project.Data.Mapping.Builders.Users;
public class UserDomainMappingBuilder : ProjectEntityBuilder<UserDomainMapping>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(NameCompatibilityManager.GetColumnName(typeof(UserDomainMapping), nameof(UserDomainMapping.UserId)))
            .AsInt64().PrimaryKey().ForeignKey<User>()
            .WithColumn(NameCompatibilityManager.GetColumnName(typeof(UserDomainMapping), nameof(UserDomainMapping.DomainId)))
            .AsInt64().PrimaryKey().ForeignKey<DomainMaster>();
    }

    #endregion
}