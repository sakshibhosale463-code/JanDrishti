namespace Project.Services.Common;

/// <summary>
/// Represents default values related to common services
/// </summary>
public static partial class ProjectCommonDefaults
{
    #region Localization client-side validation

    /// <summary>
    /// Gets a path to the localization client-side validation 
    /// </summary>
    public static string LocalePatternPath => "lib_npm/cldr-data/main/{0}";

    /// <summary>
    /// Gets a name of the archive with localization of templates
    /// </summary>
    public static string LocalePatternArchiveName => "main.zip";

    /// <summary>
    /// Gets a name of the default pattern locale
    /// </summary>
    public static string DefaultLocalePattern => "en";

    /// <summary>
    /// Gets default CultureInfo 
    /// </summary>
    public static string DefaultLanguageCulture => "en-US";

    /// <summary>
    /// Gets minimal progress of language pack translation to download and install
    /// </summary>
    public static int LanguagePackMinTranslationProgressToInstall => 80;

    /// <summary>
    /// Gets a name of generic attribute to store the value of 'LanguagePackProgress'
    /// </summary>
    public static string LanguagePackProgressAttribute => "LanguagePackProgress";

    #endregion

    #region Maintenance

    /// <summary>
    /// Gets a default timeout (in milliseconds) before restarting the application
    /// </summary>
    public static int RestartTimeout => 3000;

    /// <summary>
    /// Gets a path to the database backup files
    /// </summary>
    public static string DbBackupsPath => "db_backups\\";

    /// <summary>
    /// Gets a database backup file extension
    /// </summary>
    public static string DbBackupFileExtension => "bak";

    #endregion

    #region File paths

    /// <summary>
    /// Gets a location logo file path
    /// {0} file name
    /// </summary>
    public static string LocationLogoFilePath => "files/location/logo/{0}";

    /// <summary>
    /// Gets or sets the Wip bag number excel file path
    /// </summary>
    public static string WipBagNoExcelFilePath => "files/wip/bags/{0}";

    /// <summary>
    /// Gets or sets the Wip bag number txt file path
    /// </summary>
    public static string WipBagNoTxtFilePath => "files/wip/bags/{0}";

    /// <summary>
    /// Gets or sets the Wip order number excel file path
    /// </summary>
    public static string WipOrderNoExcelFilePath => "files/wip/orders/{0}";
    /// <summary>
    /// Gets or sets the Wip order number excel file path
    /// </summary>
    public static string WipDesignLabelFilePath => "files/wip/DesignLabel/{0}";
    public static string WipJobWorkDesignLabelFilePath => "files/wip/JobWorkDesignLabel/{0}";

    public static string TaskUpload => "files/ProjectGyaan/tasks/{0}";

    public static string SyllabusMaterial => "files/ProjectGyaan/syllabusMaterial/{0}";

    #endregion
}
