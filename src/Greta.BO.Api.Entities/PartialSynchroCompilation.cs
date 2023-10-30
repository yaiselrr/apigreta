using System;
/*using Newtonsoft.Json;*/

namespace Greta.BO.Api.Entities
{
    public class PartialSynchroCompilation
    {
        public long SynchroId { get; set; }
        public long Tag { get; set; }
        public DateTime CreationDate { get; set; }

        public DownSynchroCompilation Added { get; set; }
        public DownSynchroCompilation Updated { get; set; }
        public DownSynchroCompilation Deleted { get; set; }

        /*public static PartialSynchroCompilation Convert(Synchro synchro)
        {
            PartialSynchroCompilation partial = new PartialSynchroCompilation();

            partial.SynchroId = synchro.Id;
            partial.Tag = synchro.Tag;

            partial.Added = Build(synchro.SynchroDetails
                        .Where(x => x.Type == SynchroType.CREATE)
                        .ToList());
            partial.Updated = Build(synchro.SynchroDetails
                .Where(x => x.Type == SynchroType.UPDATE)
                .ToList());
            partial.Deleted = Build(synchro.SynchroDetails
                .Where(x => x.Type == SynchroType.DELETE)
                .ToList());

            return partial;
        }

        private static DownSynchroCompilation Build(List<SynchroDetail> items)
        {
            DownSynchroCompilation data = new();


            data.Employees = items.Where(x => x.Entity == nameof(BOUser))
                .Select(x => JsonConvert.DeserializeObject<LiteEmployee>(x.Data)) //LiteEmployee.Convert(JsonConvert.DeserializeObject<BOUser>(x.Data)))
                //.Select(x => x != null)
                .ToList();
                
                // (await _repository.GetEntity<BOUser>()
                // //.Where(x => x.POSProfileId != null)
                // .ToListAsync())
                // .Select(LiteEmployee.Convert).ToList();
            
            // List<LiteStore> stores = //LiteStore.Convert(
            // items.Where(x => x.Entity == nameof(Store))
            //     .Select(x => LiteStore.Convert(JsonConvert.DeserializeObject<Store>(x.Data)))
            //     .Where(x => x != null)
            //     .ToList();
            //
            //
            // var products = await _repository.GetEntity<Product>()
            //     // .Include(x => x.MixAndMatchs)
            //     .LeftJoin(
            //         _repository.GetEntity<StoreProduct>().Where(x => x.StoreId == storeId).Include(x => x.Taxs),
            //         product => product.Id,
            //         storeP => storeP.ProductId,
            //         (product, storeP) => new
            //         {
            //             Product = product,
            //             StoreP = storeP
            //         }
            //     ) 
            //     .Where(x =>
            //         x.StoreP != null)
            //     .ToListAsync();
            
            // var productIds = products.Select(p => p.Product.Id).ToList();
            //
            // //All This element need to be synchro not need be filter by store
            //
            // data.Profiles = (await _repository.GetEntity<Profiles>()
            //         .Where(x => x.ApplicationId == 2)
            //         .Include(x => x.Permissions)
            //         .ToListAsync())
            //     .Select(LiteProfile.Convert).ToList();
            //
            // data.Employees = (await _repository.GetEntity<BOUser>()
            //     //.Where(x => x.POSProfileId != null)
            //     .ToListAsync())
            //     .Select(LiteEmployee.Convert).ToList();
            //
            // data.TenderTypes = (await _repository.GetEntity<TenderType>().ToListAsync())
            //     .Select(LiteTenderType.Convert).ToList();
            //
            // data.Families = (await _repository.GetEntity<Family>().ToListAsync())
            //     .Select(LiteFamily.Convert).ToList();
            //
            // data.Discounts = (await _repository.GetEntity<Discount>()
            //         .Include(p => p.Products.Where( x=> productIds.Contains(x.Id)))
            //         .ToListAsync())
            //     .Select(LiteDiscount.Convert).ToList();
            //
            // // data.Discounts = (await _repository.GetEntity<Discount>()
            // //         .Include(p => p.Products)
            // //     .ToListAsync())
            // //     .Select(LiteDiscount.Convert).ToList();
            //
            // data.Departments = (await _repository.GetEntity<Department>().ToListAsync())
            //     .Select(LiteDepartment.Convert).ToList();
            //
            // data.Categories = (await _repository.GetEntity<Category>().ToListAsync())
            //     .Select(LiteCategory.Convert).ToList();
            //
            // data.ScaleCategories = (await _repository.GetEntity<ScaleCategory>().ToListAsync())
            //     .Select(LiteScaleCategory.Convert).ToList();
            //
            // data.ScaleBrands = (await _repository.GetEntity<ScaleBrand>().ToListAsync())
            //     .Select(LiteScaleBrand.Convert).ToList();
            //
            // data.ExternalScales =
            //     (await _repository.GetEntity<ExternalScale>().Where(x => x.StoreId == storeId).ToListAsync())
            //     .Select(LiteExternalScale.Convert).ToList();
            //
            // data.ScaleHomeFavs =
            //     (await _repository.GetEntity<ScaleHomeFav>().Where(x => x.StoreId == storeId).ToListAsync())
            //     .Select(LiteScaleHomeFav.Convert).ToList();
            //
            // data.Taxes = (await _repository.GetEntity<Tax>().Where(x => x.Stores.Any( s => s.Id == storeId)).ToListAsync())
            //     .Select(LiteTax.Convert).ToList();
            //
            // data.ScaleLabelTypes = (await _repository.GetEntity<ScaleLabelType>().ToListAsync())
            //     .Select(LiteScaleLabelType.Convert).ToList();
            //
            // data.ScaleLabelDefinitions = (await _repository.GetEntity<ScaleLabelDefinition>().ToListAsync())
            //     .Select(LiteScaleLabelDefinition.Convert).ToList();
            //
            // data.Fees = (await _repository.GetEntity<Fee>()
            //         .Include(p => p.Products)
            //         .Include(p => p.Categories)
            //         .Include(p => p.Families)
            //         .ToListAsync())
            //     .Select(LiteFee.Convert).ToList();
            //
            // data.MixAndMatchs = (await _repository.GetEntity<MixAndMatch>()
            //         .Include(p => p.Products)
            //         .Include(p => p.Families)
            //         .ToListAsync())
            //     .Select(LiteMixAndMatch.Convert).ToList();
            //
            // data.Products = products.Where(x => x.Product.ProductType == ProductType.SPT)
            //     .Select(x => LiteProduct.Convert(x.Product, x.StoreP)).ToList();
            // data.ScaleProducts = products.Where(x => x.Product.ProductType == ProductType.SLP)
            //     .Select(x => LiteScaleProduct.Convert1((ScaleProduct)x.Product, x.StoreP)).ToList();
            // data.WProducts = products.Where(x => x.Product.ProductType == ProductType.CWP)
            //     .Select(x => LiteWProduct.Convert2((WProduct)x.Product, x.StoreP)).ToList();
            // data.Customers = (await _repository.GetEntity<Customer>().ToListAsync())
            //     .Select(LiteCustomer.Convert).ToList();

            return data;
        }*/
    }
}