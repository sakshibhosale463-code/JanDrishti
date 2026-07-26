using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Core.Infrastructure;
using Project.Services.Common.DataModel;
using Project.Services.Localization;
using Project.Web.Framework.Models;

namespace Project.Web.Framework.Helpers
{
    /// <summary>
    /// Select list helper
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Get localized select list item from enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static async Task<List<SelectListModel>> GetLocalizedSelectListAsync<TEnum>(bool addDefault = false) where TEnum : struct
        {
            //prepare select list
            var selectListItems = Enum.GetValues(typeof(TEnum)).OfType<Enum>().Select(item =>
            {
                var text = Enum.GetName(typeof(TEnum), item);

                var selectListItem = new SelectListModel()
                {
                    Name = text,
                    Id = Convert.ToInt32(item)
                };

                return selectListItem;
            }).ToList();

            //get localized values for select list
            if (selectListItems != null && selectListItems.Count > 0)
            {
                var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                foreach (var item in selectListItems)
                {
                    item.Name = await localizationService.GetResourceAsync($"{typeof(TEnum).Name}.{item.Name}");
                }
            }

            return selectListItems;
        }
    }
}
