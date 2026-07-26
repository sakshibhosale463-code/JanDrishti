using AutoMapper;
using Project.Admin.Area.Model;
using Project.Admin.Area.Model.Session;
using Project.Admin.Models.Assessment;
using Project.Admin.Models.Attendance;
using Project.Admin.Models.Batch;
using Project.Admin.Models.Candidate;
using Project.Admin.Models.Catalog;
using Project.Admin.Models.DailyTask;
using Project.Admin.Models.Domain;
using Project.Admin.Models.Syllabus;
using Project.Admin.Models.Users;
using Project.Core.Domain.Assessment;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.DailyTask;
using Project.Core.Domain.Syllabus;
using Project.Core.Domain.Users;
using Project.Core.Infrastructure.Mapper;
using Project.Services.Candidate.EntityModel;
using Project.Services.Users.EntityModel;

namespace Project.Admin.Infrastructure.Mapper;

/// <summary>
/// AutoMapper configuration for admin area models
/// </summary>
public partial class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public MapperConfiguration()
    {
        //create specific maps
        CreateUserMaps();
        CreateCatalogMaps();
        CreateCandidateMaps();
        CreateDomain();
        Master();
        Batch();
        Syllabus();
        Assessment();
    }

    #endregion

    #region Utilities

    protected virtual void CreateUserMaps()
    {
        CreateMap<User, UserModel>();
        CreateMap<User, ResetPasswordRequestModel>().ReverseMap();
        CreateMap<User, UserAuthModel>();
        CreateMap<RegistrationMaster, UserAuthModel>();

        CreateMap<RegistrationMaster, UserRegistrationModel>();
        CreateMap<UserRegistrationModel, RegistrationMaster>()
        .ForMember(entity => entity.CreatedBy, options => options.Ignore());

        CreateMap<UserEntityModel, UserModel>();

        CreateMap<UserModel, User>()
            .ForMember(entity => entity.Deleted, options => options.Ignore())
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());

        CreateMap<User, UserProfileModel>().ReverseMap();

        CreateMap<UserProfileModel, RegistrationMaster>();
        CreateMap<RegistrationMaster, UserProfileModel>();
    }

    protected virtual void CreateCatalogMaps()
    {
        //user roles
        CreateMap<UserRole, UserRoleModel>();
        CreateMap<UserRoleModel, UserRole>()
            .ForMember(entity => entity.Deleted, options => options.Ignore())
            .ForMember(entity => entity.SystemName, options => options.Ignore())
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());



        //departments
        CreateMap<Department, DepartmentModel>();
        CreateMap<DepartmentModel, Department>()
            .ForMember(entity => entity.Deleted, options => options.Ignore())
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());
    }

    protected virtual void CreateCandidateMaps()
    {
        CreateMap<RegistrationMaster, UserRegistrationModel>().ReverseMap();
        CreateMap<RegistrationModel, UserRegistrationModel>().ReverseMap();
        CreateMap<AttendanceEntityModel, AttendanceModel>().ReverseMap();
        CreateMap<CollegeEnquiryForm, CollegeEnquiryFormModel>().ReverseMap();
        CreateMap<CollegeEnquiryDomainMapping, CollegeEnquiryFormModel>().ReverseMap();

    }


    protected virtual void CreateDomain()
    {
        CreateMap<DomainMaster, DomainModel>().ReverseMap();
        CreateMap<Sessions, SessionModel>().ReverseMap();
    }

    protected virtual void Master()
    {
        CreateMap<DailyTaskMaster, DailyTaskModel>().ReverseMap();
        CreateMap<TaskSubmissionMaster, TaskSubmissionModel>().ReverseMap();
        CreateMap<TaskSubmissionModel, TaskSubmissionMaster>().ReverseMap();
        CreateMap<ViewProgressModel, TaskSubmissionMaster>().ReverseMap();

    }
    protected virtual void Batch()
    {
        CreateMap<BatchMaster, BatchModel>().ReverseMap()
             .ForMember(dest => dest.SessionDays, opt => opt.Ignore());
        CreateMap<BatchEnrollmentMaster, CandidateBatchEnrollementModel>();
    }

    protected virtual void Syllabus()
    {
        CreateMap<SyllabusMaster, SyllabusModel>().ReverseMap();
        CreateMap<SyllabusModule, SyllabusModel>().ReverseMap();
        CreateMap<SyllabusTopic, SyllabusModel>().ReverseMap();
    }

    protected virtual void Assessment()
    {
        CreateMap<AssessmentMaster, AssessmentRequestModel>().ReverseMap();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 1;

    #endregion
}
