using FluentMigrator;
using Project.Core.Domain.Users;
using Project.Data.Extensions;
namespace Project.Data.Migrations.Installation;


[ProjectSchemaMigration("2026/06/25 11:43:18:8899799", "Nop.Data base schema", MigrationProcessType.Installation)]
public class SchemaMigration : ForwardOnlyMigration
{

    /// <summary>
    /// Collect the UP migration expressions
    /// <remarks>
    /// We use an explicit table creation order instead of an automatic one
    /// due to problems creating relationships between tables
    /// </remarks>
    /// </summary>
    public override void Up()
    {
        //Create.TableFor<Country>();
        //Create.TableFor<Currency>();
        //Create.TableFor<StateProvince>();
        //Create.TableFor<Language>();
        //Create.TableFor<Setting>();
        //Create.TableFor<LocaleStringResource>();
        //Create.TableFor<LocalizedProperty>();
        //Create.TableFor<UserRole>();
        //Create.TableFor<User>();
        //Create.TableFor<PermissionRecord>();
        //Create.TableFor<PermissionRecordUserRoleMapping>();
        //Create.TableFor<Log>();
        //Create.TableFor<ScheduleTask>();
        //Create.TableFor<UserPermissionRecordMapping>();
        //Create.TableFor<MessageTemplate>();


        //Alter.Table(nameof(RegistrationMaster))
        //      .AddColumn(nameof(RegistrationMaster.Deleted)).AsBoolean().Nullable();




    }
}