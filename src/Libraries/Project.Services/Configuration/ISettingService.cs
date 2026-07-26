using System.Linq.Expressions;
using Project.Core.Configuration;
using Project.Core.Domain.Configuration;

namespace Project.Services.Configuration;

/// <summary>
/// Setting service interface
/// </summary>
public partial interface ISettingService
{
    /// <summary>
    /// Gets a setting by identifier
    /// </summary>
    /// <param name="settingId">Setting identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the setting
    /// </returns>
    Task<Setting> GetSettingByIdAsync(long settingId);

    /// <summary>
    /// Gets a setting by identifier
    /// </summary>
    /// <param name="settingId">Setting identifier</param>
    /// <returns>
    /// The setting
    /// </returns>
    Setting GetSettingById(long settingId);

    /// <summary>
    /// Deletes a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteSettingAsync(Setting setting);

    /// <summary>
    /// Deletes a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    void DeleteSetting(Setting setting);

    /// <summary>
    /// Deletes settings
    /// </summary>
    /// <param name="settings">Settings</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteSettingsAsync(IList<Setting> settings);

    /// <summary>
    /// Get setting by key
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the setting
    /// </returns>
    Task<Setting> GetSettingAsync(string key, bool loadSharedValueIfNotFound = false);

    /// <summary>
    /// Get setting by key
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
    /// <returns>
    /// The setting
    /// </returns>
    Setting GetSetting(string key, bool loadSharedValueIfNotFound = false);

    /// <summary>
    /// Get setting value by key
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">Default value</param>
    /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the setting value
    /// </returns>
    Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default, bool loadSharedValueIfNotFound = false);

    /// <summary>
    /// Get setting value by key
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">Default value</param>
    /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
    /// <returns>
    /// Setting value
    /// </returns>
    T GetSettingByKey<T>(string key, T defaultValue = default,
        bool loadSharedValueIfNotFound = false);

    /// <summary>
    /// Set setting value
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task SetSettingAsync<T>(string key, T value, bool clearCache = true);

    /// <summary>
    /// Set setting value
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    void SetSetting<T>(string key, T value,bool clearCache = true);

    /// <summary>
    /// Gets all settings
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the settings
    /// </returns>
    Task<IList<Setting>> GetAllSettingsAsync();

    /// <summary>
    /// Gets all settings
    /// </summary>
    /// <returns>
    /// Settings
    /// </returns>
    IList<Setting> GetAllSettings();

    /// <summary>
    /// Determines whether a setting exists
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TPropType">Property type</typeparam>
    /// <param name="settings">Settings</param>
    /// <param name="keySelector">Key selector</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the true -setting exists; false - does not exist
    /// </returns>
    Task<bool> SettingExistsAsync<T, TPropType>(T settings,
        Expression<Func<T, TPropType>> keySelector)
        where T : ISettings, new();

    /// <summary>
    /// Determines whether a setting exists
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TPropType">Property type</typeparam>
    /// <param name="settings">Settings</param>
    /// <param name="keySelector">Key selector</param>
    /// <returns>
    /// The true -setting exists; false - does not exist
    /// </returns>
    bool SettingExists<T, TPropType>(T settings,
        Expression<Func<T, TPropType>> keySelector)
        where T : ISettings, new();

    /// <summary>
    /// Load settings
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<T> LoadSettingAsync<T>() where T : ISettings, new();

    /// <summary>
    /// Load settings
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>Settings</returns>
    T LoadSetting<T>() where T : ISettings, new();

    /// <summary>
    /// Load settings
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<ISettings> LoadSettingAsync(Type type);

    /// <summary>
    /// Load settings
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>Settings</returns>
    ISettings LoadSetting(Type type);

    /// <summary>
    /// Save settings object
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="settings">Setting instance</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task SaveSettingAsync<T>(T settings) where T : ISettings, new();

    /// <summary>
    /// Save settings object
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="settings">Setting instance</param>
    void SaveSetting<T>(T settings) where T : ISettings, new();

    /// <summary>
    /// Save settings object
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TPropType">Property type</typeparam>
    /// <param name="settings">Settings</param>
    /// <param name="keySelector">Key selector</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task SaveSettingAsync<T, TPropType>(T settings,
        Expression<Func<T, TPropType>> keySelector,
        bool clearCache = true) where T : ISettings, new();

    /// <summary>
    /// Save settings object
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TPropType">Property type</typeparam>
    /// <param name="settings">Settings</param>
    /// <param name="keySelector">Key selector</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    void SaveSetting<T, TPropType>(T settings,
        Expression<Func<T, TPropType>> keySelector,
        bool clearCache = true) where T : ISettings, new();

    /// <summary>
    /// Adds a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertSettingAsync(Setting setting, bool clearCache = true);

    /// <summary>
    /// Adds a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    void InsertSetting(Setting setting, bool clearCache = true);

    /// <summary>
    /// Updates a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateSettingAsync(Setting setting, bool clearCache = true);

    /// <summary>
    /// Updates a setting
    /// </summary>
    /// <param name="setting">Setting</param>
    /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
    void UpdateSetting(Setting setting, bool clearCache = true);

    /// <summary>
    /// Delete all settings
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteSettingAsync<T>() where T : ISettings, new();

    /// <summary>
    /// Delete settings object
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TPropType">Property type</typeparam>
    /// <param name="settings">Settings</param>
    /// <param name="keySelector">Key selector</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteSettingAsync<T, TPropType>(T settings,
        Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();

    /// <summary>
    /// Clear cache
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task ClearCacheAsync();

    /// <summary>
    /// Clear cache
    /// </summary>
    void ClearCache();

    /// <summary>
    /// Get setting key (stored into database)
    /// </summary>
    /// <typeparam name="TSettings">Type of settings</typeparam>
    /// <typeparam name="T">Property type</typeparam>
    /// <param name="settings">Settings</param>
    /// <param name="keySelector">Key selector</param>
    /// <returns>Key</returns>
    string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector)
        where TSettings : ISettings, new();
}