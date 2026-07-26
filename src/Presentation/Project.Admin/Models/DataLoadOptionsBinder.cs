using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Project.Admin.Models;
/// <summary>
/// Wip data source load option binder
/// </summary>
public class DataLoadOptionsBinder : IModelBinder
{
    #region Methods

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var loadOptions = new DataLoadOptionModel();
        DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
        bindingContext.Result = ModelBindingResult.Success(loadOptions);
        return Task.CompletedTask;
    }

    #endregion
}