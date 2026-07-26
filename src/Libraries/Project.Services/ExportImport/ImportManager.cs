using ClosedXML.Excel;
using Project.Core;
using Project.Core.Domain.Catalog; 
using Project.Core.Domain.Users; 
using Project.Services.ExportImport.Help;

namespace Project.Services.ExportImport
{
    /// <summary>
    /// Import manager interface
    /// </summary>
    public class ImportManager : IImportManager
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
 
        #endregion

        #region Constructor

        public ImportManager(CatalogSettings catalogSettings )
        {
            _catalogSettings = catalogSettings;
         }

        #endregion

        #region Methods

        /// <summary>
        /// Get excel workbook metadata
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="workbook">Excel workbook</param>
        /// <param name="languages">Languages</param>
        /// <returns>Workbook metadata</returns>
        public static WorkbookMetadata<T> GetWorkbookMetadata<T>(IXLWorkbook workbook)
        {
            // get the first worksheet in the workbook
            var worksheet = workbook.Worksheets.FirstOrDefault()
                            ?? throw new ProjectException("No worksheet found");

            var properties = new List<PropertyByName<T>>();
            var localizedProperties = new List<PropertyByName<T>>();
            var localizedWorksheets = new List<IXLWorksheet>();

            var poz = 1;
            while (true)
            {
                try
                {
                    var cell = worksheet.Row(1).Cell(poz);

                    if (string.IsNullOrEmpty(cell?.Value.ToString()))
                        break;

                    poz += 1;
                    properties.Add(new PropertyByName<T>(cell.Value.ToString()));
                }
                catch
                {
                    break;
                }
            }

            foreach (var ws in workbook.Worksheets.Skip(1))
                localizedWorksheets.Add(ws);

            if (localizedWorksheets.Any())
            {
                // get the first worksheet in the workbook
                var localizedWorksheet = localizedWorksheets.First();

                poz = 1;
                while (true)
                {
                    try
                    {
                        var cell = localizedWorksheet.Row(1).Cell(poz);

                        if (string.IsNullOrEmpty(cell?.Value.ToString()))
                            break;

                        poz += 1;
                        localizedProperties.Add(new PropertyByName<T>(cell.Value.ToString()));
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            return new WorkbookMetadata<T>
            {
                DefaultProperties = properties,
                LocalizedProperties = localizedProperties,
                DefaultWorksheet = worksheet,
                LocalizedWorksheets = localizedWorksheets
            };
        }

        /// <summary>
        /// Import manufacturers from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task ImportUsersFromXlsxAsync(Stream stream)
        {
            using var workbook = new XLWorkbook(stream);

            //the columns
            var metadata = GetWorkbookMetadata<User>(workbook);
            var defaultWorksheet = metadata.DefaultWorksheet;
            var defaultProperties = metadata.DefaultProperties;
            var localizedProperties = metadata.LocalizedProperties;

            var manager = new PropertyManager<User>(defaultProperties, _catalogSettings, localizedProperties);

            var iRow = 2;

            while (true)
            {
                var allColumnsAreEmpty = manager.GetDefaultProperties
                    .Select(property => defaultWorksheet.Row(iRow).Cell(property.PropertyOrderPosition))
                    .All(cell => cell?.Value == null || string.IsNullOrEmpty(cell.Value.ToString()));

                if (allColumnsAreEmpty)
                    break;

                manager.ReadDefaultFromXlsx(defaultWorksheet, iRow);

                var user = new User();

                foreach (var property in manager.GetDefaultProperties)
                {
                    switch (property.PropertyName)
                    {
                        case "Name":
                            user.Name = property.StringValue;
                            break;
                        case "User Name":
                            user.UserName = property.StringValue;
                            break;
                    }
                }

                iRow++;
            }
            
            //insert code
            return Task.CompletedTask;
        }

        //public async Task<(List<DiscountVoucherCode> ValidList, List<string> ErrorList)> ImportDiscountVoucherFromXlsxAsync(Stream stream, long createdBy, long discoutVouId)
        //{
        //    using var workbook = new XLWorkbook(stream);

        //    // Columns
        //    var metadata = GetWorkbookMetadata<DiscountVoucherCode>(workbook);
        //    var defaultWorksheet = metadata.DefaultWorksheet;
        //    var defaultProperties = metadata.DefaultProperties;
        //    var localizedProperties = metadata.LocalizedProperties;

        //    List<DiscountVoucherCode> validDiscountVoucherList = new List<DiscountVoucherCode>();
        //    List<string> errorList = new List<string>();

        //    var manager = new PropertyManager<DiscountVoucherCode>(defaultProperties, _catalogSettings, localizedProperties);

        //    var iRow = 2;

        //    while (true)
        //    {
        //        var allColumnsAreEmpty = manager.GetDefaultProperties
        //            .Select(property => defaultWorksheet.Row(iRow).Cell(property.PropertyOrderPosition))
        //            .All(cell => cell?.Value == null || string.IsNullOrEmpty(cell.Value.ToString()));

        //        if (allColumnsAreEmpty)
        //            break;

        //        manager.ReadDefaultFromXlsx(defaultWorksheet, iRow);
        //        var codeModel = new DiscountVoucherCode();
        //        bool hasError = false;

        //        foreach (var property in manager.GetDefaultProperties)
        //        {
        //            switch (property.PropertyName)
        //            {
        //                case "VoucherCode":
        //                    codeModel.VoucherCode = property.StringValue?.Trim();

        //                    if (string.IsNullOrEmpty(codeModel.VoucherCode))
        //                    {
        //                        errorList.Add($"Row {iRow}: Voucher Code is empty.");
        //                        hasError = true;
        //                    }
        //                    else if (await _discountVoucherService.HasCouponCodeAlredyExist(codeModel.VoucherCode))
        //                    {
        //                        errorList.Add($"Row {iRow}: Voucher Code '{codeModel.VoucherCode}' already exists.");
        //                        hasError = true;
        //                    }
        //                    break;
        //            }
        //        }

        //        if (!hasError)
        //        {
        //            codeModel.DiscountVoucherId = discoutVouId;
        //            codeModel.IsRedeem = false;
        //            codeModel.CreatedBy = createdBy;
        //            codeModel.CreatedOn = DateTime.UtcNow;

        //            validDiscountVoucherList.Add(codeModel);
        //        }

        //        iRow++;
        //    }

        //    if (validDiscountVoucherList.Any())
        //        await _discountVoucherService.InsertDiscountVoucherCodeList(validDiscountVoucherList);

        //    return (validDiscountVoucherList, errorList);
        //}

        #endregion
    }
}
