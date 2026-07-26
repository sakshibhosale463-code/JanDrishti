using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core
{
    public static class SQSGroup
    {
        /// <summary>
        /// ecom to central
        /// </summary>

        #region Customer

        public const string ECOMTOCENTRALINSERTCUSTOMER = "ecom-to-central:insert-customer";
        public const string ECOMTOCENTRALUPDATECUSTOMER = "ecom-to-central:update-customer";
        public const string ECOMTOCENTRALDELETECUSTOMER = "ecom-to-central:delete-customer";

        #endregion

        #region Product

        public const string ECOMTOCENTRALINSERTRETURNTOMAYAVEPRODUCT = "ecom-to-central:insert-return-to-mayave-product";
        public const string ECOMTOCENTRALUPDATERETURNTOMAYAVEPRODUCTSPRICE = "ecom-to-central:update-return-to-mayave-products-price";
        public const string ECOMTOCENTRALUPDATEPRODUCTHEADERQTYANDSTORECODEFORRETURNTOMAYAVE = "ecom-to-central:update-product-header-qty-and-store-code-for-return-to-mayave-product";


        #endregion

        #region CustomerReferral
        public const string ECOMTOCENTRALDELETECUSTOMERREFERRAL = "ecom-to-central:delete-customer-referral";
        public const string ECOMTOCENTRALINSERTCUSTOMERREFERRAL = "ecom-to-central:insert-customer-referral";
        public const string ECOMTOCENTRALUPDATECUSTOMERREFERRAL = "ecom-to-central:update-customer-referral";
        public const string CENTRALTOECOMINSERTOCUSTOMERREFERRALUPDATEID = "central-to-ecom:updated-centaralId-customer-referral";
        #endregion


        /// <summary>
        /// sqs from pos to central 
        /// </summary>

        #region salesperson

        public const string POSTOCENTRALINSERTSALESPERSON = "pos-to-central:insert-sales-person";
        public const string POSTOCENTRALUPDATESALESPERSON = "pos-to-central:update-sales-person";
        public const string POSTOCENTRALDELETESALESPERSON = "pos-to-central:delete-sales-person";
        

        #endregion

        #region Customer

        public const string POSTOCENTRALINSERTCUSTOMER = "pos-to-central:insert-customer";
        public const string POSTOCENTRALUPDATECUSTOMER = "pos-to-central:update-customer";
        public const string POSTOCENTRALDELETECUSTOMER = "pos-to-central:delete-customer";


        #endregion

        #region StorePolicy

         public const string POSTOCENTRALUPDATESTOREPOLICY = "pos-to-central:update-store-policy";

        #endregion

        /// <summary>
        /// sqs from central to pos and ecom
        /// </summary>
        /// 

        #region StorePolicy

        public const string CENTRALTOPOSINSERTSTOREPOLICY = "central-to-pos:insert-store-policy";
        public const string CENTRALTOECOMINSERTSTOREPOLICY = "central-to-ecom:insert-store-policy";
        public const string CENTRALTOPOSUPDATESTOREPOLICY = "central-to-pos:update-store-policy";
        public const string CENTRALTOECOMUPDATESTOREPOLICY = "central-to-ecom:update-store-policy";

        #endregion

        #region UserManagement

        public const string CENTRALTOPOSINSERTUSER = "central-to-pos:insert-user";
        public const string CENTRALTOPOSDELETEUSER = "central-to-pos:delete-user";
        public const string CENTRALTOPOSUPDATEUSER = "central-to-pos:update-user";

        public const string CENTRALTOPOSINSERTUSERROLE = "central-to-pos:insert-user-role";
        public const string CENTRALTOPOSUPDATEUSERROLE = "central-to-pos:update-user-role";
        public const string CENTRALTOPOSDELETEUSERROLE = "central-to-pos:delete-user-role";

        #endregion

        #region Store
        public const string CENTRALTOPOSINSERTSTORE = "central-to-pos:insert-store";
        public const string CENTRALTOECOMINSERTSTORE = "central-to-ecom:insert-store";
        public const string CENTRALTOFIX360INSERTSTORE = "central-to-fix360:insert-store";
        public const string CENTRALTODAILYXINSERTSTORE = "central-to-dailyx:insert-store";

        public const string CENTRALTOPOSUPDATESTORE = "central-to-pos:update-store";
        public const string CENTRALTOECOMUPDATESTORE = "central-to-ecom:update-store";
        public const string CENTRALTOFIX360UPDATESTORE = "central-to-fix360:update-store";
        public const string CENTRALTODAILYXUPDATESTORE = "central-to-dailyx:update-store";

        public const string CENTRALTOPOSDELETESTORE = "central-to-pos:delete-store";
        public const string CENTRALTOECOMDELETESTORE = "central-to-ecom:delete-store";
        public const string CENTRALTOFIX360DELETESTORE = "central-to-fix360:delete-store";
        public const string CENTRALTODAILYXDELETESTORE = "central-to-dailyx:delete-store";

        public const string CENTRALTOPOSINSERTSTOREOPSROLE = "central-to-pos:insert-store-ops-role";
        public const string CENTRALTOECOMINSERTSTOREOPSROLE = "central-to-ecom:insert-store-ops-role";
        public const string CENTRALTOPOSUPDATESTOREOPSROLE = "central-to-pos:update-store-ops-role";
        public const string CENTRALTOECOMUPDATESTOREOPSROLE = "central-to-ecom:update-store-ops-role";
        public const string CENTRALTOPOSDELETESTOREOPSROLE = "central-to-pos:delete-store-ops-role";
        public const string CENTRALTOECOMDELETESTOREOPSROLE = "central-to-ecom:delete-store-ops-role";
        #endregion

        #region Sales
        public const string CENTRALTOPOSINSERTSALESPERSON = "central-to-pos:insert-sales-person";
        public const string CENTRALTOPOSINSERTSALESPERSONUPDATEID = "central-to-pos:insert-sales-person-update-id";
        public const string CENTRALTOECOMINSERTSALESPERSON = "central-to-ecom:insert-sales-person";
        public const string CENTRALTOPOSUPDATESALESPERSON = "central-to-pos:update-sales-person";
        public const string CENTRALTOECOMUPDATESALESPERSON = "central-to-ecom:update-sales-person";
        public const string CENTRALTOPOSDELETESALESPERSON = "central-to-pos:delete-sales-person";
        public const string CENTRALTOECOMDELETESALESPERSON = "central-to-ecom:delete-sales-person";
        #endregion

        #region BillSetting
        public const string CENTRALTOPOSINSERTBILLTYPE = "central-to-pos:insert-bill-type";
        public const string CENTRALTOECOMINSERTBILLTYPE = "central-to-ecom:insert-bill-type";
        public const string CENTRALTOPOSUPDATEBILLTYPE = "central-to-pos:update-bill-type";
        public const string CENTRALTOECOMUPDATEBILLTYPE = "central-to-ecom:update-bill-type";
        public const string CENTRALTOPOSDELETEBILLTYPE = "central-to-pos:delete-bill-type";
        public const string CENTRALTOECOMDELETEBILLTYPE = "central-to-ecom:delete-bill-type";

        public const string CENTRALTOPOSINSERTBILLSETTING = "central-to-pos:insert-bill-setting";
        public const string CENTRALTOECOMINSERTBILLSETTING = "central-to-ecom:insert-bill-setting";
        public const string CENTRALTOPOSDELETEBILLSETTING = "central-to-pos:delete-bill-setting";
        public const string CENTRALTOECOMDELETEBILLSETTING = "central-to-ecom:delete-bill-setting";

        public const string POSTOCENTRALUPDATEBILLSETTING = "pos-to-central:update-bill-setting";
        public const string CENTRALTOECOMUPDATEBILLSETTING = "central-to-ecom:update-bill-setting";

        #endregion

        #region Directory
        public const string CENTRALTOPOSINSERTCOUNTRY = "central-to-pos:insert-country";
        public const string CENTRALTOECOMINSERTCOUNTRY = "central-to-ecom:insert-country";
        public const string CENTRALTOPOSUPDATECOUNTRY = "central-to-pos:update-country";
        public const string CENTRALTOECOMUPDATECOUNTRY = "central-to-ecom:update-country";
        public const string CENTRALTOPOSDELETECOUNTRY = "central-to-pos:delete-country";
        public const string CENTRALTOECOMDELETECOUNTRY = "central-to-ecom:delete-country";

        public const string CENTRALTOPOSINSERTSTATE = "central-to-pos:insert-state";
        public const string CENTRALTOECOMINSERTSTATE = "central-to-ecom:insert-state";
        public const string CENTRALTOPOSUPDATESTATE = "central-to-pos:update-state";
        public const string CENTRALTOECOMUPDATESTATE = "central-to-ecom:update-state";
        public const string CENTRALTOPOSDELETESTATE = "central-to-pos:delete-state";
        public const string CENTRALTOECOMDELETESTATE = "central-to-ecom:delete-state";

        public const string CENTRALTOPOSINSERTCITY = "central-to-pos:insert-city";
        public const string CENTRALTOECOMINSERTCITY = "central-to-ecom:insert-city";
        public const string CENTRALTOPOSUPDATECITY = "central-to-pos:update-city";
        public const string CENTRALTOECOMUPDATECITY = "central-to-ecom:update-city";
        public const string CENTRALTOPOSDELETECITY = "central-to-pos:delete-city";
        public const string CENTRALTOECOMDELETCITY = "central-to-ecom:delete-city";
        #endregion

        #region ConsumerOffers
        public const string CENTRALTOPOSINSERTDISCOUNT = "central-to-pos:insert-ConsumerOffers";
        public const string CENTRALTOECOMINSERTDISCOUNT = "central-to-ecom:insert-ConsumerOffers";
        public const string CENTRALTOPOSUPDATEDISCOUNT = "central-to-pos:update-ConsumerOffers";
        public const string CENTRALTOECOMUPDATEDISCOUNT = "central-to-ecom:update-ConsumerOffers";
        public const string CENTRALTOPOSDELETEDISCOUNT = "central-to-pos:delete-ConsumerOffers";
        public const string CENTRALTOECOMDELETEDISCOUNT = "central-to-ecom:delete-ConsumerOffers";
        #endregion

        #region GoldMakingCharges
        public const string CENTRALTOPOSINSERTGOLDMAKINGCHARGES = "central-to-pos:insert-gold-making-charges";
        public const string CENTRALTOECOMINSERTGOLDMAKINGCHARGES = "central-to-ecom:insert-gold-making-charges";       
        public const string CENTRALTOPOSUPDATEGOLDMAKINGCHARGES = "central-to-pos:update-gold-making-charges";
        public const string CENTRALTOECOMUPDATEGOLDMAKINGCHARGES = "central-to-ecom:update-gold-making-charges";
        #endregion

        #region RejectReasonMaster

        public const string CENTRALTOPOSINSERTREJECTREASONMASTER = "central-to-pos:insert-reject-reason-master";
        public const string CENTRALTOECOMINSERTREJECTREASONMASTER = "central-to-ecom:insert-reject-reason-master";
        public const string CENTRALTOPOSUPDATEREJECTREASONMASTER = "central-to-pos:update-reject-reason-master";
        public const string CENTRALTOECOMUPDATEREJECTREASONMASTER = "central-to-ecom:update-reject-reason-master";
        public const string CENTRALTOPOSDELETEREJECTREASONMASTER = "central-to-pos:delete-reject-reason-master";
        public const string CENTRALTOECOMDELETEREJECTREASONMASTER = "central-to-ecom:delete-reject-reason-master";
        #endregion

        #region GoldRateMaster
        public const string CENTRALTOPOSINSERTKARAT = "central-to-pos:insert-karat";
        public const string CENTRALTOECOMINSERTKARAT = "central-to-ecom:insert-karat";
        public const string CENTRALTOPOSUPDATEKARAT = "central-to-pos:update-karat";
        public const string CENTRALTOECOMUPDATEKARAT = "central-to-ecom:update-karat";
        public const string CENTRALTOPOSDELETEKARAT = "central-to-pos:delete-karat";
        public const string CENTRALTOECOMDELETEKARAT = "central-to-ecom:delete-karat";

        public const string CENTRALTOPOSINSERTMETAL = "central-to-pos:insert-metal";
        public const string CENTRALTOECOMINSERTMETAL = "central-to-ecom:insert-metal";
        public const string CENTRALTOPOSUPDATEMETAL = "central-to-pos:update-metal";
        public const string CENTRALTOECOMUPDATEMETAL = "central-to-ecom:update-metal";
        public const string CENTRALTOPOSDELETEMETAL = "central-to-pos:delete-metal";
        public const string CENTRALTOECOMDELETEMETAL = "central-to-ecom:delete-metal";

        public const string CENTRALTOPOSINSERTGOLDRATEMASTER = "central-to-pos:insert-gold-rate-master";
        public const string CENTRALTOECOMINSERTGOLDRATEMASTER = "central-to-ecom:insert-gold-rate-master";
        public const string CENTRALTOPOSUPDATEGOLDRATEMASTER = "central-to-pos:update-gold-rate-master";
        public const string CENTRALTOECOMUPDATEGOLDRATEMASTER = "central-to-ecom:update-gold-rate-master";
        public const string CENTRALTOPOSDELETEGOLDRATEMASTER = "central-to-pos:delete-gold-rate-master";
        public const string CENTRALTOECOMDELETGOLDRATEMASTER = "central-to-ecom:delete-gold-rate-master";
        #endregion

        #region DiamondMakingCharges

        public const string CENTRALTOPOSINSERTDIAMONDMAKINGCHARGES = "central-to-pos:insert-diamond-making-charges";
        public const string CENTRALTOECOMINSERTDIAMONDMAKINGCHARGES = "central-to-ecom:insert-diamond-making-charges";
        public const string CENTRALTOPOSUPDATEDIAMONDMAKINGCHARGES = "central-to-pos:update-diamond-making-charges";
        public const string CENTRALTOECOMUPDATEDIAMONDMAKINGCHARGES = "central-to-ecom:update-diamond-making-charges";
        public const string CENTRALTOPOSDELETEDIAMONDMAKINGCHARGES = "central-to-pos:delete-diamond-making-charges";
        public const string CENTRALTOECOMDELETEDIAMONDMAKINGCHARGES = "central-to-ecom:delete-diamond-making-charges";

        #endregion

        #region ReturnDeduction

        public const string CENTRALTOPOSINSERTRETURNDEDUCTION = "central-to-pos:insert-return-deduction";
        public const string CENTRALTOECOMINSERTRETURNDEDUCTION = "central-to-ecom:insert-return-deduction";
        public const string CENTRALTOPOSUPDATERETURNDEDUCTION = "central-to-pos:update-return-deduction";
        public const string CENTRALTOECOMUPDATERETURNDEDUCTION = "central-to-ecom:update-return-deduction";
        public const string CENTRALTOPOSDELETERETURNDEDUCTION = "central-to-pos:delete-return-deduction";
        public const string CENTRALTOECOMDELETERETURNDEDUCTION = "central-to-ecom:delete-return-deduction";

        #endregion

        #region Enquiry

        public const string ECOMTOCENTRALINSERTENQUIRY = "ecom-to-central:insert-enquiry";
        public const string ECOMTOCENTRALUPDATEENQUIRY = "ecom-to-central:update-enquiry";
        public const string ECOMTOCENTRALDELETEENQUIRY = "ecom-to-central:delete-enquiry";

        public const string CENTRALTOPOSINSERTENQUIRY = "central-to-pos:insert-enquiry";
        public const string CENTRALTOECOMUPDATEENQUIRY = "central-to-ecom:update-enquiry"; //To update CentralId in Ecom
        public const string CENTRALTOPOSUPDATEENQUIRY = "central-to-pos:update-enquiry";
        public const string CENTRALTOPOSDELETEENQUIRY = "central-to-pos:delete-enquiry";

        #endregion

        #region Discount Voucher

        public const string CENTRALTOPOSINSERTDISCOUNTVOUCHER = "central-to-pos:insert-discount-voucher";
        public const string CENTRALTOECOMINSERTDISCOUNTVOUCHER = "central-to-ecom:insert-discount-voucher";

        public const string CENTRALTOPOSUPDATEDISCOUNTVOUCHERCODE = "central-to-pos:update-discount-vouchercode";
        public const string CENTRALTOECOMUPDATEDISCOUNTVOUCHERCODE = "central-to-ecom:update-discount-vouchercode";

        #endregion

        #region Old-Gold-Purchase-Rate

        public const string CENTRALTOPOSINSERTOLDGOLDPURCHASERATE = "central-to-pos:insert-old-gold-purchase-rate";
        public const string CENTRALTOECOMINSERTOLDGOLDPURCHASERATE = "central-to-ecom:insert-old-gold-purchase-rate";
        public const string CENTRALTOPOSUPDATEOLDGOLDPURCHASERATE = "central-to-pos:update-old-gold-purchase-rate";
        public const string CENTRALTOECOMUPDATEOLDGOLDPURCHASERATE = "central-to-ecom:update-old-gold-purchase-rate";
        public const string CENTRALTOPOSDELETEOLDGOLDPURCHASERATE = "central-to-pos:delete-old-gold-purchase-rate";
        public const string CENTRALTOECOMDELETEOLDGOLDPURCHASERATE = "central-to-ecom:delete-old-gold-purchase-rate";

        #endregion

        #region Product

        public const string CENTRALTOPOSINSERTCATEGORY = "central-to-pos:insert-category";
        public const string CENTRALTOECOMINSERTCATEGORY = "central-to-ecom:insert-category";
        public const string CENTRALTOPOSUPDATECATEGORY = "central-to-pos:update-category";
        public const string CENTRALTOECOMUPDATECATEGORY = "central-to-ecom:update-category";
        public const string CENTRALTOPOSDELETECATEGORY = "central-to-pos:delete-category";
        public const string CENTRALTOECOMDELETECATEGORY = "central-to-ecom:delete-category";

        public const string CENTRALTOPOSINSERTCOLLECTION = "central-to-pos:insert-collection";
        public const string CENTRALTOECOMINSERTCOLLECTION = "central-to-ecom:insert-collection";
        public const string CENTRALTOPOSUPDATECOLLECTION = "central-to-pos:update-collection";
        public const string CENTRALTOECOMUPDATECOLLECTION = "central-to-ecom:update-collection";
        public const string CENTRALTOPOSDELETECOLLECTION = "central-to-pos:delete-collection";
        public const string CENTRALTOECOMDELETECOLLECTION = "central-to-ecom:delete-collection";

        public const string CENTRALTOPOSINSERTCOMPANY = "central-to-pos:insert-company";
        public const string CENTRALTOECOMINSERTCOMPANY = "central-to-ecom:insert-company";
        public const string CENTRALTOPOSUPDATECOMPANY = "central-to-pos:update-company";
        public const string CENTRALTOECOMUPDATECOMPANY = "central-to-ecom:update-company";
        public const string CENTRALTOPOSDELETECOMPANY = "central-to-pos:delete-company";
        public const string CENTRALTOECOMDELETECOMPANY = "central-to-ecom:delete-company";

        public const string CENTRALTOPOSINSERTDIAMONDSHAPE = "central-to-pos:insert-diamond-shape";
        public const string CENTRALTOECOMINSERTDIAMONDSHAPE = "central-to-ecom:insert-diamond-shape";
        public const string CENTRALTOPOSUPDATEDIAMONDSHAPE = "central-to-pos:update-diamond-shape";
        public const string CENTRALTOECOMUPDATEDIAMONDSHAPE = "central-to-ecom:update-diamond-shape";
        public const string CENTRALTOPOSDELETEDIAMONDSHAPE = "central-to-pos:delete-diamond-shape";
        public const string CENTRALTOECOMDELETEDIAMONDSHAPE = "central-to-ecom:delete-diamond-shape";

        public const string CENTRALTOPOSINSERTMETALCOLOR = "central-to-pos:insert-metal-color";
        public const string CENTRALTOECOMINSERTMETALCOLOR = "central-to-ecom:insert-metal-color";
        public const string CENTRALTOPOSUPDATEMETALCOLOR = "central-to-pos:update-metal-color";
        public const string CENTRALTOECOMUPDATEMETALCOLOR = "central-to-ecom:update-metal-color";
        public const string CENTRALTOPOSDELETEMETALCOLOR = "central-to-pos:delete-metal-color";
        public const string CENTRALTOECOMDELETEMETALCOLOR = "central-to-ecom:delete-metal-color";

        public const string CENTRALTOPOSINSERTPRODUCTHEADER = "central-to-pos:insert-product-header";
        public const string CENTRALTOECOMINSERTPRODUCTHEADER = "central-to-ecom:insert-product-header";

        public const string CENTRALTOPOSINSERTPRODUCTDETAIL = "central-to-pos:insert-product-detail";
        public const string CENTRALTOECOMINSERTPRODUCTDETAIL = "central-to-ecom:insert-product-detail";

        public const string POSTOCENTRALUPDATEPRODUCTHEADER = "pos-to-central:update-product-header";
        public const string CENTRALTOECOMUPDATEPRODUCTHEADER = "central-to-ecom:update-product-header";

        #endregion

        #region MaterialType

        public const string CENTRALTOPOSINSERTMATERIALTYPE = "central-to-pos:insert-material-type";
        public const string CENTRALTOECOMINSERTMATERIALTYPE = "central-to-ecom:insert-material-type";
        public const string CENTRALTOPOSUPDATEMATERIALTYPE = "central-to-pos:update-material-type";
        public const string CENTRALTOECOMUPDATEMATERIALTYPE = "central-to-ecom:update-material-type";
        public const string CENTRALTOPOSDELETEMATERIALTYPE = "central-to-pos:delete-material-type";
        public const string CENTRALTOECOMDELETEMATERIALTYPE = "central-to-ecom:delete-material-type";

        #endregion

        #region PaymentType

        public const string CENTRALTOPOSINSERTPAYMENTTYPE = "central-to-pos:insert-payment-type";
        public const string CENTRALTOECOMINSERTPAYMENTTYPE = "central-to-ecom:insert-payment-type";
        public const string CENTRALTOPOSUPDATEPAYMENTTYPE = "central-to-pos:update-payment-type";
        public const string CENTRALTOECOMUPDATEPAYMENTTYPE = "central-to-ecom:update-payment-type";
        public const string CENTRALTOPOSDELETEPAYMENTTYPE = "central-to-pos:delete-payment-type";
        public const string CENTRALTOECOMDELETEPAYMENTTYPE = "central-to-ecom:delete-payment-type";

        #endregion

        #region Reward

        public const string CENTRALTOECOMINSERTEARNINGSETTING = "central-to-ecom:insert-earning-setting";
        public const string CENTRALTOECOMINSERTREDEMPTIONSETTING = "central-to-ecom:insert-redemption-setting";

        public const string CENTRALTOPOSINSERTEARNINGSETTING = "central-to-pos:insert-earning-setting";
        public const string CENTRALTOPOSINSERTREDEMPTIONSETTING = "central-to-pos:insert-redemption-setting";

        #endregion

        #region Customer

        public const string CENTRALTOPOSINSERTCUSTOMER = "central-to-pos:insert-customer";
        public const string CENTRALTOPOSINSERTCUSTOMERUPDATEID = "central-to-pos:insert-customer-update-id";
        public const string CENTRALTOECOMINSERTCUSTOMERUPDATEID = "central-to-ecom:insert-customer-update-id";
        public const string CENTRALTOECOMINSERTCUSTOMER = "central-to-ecom:insert-customer";
        public const string CENTRALTOPOSUPDATECUSTOMER = "central-to-pos:update-customer";
        public const string CENTRALTOECOMUPDATECUSTOMER = "central-to-ecom:update-customer";
        public const string CENTRALTOPOSDELETECUSTOMER = "central-to-pos:delete-customer";
        public const string CENTRALTOECOMDELETECUSTOMER = "central-to-ecom:delete-customer";

        public const string CENTRALTOPOSINSERTWALKINSOURCE = "central-to-pos:insert-walkinsource";
        public const string CENTRALTOECOMINSERTWALKINSOURCE = "central-to-ecom:insert-walkinsource";
        public const string CENTRALTOPOSUPDATEWALKINSOURCE = "central-to-pos:update-walkinsource";
        public const string CENTRALTOECOMUPDATEWALKINSOURCE = "central-to-ecom:update-walkinsource";
        public const string CENTRALTOPOSDELETEWALKINSOURCE = "central-to-pos:delete-walkinsource";
        public const string CENTRALTOECOMDELETEWALKINSOURCE = "central-to-ecom:delete-walkinsource";


        #endregion

        #region RepairWork

        public const string CENTRALTOPOSINSERTREPAIRWORK = "central-to-pos:insert-repairwork";
        public const string CENTRALTOECOMINSERTREPAIRWORK = "central-to-ecom:insert-repairwork";
        public const string CENTRALTOPOSUPDATEREPAIRWORK = "central-to-pos:update-repairwork";
        public const string CENTRALTOECOMUPDATEREPAIRWORK = "central-to-ecom:update-repairwork";
        public const string CENTRALTOPOSDELETEREPAIRWORK = "central-to-pos:delete-repairwork";
        public const string CENTRALTOECOMDELETEREPAIRWORK = "central-to-ecom:delete-repairwork";


        #endregion

        #region ReturnToMayave

        public const string POSTOCENTRALINSERTRETURNTOMAYAVEPRODUCT = "pos-to-central:insert-return-to-mayave-product";
        public const string CENTRALTOECOMINSERTRETURNTOMAYAVEPRODUCT = "central-to-ecom:insert-return-to-mayave-product";

        public const string CENTRALTOPOSUPDATERETURNTOMAYAVEPRODUCT = "central-to-pos:update-return-to-mayave-product";

        public const string POSTOCENTRALUPDATERETURNTOMAYAVEPRODUCTSPRICE = "pos-to-central:update-return-to-mayave-products-price";
        public const string CENTRALTOECOMUPDATERETURNTOMAYAVEPRODUCTSPRICE = "central-to-ecom:update-return-to-mayave-products-price";

        public const string POSTOCENTRALUPDATERETURNTOMAYAVEPRODUCTSPRICEACCEPTORREJECT = "pos-to-central:update-return-to-mayave-products-price-accept-or-reject";
        public const string CENTRALTOECOMUPDATERETURNTOMAYAVEPRODUCTSPRICEACCEPTORREJECT = "central-to-ecom:update-return-to-mayave-products-price-accept-or-reject";

        //public const string POSTOCENTRALDELETERETURNTOMAYAVEPRODUCT = "pos-to-central:delete-return-to-mayave-product";
        //public const string CENTRALTOECOMDELETERETURNTOMAYAVEPRODUCT = "central-to-ecom:delete-return-to-mayave-product";

        public const string POSTOCENTRALUPDATEPRODUCTHEADERQTYANDSTORECODEFORRETURNTOMAYAVE = "pos-to-central:update-product-header-qty-and-store-code-for-return-to-mayave-product";
        public const string CENTRALTOECOMUPDATEPRODUCTHEADERQTYANDSTORECODEFORRETURNTOMAYAVE = "central-to-ecom:update-product-header-qty-and-store-code-for-return-to-mayave-product";


        #endregion

        #region StyleMaster

        public const string CENTRALTOECOMINSERTSTYLEMASTER = "central-to-ecom:insert-style-master";
        public const string CENTRALTOPOSINSERTSTYLEMASTER = "central-to-pos:insert-style-master";

        #endregion

        #region CustomerReferral
        public const string POSTOCENTRALDELETECUSTOMERREFERRAL = "pos-to-central:delete-customer-referral";
        public const string POSTOCENTRALINSERTCUSTOMERREFERRAL = "pos-to-central:insert-customer-referral";
        public const string POSTOCENTRALUPDATECUSTOMERREFERRAL = "pos-to-central:update-customer-referral";
        public const string CENTRALTOPOSINSERTOCUSTOMERREFERRALUPDATEID = "central-to-pos:updated-centaralId-customer-referral";
        #endregion

        #region SalesEntry
        public const String POSTOCENTRALSALESENTRYSQS = "pos-to-central:insert-sales-entry";
        public const String CENTRALTOPOSUPDATESALESENTRYCENTRALID = "centarl-to-pos:update-sales-entry-centralid";
        public const string CENTRALTOECOMINSERTSALESENTRY = "central-to-ecom:insert-sales-entry";
        #endregion

        #region PurchaseOrderItemBag
        public const String POSTOCENTRALUPDATEPURCHASEORDERSTATUS = "pos-to-central:update-purchase-order-status";
        public const String POSTOCENTRALINSERTPURCHASEORDERITEMBAG = "pos-to-central:insert-purchase-order-item-bag";
        public const String CENTRALTOPOSUPDATECENTRALIDPURCHASEITEMBAG = "central-to-pos:update-centralid-purchase-order-item-bag";
        public const String POSTOCENTRALUPDATEPURCHASEORDERSRATE = "pos-to-central:update-purchase-order-rate";
        public const String CENTRALTOECOMINSERTPURCHASEORDERBAG = "central-to-ecom:insert-purchase-order-item-bag";
        #endregion

        #region EsttimatePayment
        public const String POSTOCENTRALINSERTESTIMATEPAYMENT = "pos-to-central:insert-estimate-payments";
        public const String CENTRALTOPOSUPDATEESTIMATEPAYMENTCENTRALID = "central-to-pos:update-estimate-payment-centralids";
        public const String CENTRALTOECOMUPDATEESTIMATEPAYMENTCENTRALID = "central-to-ecom:update-estimate-payment-centralid";
        #endregion

        #region Return Sales Order
        public const String POSTOCENTRALINSERTSALESRETURNORDER = "pos-to-central-insert-return-sales-order";
        public const String CENTRALTOECOMINSERTRETURNSALESORDER = "central-to-ecom:insert-return-sales-order";
        public const String CENTRALTOPOSUPDATESALESRETURNORDERCENTRALID = "central-to-pos:update-sales-return-order-central-id";
        #endregion
        #region
        public const String CENTRALTOPOSINSERTCOMPLEXITY = "central-to-pos:insert-complexity";
        public const String CENTRALTOPOSUPDATECOMPLEXITY = "central-to-pos:update-complexity";
        public const String CENTRALTOECOMINSERTCOMPLEXITY = "central-to-ecom:insert-complexity";
        public const String CENTRALTOECOMUPDATECOMPLEXITY = "central-to-ecom:update-complexity";


        #endregion

        #region RepairWork
        public const String POSTOCENTRALINSERTREPAIRWORKITEMS = "pos-to-central-insert-repair-work-items";
        public const String CENTRALTOPOSUPDATECENTRALID = "central-to-pos-update-central-id";
        public const String CENTRALTOECOMINSERTREPAIRWORKITEMS = "central-to-ecom-insert-repair-work-items";
        #endregion
    }
}
