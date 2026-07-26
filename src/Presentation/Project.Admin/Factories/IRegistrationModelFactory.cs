using Project.Admin.Area.Model;
using Project.Admin.Models.Registration;
using Project.Admin.Models.Users;
using Project.Core.Domain.Candidate;

namespace Project.Admin.Factories;

public interface IRegistrationModelFactory
{
    Task<UserAuthModel> PrepareStudentAuthModelAsync(RegistrationMaster student);

    //Task<RegistrationListModel> PrepareProjectGyaanRegistrationListModelAsync(RegistrationSearchModel searchModel, bool? onlyPaidUsers = null/*, long? batchId = null*/);

    Task<UserRegistrationModel> PrepareProjectGyaanRegistrationModelAsync(UserRegistrationModel model, RegistrationMaster registrationMaster, bool excludeProperties = false);

}
