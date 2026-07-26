namespace Project.Data.Mapping;

public partial class ProjectEntityDescriptor
{
    #region Constructor

    public ProjectEntityDescriptor()
    {
        Fields = new List<ProjectEntityFieldDescriptor>();
    }

    #endregion

    #region Fields
    public string EntityName { get; set; }
    public string SchemaName { get; set; }
    public ICollection<ProjectEntityFieldDescriptor> Fields { get; set; }

    #endregion
}