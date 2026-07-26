using AutoMapper;
using AutoMapper.Internal;
using Project.Core.Configuration;
using Project.Core.Infrastructure.Mapper;
using Project.Services.Common.DataModel;
using Project.Web.Framework.Models;

namespace Project.Web.Framework.Infrastructure.Mapper;
/// <summary>
/// AutoMapper configuration for admin area models
/// </summary>
public partial class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public MapperConfiguration()
    {
        //create specific maps
        CreateCommonMaps();

        //add some generic mapping rules
        this.Internal().ForAllMaps((mapConfiguration, map) =>
        {
            //exclude some properties from mapping configuration and models
            if (typeof(IConfig).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(IConfig.Name), options => options.Ignore());
        });
    }

    #endregion

    #region Utilities

    protected virtual void CreateCommonMaps()
    {
        CreateMap<SelectListDataModel, SelectListModel>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 0;

    #endregion
}