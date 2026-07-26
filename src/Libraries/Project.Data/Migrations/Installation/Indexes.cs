using FluentMigrator;
using Project.Data.Mapping;

namespace Project.Data.Migrations.Installation;

[ProjectSchemaMigration("2021/03/10 11:24:16:2551779", "Nop.Data base indexes", MigrationProcessType.Installation)]
public class Indexes : ForwardOnlyMigration
{
    public override void Up()
    {
        //Create.Index("UKey_TagName_TagBIdNo_UsrCd").OnTable(NameCompatibilityManager.GetTableName(typeof(BagList)))
        //    .OnColumn(nameof(BagList.TagName)).Ascending()
        //    .OnColumn(nameof(BagList.TagBIdNo)).Ascending()
        //    .OnColumn(nameof(BagList.UsrCd)).Ascending()
        //    .WithOptions().NonClustered();


        //Create.Index("UKey_TagName_OdIdNo_UsrCd").OnTable(NameCompatibilityManager.GetTableName(typeof(OrdList)))
        //   .OnColumn(nameof(OrdList.TagName)).Ascending()
        //   .OnColumn(nameof(OrdList.OdIdNo)).Ascending()
        //   .OnColumn(nameof(OrdList.UsrCd)).Ascending()
        //   .WithOptions().NonClustered();

        //Create.Index("UQKey_WorkCd").OnTable(NameCompatibilityManager.GetTableName(typeof(InsMstWork)))
        //  .OnColumn(nameof(OrdList.TagName)).Ascending()
        //  .WithOptions().NonClustered();

        //Create.Index("UQKey_StruRcCrmCustCd").OnTable(NameCompatibilityManager.GetTableName(typeof(StruCustRcCrm)))
        //  .OnColumn(nameof(StruCustRcCrm.StruRcCrmCustCd)).Ascending()
        //  .WithOptions().NonClustered();
    }
}
