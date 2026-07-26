using ClosedXML.Excel;
using Project.Services.ExportImport.Help;

namespace Project.Services.ExportImport;

public partial class WorkbookMetadata<T>
{
    public List<PropertyByName<T>> DefaultProperties { get; set; }

    public List<PropertyByName<T>> LocalizedProperties { get; set; }

    public IXLWorksheet DefaultWorksheet { get; set; }

    public List<IXLWorksheet> LocalizedWorksheets { get; set; }
}