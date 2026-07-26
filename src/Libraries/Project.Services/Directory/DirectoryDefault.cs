using Project.Core.Caching;

namespace Project.Services.Directory
{
    public static partial class DirectoryDefault
    {
        public static CacheKey CitySelectDataAllCacheKey => new("Project.City.select.all.{0}");
        public static string CitySelectDataAllCacheKeyPrefix => "Project.City.select.all.{0}";

        public static CacheKey StateProvinceSelectDataAllCacheKey => new("Project.StateProvince.select.all.{0}");
        public static string StateProvinceSelectDataAllCacheKeyPrefix => "Project.StateProvince.select.all.{0}";

        public static CacheKey CountrySelectDataAllCacheKey => new("Project.Country.select.all.{}");
        public static string CountrySelectDataAllCacheKeyPrefix => "Project.Country.select.all.{}";

        public static CacheKey MetalSelectDataAllCacheKey => new("Project.Metal.select.all.{}");
        public static string MetalSelectDataAllCacheKeyPrefix => "Project.Metal.select.all.{}";

        public static CacheKey KaratSelectDataAllCacheKey => new("Project.Karat.select.all.{}");
        public static string KaratSelectDataAllCacheKeyPrefix => "Project.Karat.select.all.{}";

     
        public static CacheKey CategorySelectDataAllCacheKey => new("Project.Category.select.all.{}");
        public static string CategorySelectDataAllCacheKeyPrefix => "Project.Category.select.all.{}";

        public static CacheKey CollectionSelectDataAllCacheKey => new("Project.Collection.select.all.{}");
        public static string CollectionSelectDataAllCacheKeyPrefix => "Project.Collection.select.all.{}";

        public static CacheKey CompanySelectDataAllCacheKey => new("Project.Company.select.all.{}");
        public static string CompanySelectDataAllCacheKeyPrefix => "Project.Company.select.all.{}";

        public static CacheKey DiamondShapeSelectDataAllCacheKey => new("Project.DiamondShape.select.all.{}");
        public static string DiamondShapeSelectDataAllCacheKeyPrefix => "Project.DiamondShape.select.all.{}";

        public static CacheKey MetalColorSelectDataAllCacheKey => new("Project.MetalColor.select.all.{}");
        public static string MetalColorSelectDataAllCacheKeyPrefix => "Project.MetalColor.select.all.{}";

        public static CacheKey MaterialTypeSelectDataAllCacheKey => new("Project.MaterialType.select.all.{}");
        public static string MaterialTypeSelectDataAllCacheKeyPrefix => "Project.MaterialType.select.all.{}";

        public static CacheKey UserRoleSelectDataAllCacheKey => new("Project.UserRole.select.all.{}");
        public static string UserRoleSelectDataAllCacheKeyPrefix => "Project.UserRole.select.all.{}";

        public static CacheKey StoreTypeSelectDataAllCacheKey => new("Project.StoreType.select.all.{}");
        public static string StoreTypeSelectDataAllCacheKeyPrefix => "Project.StoreType.select.all.{}";

        public static CacheKey StoreSelectDataAllCacheKey => new("Project.Store.select.all.{}");
        public static string StoreSelectDataAllCacheKeyPrefix => "Project.Store.select.all.{}";

        public static CacheKey StoreOpsRoleSelectDataAllCacheKey => new("Project.StoreOpsRole.select.all.{}");
        public static string StoreOpsRoleSelectDataAllCacheKeyPrefix => "Project.StoreOpsRole.select.all.{}";

        public static CacheKey InvoiceAmtSlabSelectDataAllCacheKey => new("Project.InvoiceAmtSlab.select.all.{}");
        public static string InvoiceAmtSlabSelectDataAllCacheKeyPrefix => "Project.InvoiceAmtSlab.select.all.{}";

        public static CacheKey WalkInSourceSelectDataAllCacheKey => new("Project.WalkInSource.select.all.{}");
        public static string WalkInSourceSelectDataAllCacheKeyPrefix => "Project.WalkInSource.select.all.{}";


        public static CacheKey MrpTypeSelectDataAllCacheKey => new("Project.MrpType.select.all.{}");
        public static string MrpTypeSelectDataAllCacheKeyPrefix => "Project.MrpType.select.all.{}";
        
        public static CacheKey DiscountOnSRPSelectDataAllCacheKey => new("Project.DiscountOnSRP.select.all.{}");
        public static string DiscountOnSRPSelectDataAllCacheKeyPrefix => "Project.DiscountOnSRP.select.all.{}";

        public static CacheKey DiscountOnMRPSelectDataAllCacheKey => new("Project.DiscountOnMRP.select.all.{}");
        public static string DiscountOnMRPSelectDataAllCacheKeyPrefix => "Project.DiscountOnMRP.select.all.{}";
        
        public static CacheKey DiscountTypeSelectDataAllCacheKey => new("Project.DiscountType.select.all.{}");
        public static string DiscountTypeSelectDataAllCacheKeyPrefix => "Project.DiscountType.select.all.{}";


  public static CacheKey PaymentTypeSelectDataAllCacheKey => new("Project.PaymentType.select.all.{}");
        public static string PaymentTypeSelectDataAllCacheKeyPrefix => "Project.PaymentType.select.all.{}";

    }
}
