using System.Collections.Generic;
using Greta.BO.Api.Entities.Lite;

namespace Greta.BO.Api.Entities
{
    /// <summary>
    ///     Class used for storage and serializate all databae for full backup syncronization
    /// </summary>
    public class DownSynchroCompilation
    {
        //es necesario el tag o version de sinconizacion actual de la base de datos
        //ademas la informacion de configuracion de el store, he incluso podria ser el de los devices de la tienda

        public LiteStore StoreConfiguration { get; set; }
        public List<LiteScaleReasonCodes> ReasonCodes { get; set; }
        public LiteLoyaltyDiscount LoyaltyDiscount { get; set; }
        public List<LiteProfile> Profiles { get; set; }
        public List<LiteEmployee> Employees { get; set; }
        public List<LiteTenderType> TenderTypes { get; set; }
        public List<LiteDepartment> Departments { get; set; }
        public List<LiteCategory> Categories { get; set; }
        public List<LiteScaleCategory> ScaleCategories { get; set; }
        public List<LiteFamily> Families { get; set; }
        public List<LiteDiscount> Discounts { get; set; }
        public List<LiteExternalScale> ExternalScales { get; set; }
        public List<LiteTax> Taxes { get; set; }
        public List<LiteFee> Fees { get; set; }
        public List<LiteMixAndMatch> MixAndMatchs { get; set; }
        public List<LiteScaleLabelType> ScaleLabelTypes { get; set; }
        public List<LiteScaleLabelDefinition> ScaleLabelDefinitions { get; set; }
        public List<LiteScaleHomeFav> ScaleHomeFavs { get; set; }
        public List<LiteProduct> Products { get; set; }
        public List<LiteScaleProduct> ScaleProducts { get; set; }
        public List<LiteKitProduct> KitProducts { get; set; }
        public List<LiteAdBatch> AdBatches { get; set; }
        public List<LitePriceBatch> PriceBatches { get; set; }
        public List<LitePriceBatchDetail> PriceBatchDetails { get; set; }
        public List<LiteVendor> Vendors { get; set; }
        public List<LiteCustomer> Customers { get; set; }
    }
}