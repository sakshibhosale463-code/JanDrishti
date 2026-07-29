using AutoMapper;
using Project.Admin.Area.Model;
using Project.Admin.Models.Catalog;
using Project.Admin.Models.Registration;
using Project.Admin.Models.Users;
using Project.Core.Domain.Candidate;
using Project.Core.Domain.Catalog;
using Project.Core.Domain.Users;
using Project.Core.Infrastructure.Mapper;
using Project.Services.Registration.EntityModel;
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

    }

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 1;

    #endregion
}
