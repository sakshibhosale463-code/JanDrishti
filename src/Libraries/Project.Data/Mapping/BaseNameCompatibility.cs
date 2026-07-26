namespace Project.Data.Mapping;

/// <summary>
/// Base instance of backward compatibility of table naming
/// </summary>
public partial class BaseNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
    };

    public Dictionary<Type, string> ViewNames => new()
    {
    };

    public Dictionary<Type, string> DatabaseNames => new()
    {
    };

    public Dictionary<(Type, string), string> ColumnName => new()
    {
    };
}
