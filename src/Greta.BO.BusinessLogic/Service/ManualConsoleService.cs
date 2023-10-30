using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BooleanConverter = Greta.BO.BusinessLogic.TypeConverters.BooleanConverter;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IManualConsoleService : IBaseService
    {
        Task OutsideCustomerImport(string csvFilePath);
        Task OutsideGiftCardImport(string csvFilePath);
        Task OutsideVendorProductsImport(string csvFilePath);
        Task UpdateSalesProductYavapee();
        Task ImportSProducts(string csvFilePath);
        Task ResyncScaleCategory();
        Task ResampleProductsFromOneStoreToOther(long storeFrom, long storeTo);
        Task ResyncScaleProduct();
        Task LoadQtyProducts(string csvFilePath);
        Task ImportStandardProducts(string csvFilePath);
        Task ResyncCategory(string csvFilePath);
        Task setupVendorONMorelos(string csvFilePath);
    }

    public class ManualConsoleService : IManualConsoleService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IGiftCardRepository _giftCardRepository;
        private readonly IVendorProductRepository _vendorProductRepository;
        private readonly IVendorOrderRepository _vendorOrderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IStoreProductService _storeProductService;
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly IScaleLabelDefinitionService _scaleLabelDefinitionService;
        private readonly IScaleCategoryService _scaleCategoryService;
        private readonly IAuthenticateUser<string> _user;
        private readonly ICategoryService _categoryService;
        private readonly ISynchroService _synchroService;
        private readonly ILogger<ManualConsoleService> _logger;

        public ManualConsoleService(
            // ICustomerRepository customerRepository,
            IGiftCardRepository giftCardRepository,
            // IVendorProductRepository vendorProductRepository,
            // IVendorOrderRepository vendorOrderRepository,
            IProductRepository productRepository,
            IProductService productService,
            IStoreProductService storeProductService,
            // IStoreProductRepository storeProductRepository,
            // IScaleLabelDefinitionService scaleLabelDefinitionService,
            // IScaleCategoryService scaleCategoryService,
            IAuthenticateUser<string> user,
            ICategoryService categoryService,
            // ISynchroService synchroService,
            ILogger<ManualConsoleService> logger)
        {
            // _customerRepository = customerRepository;
            _giftCardRepository = giftCardRepository;
            // _vendorProductRepository = vendorProductRepository;
            // _vendorOrderRepository = vendorOrderRepository;
            _productRepository = productRepository;
            _productService = productService;
            _storeProductService = storeProductService;
            // _storeProductRepository = storeProductRepository;
            // _scaleLabelDefinitionService = scaleLabelDefinitionService;
            // _scaleCategoryService = scaleCategoryService;
            _user = user;
            _categoryService = categoryService;
            // _synchroService = synchroService;
            _logger = logger;
        }

        public async Task setupVendorONMorelos(string csvFilePath)
        {
            _user.IsAuthenticated = true;
            _user.UserId = "c59cb4bd-5211-4a3f-8819-73cd78cfc8ce"; //morelos

            using var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," });
            await csv.ReadAsync();
            csv.ReadHeader();
            var csvHeaders = csv.HeaderRecord.ToList();
            var vendors = new Dictionary<string, long>();
            while (await csv.ReadAsync())
            {
                var upc = csv.GetField<string>(csvHeaders[0]);
                var itemNUmber = csv.GetField<string>(csvHeaders[2]);
                var vendor = csv.GetField<string>(csvHeaders[3]);


                if (!vendors.TryGetValue(vendor, out long vId))
                {
                    var ventity = await _productRepository.GetEntity<Vendor>()
                        .Where(x => x.Name == vendor)
                        .FirstOrDefaultAsync();
                    if (ventity != null)
                    {
                        vId = ventity.Id;
                    }
                    else
                    {
                        var ventity1 = await _productRepository.GetEntity<Vendor>()
                            .AddAsync(new Vendor()
                            {
                                Name = vendor
                            });
                        await _productRepository.GetContext<SqlServerContext>().SaveChangesAsync();
                        vId = ventity1.Entity.Id;
                        _logger.LogInformation("Vendor {Vendor} not found", vendor);
                    }
                }
                
                var product = await _productRepository.GetEntity<Product>()
                    .Where(x => x.UPC == upc)
                    .FirstOrDefaultAsync();

                if (product != null && vId > 0)
                {
                    await _productRepository.GetEntity<VendorProduct>()
                        .AddAsync(new VendorProduct
                        {
                            State = true,
                            UserCreatorId = _user.UserId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            ProductCode = itemNUmber,
                            PackSize = "1",
                            OrderByCase = VendorProductType.CASE,
                            VendorId = vId,
                            ProductId = product.Id
                        });
                    await _productRepository.GetContext<SqlServerContext>().SaveChangesAsync();
                    _logger.LogInformation("Complete {UPC}", upc);
                }
                else
                {
                    _logger.LogInformation("Vendor {Vendor} not found or product with upc {UPC} not found", vendor, upc);
                }

            }
            
        }

        public async Task manualsetupsproducts()
        {
            var ids = new List<string>()
            {
                "781",
                "793",
                "776",
                "773",
                "774",
                "777",
                "778",
                "779",
                "780",
                "782",
                "784",
                "785",
                "786",
                "787",
                "788",
                "789",
                "790",
                "791",
                "792",
                "795",
                "775",
                "796",
                "798",
                "805",
                "808",
                "814",
                "816",
                "797",
                "801",
                "803",
                "804",
                "812",
                "809",
                "800",
                "807",
                "818",
                "815",
                "817",
                "802",
                "783",
                "799",
                "811"
            };
            
            
        }

        public async Task ResyncCategory(string csvFilePath)
        {
            
            _user.IsAuthenticated = true;
            _user.UserId = "ed5f6e15-2cc1-45c8-b47e-2ae69b2b59ae"; //morelos

            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                var csvHeaders = csv.HeaderRecord.ToList();
                var departments = new Dictionary<int, long>();
                while (await csv.ReadAsync())
                {
                    var name = csv.GetField<string>("Name");
                    var depId = csv.GetField<int>("Dept 1d");
                    var catid = csv.GetField<int>("Cat ID");
                    
                    if (!departments.ContainsKey(depId))
                    {
                        var d = await _productRepository.GetEntity<Department>()
                            .Where(x => x.DepartmentId == depId)
                            .Select(x => new Department() { Id = x.Id, DepartmentId = x.DepartmentId })
                            .FirstOrDefaultAsync();
                        if (d != null)
                        {
                            departments.Add(depId, d.Id);
                        }
                        else
                        {
                            _logger.LogError($"Category {name}, fail by departmentId {depId} not found");
                            continue;
                        }
                    }

                    var category = new Category
                    {
                        CategoryId = catid,
                        Name = name,
                        Description = name,
                        DepartmentId = departments[depId]
                    };

                    await _categoryService.Post(category);


                }
            }
        }
        
        public async Task ResyncScaleCategory()
        {
            var cales = await _scaleCategoryService.Get();
            foreach (var scaleCategory in cales)
            {
                await _scaleCategoryService.Put(scaleCategory.Id, scaleCategory);
            }

            _logger.LogInformation("end");
        }
        
        public async Task LoadQtyProducts(string csvFilePath)
        {
            _user.IsAuthenticated = true;
            _user.UserId = "b50cd8f4-e24f-4c07-b3b4-3d99465dbdbe"; //richardson

            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                var csvHeaders = csv.HeaderRecord.ToList();

                while (await csv.ReadAsync())
                {
                    try
                    {
                        var upc = csv.GetField<string>(csvHeaders[1]);


                        var qty = csv.GetField<decimal>(csvHeaders[6]);
                        if (qty != 0)
                        {
                            _logger.LogInformation($"Processing {upc} updating qty to {qty}");

                            var p = await _storeProductRepository.GetEntity<StoreProduct>()
                                .Where(x => x.Product.UPC == upc).FirstOrDefaultAsync();
                            p.QtyHand += qty;
                            await _storeProductRepository.UpdateAsync(p.Id, p);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            _logger.LogInformation($"The end");
        }

        public async Task ResampleProductsFromOneStoreToOther(long storeFrom, long storeTo)
        {
            var products = await _storeProductRepository.GetEntity<StoreProduct>()
                //.Include(x => x.Product)
                .Where((x => x.StoreId == storeFrom)).ToListAsync();
            foreach (var p in products)
            {
                var exist = await _storeProductService.GetAllByProductAndStore(p.ProductId, storeTo);
                if (exist == null)
                {
                    var product = await _storeProductRepository.GetEntity<Product>()
                        //.Include(x => x.Product)
                        .Where((x => x.Id == p.Id)).FirstOrDefaultAsync();
                    await _storeProductService.PostImport(new StoreProduct()
                    {
                        StoreId = storeTo,
                        ProductId = p.ProductId,
                        Price = p.Price,
                        Cost = p.Cost,
                        GrossProfit = p.GrossProfit,
                        UserCreatorId = p.UserCreatorId,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    }, product);
                }
            }
        }

        public async Task ResyncScaleProduct()
        {
            var products = await _productRepository.GetEntity<ScaleProduct>()
                .Include(x => x.StoreProducts)
                .Include(x => x.ScaleLabelDefinitions)
                .ToListAsync();

            foreach (var sp in products)
            foreach (var s in sp.StoreProducts)
            {
                //(List<long> stores, TDat data, SynchroType type, Func<TDat, string> converter = null);
                await _synchroService.AddSynchroToStore(
                    s.StoreId,
                    LiteScaleProduct.Convert1(sp, s),
                    SynchroType.UPDATE
                );
                if (sp.ScaleLabelDefinitions != null)
                    foreach (var sd in sp.ScaleLabelDefinitions)
                    {
                        await _synchroService.AddSynchroToStore(
                            s.StoreId,
                            LiteScaleLabelDefinition.Convert(sd),
                            SynchroType.UPDATE
                        );
                    }
            }

            _logger.LogInformation("end");
        }

        public async Task ImportSProducts1(string csvFilePath)
        {
            _user.IsAuthenticated = true;
            _user.UserId = "bc499345-b7c0-46d7-a70a-00e04536be29"; //richardson

            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                var csvHeaders = csv.HeaderRecord.ToList();

                while (await csv.ReadAsync())
                {
                    var upc = csv.GetField<int>(csvHeaders[0]);
                    _logger.LogInformation($"Processing {upc} ....");

                    var PLUNumber = (int)GetCsvValue("PLUNumber", csv, typeof(int), 0);


                    var product = await _productService.GetProductByUPC(upc.ToString());
                    if (product != null)
                    {
                        if (product.ProductType == ProductType.SLP)
                        {
                            _logger.LogError($"Product {upc} is scale type");
                        }
                        else
                        {
                            if (await _productRepository.DeleteAsync(product.Id))
                                _logger.LogInformation($"ok");
                            else
                            {
                                _logger.LogError($"fail");
                            }
                            //await _productRepository.GetContext<SqlServerContext>().SaveChangesAsync();
                        }
                        // if (product.ProductType == ProductType.SPT)
                        // {
                        //     await _productService.Delete(product.Id);
                        // }
                        // else
                        // {
                        //     _logger.LogError($"Product {upc} is scale type");
                        // }
                    }
                    else
                    {
                        _logger.LogError($"notfound");
                    }
                }

                _logger.LogInformation($"The end");
            }
        }

        public async Task ImportSProducts(string csvFilePath)
        {
            _user.IsAuthenticated = true;
            //_user.UserId = "bc499345-b7c0-46d7-a70a-00e04536be29";//richardson
            _user.UserId = "449b813d-bf96-45c2-996e-f761c55c75d6"; //journey on stage

            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                var csvHeaders = csv.HeaderRecord.ToList();
                var categorys = new Dictionary<int, long>();
                var scategorys = new Dictionary<int, long>();
                var departments = new Dictionary<int, long>();
                while (await csv.ReadAsync())
                {
                    var upc = csv.GetField<int>(csvHeaders[0]);

                    var catId = csv.GetField<int>("catID");
                    //var scatId = (int)GetCsvValue("ScaleCat_ID", csv, typeof(int), 120);//csv.GetField<int>("Cat_ID");
                    var scatId = (int)csv.GetField<int>("﻿ScaleCat_ID");
                    var departmentId = csv.GetField<int>("DepartmentID");

                    if (!categorys.ContainsKey(catId))
                    {
                        var d = await _productRepository.GetEntity<Category>().Where(x => x.CategoryId == catId)
                            .Select(x => new Category() { Id = x.Id, CategoryId = x.CategoryId }).FirstOrDefaultAsync();
                        if (d != null)
                        {
                            categorys.Add(catId, d.Id);
                        }
                        else
                        {
                            _logger.LogError($"Product with upc {upc}, fail by categoryId {catId} not found");
                            continue;
                        }
                    }

                    if (!scategorys.ContainsKey(scatId))
                    {
                        var d = await _productRepository.GetEntity<ScaleCategory>().Where(x => x.CategoryId == scatId)
                            .Select(x => new ScaleCategory() { Id = x.Id, CategoryId = x.CategoryId })
                            .FirstOrDefaultAsync();
                        if (d != null)
                        {
                            scategorys.Add(scatId, d.Id);
                        }
                        else
                        {
                            _logger.LogError($"Product with upc {upc}, fail by scategoryId {scatId} not found");
                            continue;
                        }
                    }

                    if (!departments.ContainsKey(departmentId))
                    {
                        var d = await _productRepository.GetEntity<Department>()
                            .Where(x => x.DepartmentId == departmentId)
                            .Select(x => new Department() { Id = x.Id, DepartmentId = x.DepartmentId })
                            .FirstOrDefaultAsync();
                        if (d != null)
                        {
                            departments.Add(departmentId, d.Id);
                        }
                        else
                        {
                            _logger.LogError($"Product with upc {upc}, fail by departmentId {departmentId} not found");
                            continue;
                        }
                    }

                    //strings
                    var description = csv.GetField<string>("Name");
                    var SCALE_DESCRIPTION_1 = csv.GetField<string>("SCALE_DESCRIPTION_1");
                    var SCALE_DESCRIPTION_2 = csv.GetField<string>("SCALE_DESCRIPTION_2");
                    var INGRED_STATEMENT = csv.GetField<string>("INGRED_STATEMENT");

                    //Boolleans
                    //var posvisible = GetCsvValue<bool, BooleanConverter>("POS Visible", csv, false);
                    var foodstamp = GetCsvValue<bool, BooleanConverter>("FoodStampEligible", csv, false);
                    var allozero = GetCsvValue<bool, BooleanConverter>("AllowZeroStock", csv, false);
                    var promptforprice = GetCsvValue<bool, BooleanConverter>("PromptForPrice", csv, false);

                    //numbers
                    var price = (decimal)GetCsvValue("PriceA", csv, typeof(decimal), 0M);
                    var cost = (decimal)GetCsvValue("Cost", csv, typeof(decimal), 0M);
                    var tare = (decimal)GetCsvValue("Tare", csv, typeof(decimal), 0M);
                    var Points = (int)GetCsvValue("Points", csv, typeof(int), 0);
                    var BYCOUNT = (int)GetCsvValue("BYCOUNT", csv, typeof(int), 0);
                    var TARE_1_S = (decimal)GetCsvValue("TARE_1_S", csv, typeof(decimal), (decimal)0);
                    var SHELF_LIFE = (int)GetCsvValue("SHELF_LIFE", csv, typeof(int), 0);
                    var PRODUCT_LIFE = (int)GetCsvValue("PRODUCT_LIFE", csv, typeof(int), 0);
                    var PLUNumber = (int)GetCsvValue("PLUNumber", csv, typeof(int), 0);
                    if (PLUNumber == 0)
                    {
                        PLUNumber = upc;
                    }

                    _logger.LogInformation($"Processing {upc}-- {PLUNumber} ....");
                    //label
                    //
                    var Label_1 = (int)GetCsvValue("Label_1", csv, typeof(int), 0);

                    var stringValue = csv.GetField(typeof(string), "ITEM_TYPE");
                    var PLUType = PluType.RandomWeight;
                    switch (stringValue)
                    {
                        case "Fixed Weight":
                            PLUType = PluType.FixedWeight;
                            break;
                        case "By Count":
                            PLUType = PluType.ByCount;
                            break;
                    }

                    var product = await _productService.GetProductByUPC(upc.ToString());
                    if (product == null)
                    {
                        var wp = new ScaleProduct
                        {
                            State = true,
                            UPC = upc.ToString(),
                            Name = description,
                            CategoryId = categorys[catId],
                            DepartmentId = departments[departmentId],
                            ScaleCategoryId = scategorys[scatId],
                            ProductType = ProductType.SLP,
                            SnapEBT = foodstamp,
                            AllowZeroStock = allozero,
                            PromptPriceAtPOS = promptforprice,
                            LoyaltyPoints = Points,
                            PLUNumber = PLUNumber,
                            PLUType = PLUType,
                            Description1 = SCALE_DESCRIPTION_1,
                            Description2 = SCALE_DESCRIPTION_2,
                            ByCount = BYCOUNT,
                            ShelfLife = SHELF_LIFE,
                            ProductLife = PRODUCT_LIFE,
                            //ScaleLabelDefinitions = null,
                            Text1 = INGRED_STATEMENT,
                            Tare1 = TARE_1_S
                        };
                        var np = await _productService.CreateScaleProduct(wp);


                        if (Label_1 == 103 || Label_1 == 203)
                        {
                            long labelId = await _scaleLabelDefinitionService.GetIdByName(Label_1.ToString());
                            if (labelId > 0)
                            {
                                await _scaleLabelDefinitionService.Post(new ScaleLabelDefinition()
                                {
                                    ScaleProductId = np.Id,
                                    ScaleLabelType1Id = labelId,
                                    ScaleLabelType2Id = labelId,
                                    ScaleBrandId = 1
                                });
                            }
                        }

                        await _storeProductService.PostImport(new StoreProduct()
                        {
                            ProductId = np.Id,
                            StoreId = 3,
                            Cost = cost,
                            Price = price
                        }, np);
                        if (np != null)
                        {
                            _logger.LogInformation($"Product {upc} - {description} OK");
                            // _logger.LogInformation($"Sucess");
                        } //else{
                        //     _logger.LogError($"Fail.");
                        // }
                    }
                    else
                    {
                        //_logger.LogError($"Product {upc} found");
                        if (product is ScaleProduct scaleProduct)
                        {
                            scaleProduct.Name = description;
                            scaleProduct.CategoryId = categorys[catId];
                            scaleProduct.DepartmentId = departments[departmentId];
                            scaleProduct.ScaleCategoryId = scategorys[scatId];
                            scaleProduct.ProductType = ProductType.SLP;
                            scaleProduct.SnapEBT = foodstamp;
                            scaleProduct.AllowZeroStock = allozero;
                            scaleProduct.PromptPriceAtPOS = promptforprice;
                            scaleProduct.LoyaltyPoints = Points;
                            scaleProduct.PLUNumber = PLUNumber;
                            scaleProduct.PLUType = PLUType;
                            scaleProduct.Description1 = SCALE_DESCRIPTION_1;
                            scaleProduct.Description2 = SCALE_DESCRIPTION_2;
                            scaleProduct.ByCount = BYCOUNT;
                            scaleProduct.ShelfLife = SHELF_LIFE;
                            scaleProduct.ProductLife = PRODUCT_LIFE;
                            scaleProduct.Text1 = INGRED_STATEMENT;
                            scaleProduct.Tare1 = TARE_1_S;


                            //var np = await _productService.UpdateScaleProduct(product.Id, scaleProduct);
                            // if (np)
                            // {
                            //     _logger.LogInformation($"Sucess");
                            // }else{
                            //     _logger.LogError($"Fail.");
                            // }
                            if (Label_1 == 103 || Label_1 == 203)
                            {
                                long labelId = await _scaleLabelDefinitionService.GetIdByName(Label_1.ToString());
                                if (labelId > 0)
                                {
                                    if (scaleProduct.ScaleLabelDefinitions.Count > 0)
                                    {
                                        scaleProduct.ScaleLabelDefinitions[0].ScaleLabelType1Id = labelId;
                                        await _productService.UpdateScaleProduct(product.Id, scaleProduct);
                                    }
                                    else
                                    {
                                        await _scaleLabelDefinitionService.Post(new ScaleLabelDefinition()
                                        {
                                            ScaleProductId = scaleProduct.Id,
                                            ScaleLabelType1Id = labelId,
                                            ScaleLabelType2Id = labelId,
                                            ScaleBrandId = 1
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _logger.LogError($"Product {upc} Fail current product is not sproduct");
                        }
                    }
                }
            }
        }

        public async Task ImportStandardProducts(string csvFilePath)
        {
            _user.IsAuthenticated = true;
            //_user.UserId = "bc499345-b7c0-46d7-a70a-00e04536be29";//richardson
            // _user.UserId = "a617a7fb-971f-4cb3-8f70-ac291960253c"; //journey on stage
            // _user.UserId = "10390a4e-39db-4dc3-b0c4-0e4669c0b033"; //el paya
            _user.UserId = "ed5f6e15-2cc1-45c8-b47e-2ae69b2b59ae"; //el morelos

            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                var csvHeaders = csv.HeaderRecord.ToList();
                var categorys = new Dictionary<int, long>();
                var departments = new Dictionary<int, long>();
                while (await csv.ReadAsync())
                {
                    var upc = csv.GetField<string>("UPC");
                    var catId = csv.GetField<double>("Cat ID");
                    var departmentId = csv.GetField<double>("Dept ID");

                    if (!categorys.ContainsKey((int)catId))
                    {
                        var d = await _productRepository.GetEntity<Category>().Where(x => x.CategoryId == catId)
                            .Select(x => new Category() { Id = x.Id, CategoryId = x.CategoryId }).FirstOrDefaultAsync();
                        if (d != null)
                        {
                            categorys.Add((int)catId, d.Id);
                        }
                        else
                        {
                            _logger.LogError($"Product with upc {upc}, fail by categoryId {catId} not found");
                            continue;
                        }
                    }

                    if (!departments.ContainsKey((int)departmentId))
                    {
                        var d = await _productRepository.GetEntity<Department>()
                            .Where(x => x.DepartmentId == departmentId)
                            .Select(x => new Department() { Id = x.Id, DepartmentId = x.DepartmentId })
                            .FirstOrDefaultAsync();
                        if (d != null)
                        {
                            departments.Add((int)departmentId, d.Id);
                        }
                        else
                        {
                            _logger.LogError($"Product with upc {upc}, fail by departmentId {departmentId} not found");
                            continue;
                        }
                    }

                    var description = csv.GetField<string>("Name");
                    //var posvisible = GetCsvValue<bool, BooleanConverter>("POS Visible", csv, false);
                    //var foodstamp = GetCsvValue<bool, BooleanConverter>("FoodStampEligible", csv, false);
                    var allozero = GetCsvValue<bool, BooleanConverter>("SellBeyond Zero", csv, false);
                    // var stock = GetCsvValue<bool, BooleanConverter>("Display Stock", csv, false);
                    var print = GetCsvValue<bool, BooleanConverter>("Print Shelf", csv, false);
                    var onlineStore = GetCsvValue<bool, BooleanConverter>("Add to Web", csv, false);
                    // var ebt = GetCsvValue<bool, BooleanConverter>("EBT", csv, false);
                    var price = (decimal)GetCsvValue("Price", csv, typeof(decimal), 0M);
                    var cost = (decimal)GetCsvValue("COSTO", csv, typeof(decimal),0M);

                    //var tare = (decimal)GetCsvValue("Tare", csv, typeof(decimal),0M);

                    //Display Stock
                    //Print Shelf Tag

                    var product = await _productService.GetProductByUPC(upc.ToString());
                    if (product == null)
                    {
                        var wp = new Product
                        {
                            State = true,
                            UPC = upc.ToString(),
                            Name = description,
                            //PosVisible = posvisible,
                            ScaleVisible = false,
                            CategoryId = categorys[(int)catId],
                            DepartmentId = departments[(int)departmentId],
                            ProductType = ProductType.SPT,
                            // SnapEBT = ebt,
                            AllowZeroStock = allozero,
                            NoDiscountAllowed = false,
                            PrintShelfTag = print,
                            // DisplayStockOnPosButton = stock,
                            AddOnlineStore = onlineStore,
                        };
                        var np = await _productService.CreateProduct(wp);

                        await _storeProductService.PostImport(new StoreProduct()
                        {
                            ProductId = np.Id,
                            StoreId = 3,
                            Price = price,
                            Cost = cost,
                            GrossProfit = price - cost / price
                        }, np);
                        if (np != null)
                        {
                            _logger.LogInformation($"Product {upc}-{np.Name} created.");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Product {upc} present.");
                        if (product is Product wproduct)
                        {
                            if (wproduct.CategoryId == 11) continue;
                            wproduct.UPC = upc.ToString();
                            wproduct.Name = description;
                            // wproduct.PosVisible = posvisible;
                            wproduct.CategoryId = categorys[(int)catId];
                            wproduct.DepartmentId = departments[(int)departmentId];
                            wproduct.ProductType = ProductType.SPT;
                            // wproduct.SnapEBT = ebt;
                            wproduct.AllowZeroStock = allozero;
                            wproduct.AddOnlineStore = onlineStore;
                            //wproduct.Tare = tare;


                            var np = await _productService.UpdateProduct(product.Id, wproduct);
                            if (np)
                            {
                                _logger.LogInformation($"Product {upc}-{wproduct.Name} updated.");
                            }
                        }
                        else
                        {
                            _logger.LogError($"Product {upc} is not a wproduct.");
                        }
                    }
                }
            }
        }

        public async Task UpdateSalesProductYavapee()
        {
            var finalDate = new DateTime(2022, 10, 12);
            var orders = await _vendorOrderRepository.GetEntity<VendorOrder>()
                .Include(x => x.VendorOrderDetails)
                //.ThenInclude(vd => vd.Product)
                .OrderBy(x => x.ReceivedDate)
                .Where(x =>
                    x.Status == VendorOrderStatus.Received
                )
                .ToListAsync();
            for (var i = 0; i < orders.Count; i++)
                //foreach (var order in orders)
            {
                _logger.LogInformation(
                    $"Starting order {orders[i].Id} with {orders[i].VendorOrderDetails.Count},  recived day {orders[i].ReceivedDate.ToString("MM/dd/yyyy")}");
                foreach (var detail in orders[i].VendorOrderDetails)
                {
                    if (detail.CaseCost > 0)
                    {
                        var sales = new List<SaleProduct>();
                        if (i < orders.Count - 1)
                        {
                            sales = await _vendorOrderRepository.GetEntity<SaleProduct>()
                                .Include(x => x.Sale)
                                .Where(x =>
                                    x.Sale.SaleTime.Date >= orders[i].ReceivedDate.Date &&
                                    x.Sale.SaleTime <= orders[i + 1].ReceivedDate &&
                                    x.ProductId == detail.ProductId
                                )
                                .ToListAsync();
                        }
                        else
                        {
                            sales = await _vendorOrderRepository.GetEntity<SaleProduct>()
                                .Include(x => x.Sale)
                                .Where(x =>
                                    x.Sale.SaleTime.Date >= orders[i].ReceivedDate.Date &&
                                    x.ProductId == detail.ProductId
                                )
                                .ToListAsync();
                        }

                        foreach (var saleProduct in sales)
                        {
                            saleProduct.Cost = detail.CaseCost;
                        }

                        await _vendorOrderRepository.UpdateRangeAsync(sales);
                    }
                }

                _logger.LogInformation(
                    $"Finish order {orders[i].Id} recived day {orders[i].ReceivedDate.ToString("MM/dd/yyyy")}");
            }

            _logger.LogInformation("end");
        }

        public async Task OutsideVendorProductsImport(string csvFilePath)
        {
            _user.IsAuthenticated = true;
            _user.UserId = "d2f931ca-94ee-4056-8529-d548e96cfecf"; //oftstcuba

            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                //var csvHeaders = csv.HeaderRecord.OrderBy(x => x.ToString()).ToList();
                var products = new List<VendorProduct>();
                var vendorDictionary = new Dictionary<string, long>();
                while (await csv.ReadAsync())
                {
                    var vendorS = csv.GetField<string>("Vendor");
                    long vendor = 0;
                    if (!vendorDictionary.TryGetValue(vendorS, out vendor))
                    {
                        vendor = await _vendorProductRepository.GetEntity<Vendor>()
                            .Where(x => x.Name == vendorS)
                            .Select(x => x.Id)
                            .FirstOrDefaultAsync();
                        if (vendor == 0)
                            continue;
                        vendorDictionary.Add(vendorS, vendor);
                    }

                    var upsS = csv.GetField<string>("UPC");
                    var prod = await _vendorProductRepository.GetEntity<Product>()
                        .Where(x => x.UPC == upsS)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();
                    if (prod == 0)
                    {
                        continue;
                    }

                    var casepack = csv.GetField<int>("Case Pack");
                    var casecost = csv.GetField<decimal>("Unit Cost");
                    var unitcost = casepack > 0 ? casecost / casepack : 0;
                    var prodVendor = new VendorProduct()
                    {
                        ProductCode = csv.GetField<string>("Product Code"),
                        IsPrimary = false,
                        CasePack = casepack,
                        CaseCost = casecost,
                        UnitCost = unitcost,
                        OrderByCase = VendorProductType.CASE,

                        LastOrderDate = null,

                        VendorId = vendor,
                        ProductId = prod
                    };
                    // products.Add(prodVendor);
                    await _vendorProductRepository.CreateAsync(prodVendor);
                }

                // if (products.Count > 0)
                // {
                //     await _vendorProductRepository.CreateRangeAsync(products);
                // }
            }
        }

        public async Task OutsideCustomerImport(string csvFilePath)
        {
            using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                       new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                //var csvHeaders = csv.HeaderRecord.OrderBy(x => x.ToString()).ToList();
                var customers = new List<Customer>();
                while (csv.Read())
                {
                    //"First Name","Last Name","Phone Number","Email Address","Address Line 1","Address Line 2","Address Line 3","City","State / Province","Postal / Zip Code","Country","Customer Since","Marketing Allowed","Additional Addresses"

                    var phone = csv.GetField<string>("Phone Number");
                    //sanitaze phone number
                    phone = phone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace(",", "|");
                    var cust = new Customer()
                    {
                        LastName = csv.GetField<string>("Last Name"),
                        FirstName = csv.GetField<string>("First Name"),
                        Phone = phone,
                        Email = csv.GetField<string>("Email Address"),
                        Address1 = csv.GetField<string>("Address Line 1"),
                        Address2 = csv.GetField<string>("Address Line 2"),
                        CityName = csv.GetField<string>("City"),
                        ProvinceName = csv.GetField<string>("State / Province"),
                        CountryName = csv.GetField<string>("Country"),
                        Zip = csv.GetField<string>("Postal / Zip Code"),
                        MarketingAllowed = csv.GetField<bool, BooleanConverter>("Marketing Allowed"),
                        TaxExcept = false,
                        TaxID = "",
                        StoreCredit = 0,
                        LastBuy = DateTime.UtcNow,
                        //CreatedAt = csv.GetField<DateTime>("Customer Since"), //Customer Since
                        UserCreatorId = "45116265-2a2c-41e0-be38-a194e21d1df1"
                    };
                    if (cust.FirstName == "")
                    {
                        cust.FirstName = "No First Name";
                    }

                    if (cust.LastName == "")
                    {
                        cust.LastName = "No Last Name";
                    }

                    customers.Add(cust);
                }

                if (customers.Count > 0)
                {
                    await _customerRepository.CreateRangeAsync(customers);
                }
            }
        }

        public async Task OutsideGiftCardImport(string csvFilePath)
        {
            try
            {
                using (var csv = new CsvReader(new StreamReader(File.Open(csvFilePath, FileMode.Open)),
                           new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
                {
                    csv.Read();
                    csv.ReadHeader();
                    //var csvHeaders = csv.HeaderRecord.OrderBy(x => x.ToString()).ToList();
                    var customers = new List<GiftCard>();
                    while (csv.Read())
                    {
//CARDNUMBER,REMAINING BALANCE,ACTIVATION DATE,LAST TRANSACTION DATE,STARTING BALANCE
//CARDNUMBER, REMAINING BALANCE ,ACTIVATION DATE,LAST TRANSACTION DATE,LAST FINANCIAL TRANSACTION DATE,LAST POSITIVE TRANSACTION DATE,ACTIVATION MERCHANTNO,ACTIVATION ALT MERCHANTNO,STATE OF ACTIVATION,PROMO NUMBER,PROMO DESC,STARTING BALANCE
                        var priceS = csv.GetField<string>("col2");
                        var priceval = decimal.Parse(priceS.Replace("$", ""));
                        var cust = new GiftCard()
                        {
                            Number = csv.GetField<string>("col1"),
                            Amount = priceval,
                            Balance = priceval,
                            LastUsed = DateTime.UtcNow, //csv.GetField<DateTime>("LAST TRANSACTION DATE"),
                            DateSold = DateTime.UtcNow, //csv.GetField<DateTime>("ACTIVATION DATE"),
                            EmployeeId = 1,
                            EmployeeName = "greta",
                            StoreId = 3,
                            State = true,
                            //CreatedAt = csv.GetField<DateTime>("Customer Since"), //Customer Since
                            UserCreatorId = "480977c0-55e2-48bb-9374-acf5f3ce721d"
                        };
                        if (priceval <= 0) continue;
                        // if (cust.FirstName == "")
                        // {
                        //     cust.FirstName = "No First Name";
                        // }
                        // if (cust.LastName == "")
                        // {
                        //     cust.LastName = "No Last Name";
                        // }
                        var g = await _giftCardRepository.GetEntity<GiftCard>().Where(x => x.Number == cust.Number)
                            .FirstOrDefaultAsync();
                        if (g == null)
                            customers.Add(cust);
                    }

                    if (customers.Count > 0)
                    {
                        await _giftCardRepository.CreateRangeAsync(customers);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Problem to response.");
            }
        }

        private TReturn GetCsvValue<TReturn, TConverter>(string key, CsvReader csv, TReturn defaultReturn)
            where TConverter : ITypeConverter
        {
            try
            {
                return csv.GetField<TReturn, TConverter>(key);
            }
            catch
            {
                return defaultReturn;
            }
        }

        private object GetCsvValue(string key, CsvReader csv, Type type, object defaultReturn)
        {
            try
            {
                return csv.GetField(type, key);
            }
            catch
            {
                return defaultReturn;
            }
        }
    }
}