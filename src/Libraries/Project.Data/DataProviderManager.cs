using Project.Core;
using Project.Core.Infrastructure;
using Project.Data.Configuration;
using Project.Data.DataProviders;

namespace Project.Data;

/// <summary>
/// Represents the data provider manager
/// </summary>
public partial class DataProviderManager : IDataProviderManager
{
    #region Methods

    /// <summary>
    /// Gets data provider by specific type
    /// </summary>
    /// <param name="dataProviderType">Data provider type</param>
    /// <returns></returns>
    public static IProjectDataProvider GetDataProvider(DataProviderType dataProviderType)
    {
        return dataProviderType switch
        {
            DataProviderType.SqlServer => new MsSqlNopDataProvider(),
            DataProviderType.MySql => new MySqlNopDataProvider(),
            DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
            _ => throw new ProjectException($"Not supported data provider name: '{dataProviderType}'"),
        };
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets data provider
    /// </summary>
    public IProjectDataProvider DataProvider
    {
        get
        {
            var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

            return GetDataProvider(dataProviderType);
        }
    }

    #endregion
}