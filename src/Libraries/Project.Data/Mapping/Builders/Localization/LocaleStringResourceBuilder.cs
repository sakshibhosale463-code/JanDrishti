using FluentMigrator.Builders.Create.Table;
using Project.Core.Domain.Localization;
using Project.Data.Extensions;

namespace Project.Data.Mapping.Builders.Localization;

/// <summary>
/// Represents a locale string resource entity builder
/// </summary>
public partial class LocaleStringResourceBuilder : ProjectEntityBuilder<LocaleStringResource>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LocaleStringResource.ResourceName)).AsString(200).NotNullable()
            .WithColumn(nameof(LocaleStringResource.ResourceValue)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(LocaleStringResource.LanguageId)).AsInt64().ForeignKey<Language>();
    }

    #endregion
}