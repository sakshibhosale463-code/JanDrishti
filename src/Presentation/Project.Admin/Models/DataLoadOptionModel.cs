
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace Project.Admin.Models;
/// <summary>
/// Represents wip data load option model
/// </summary>
[ModelBinder(BinderType = typeof(DataLoadOptionsBinder))]
public class DataLoadOptionModel : DataSourceLoadOptionsBase
{
}
