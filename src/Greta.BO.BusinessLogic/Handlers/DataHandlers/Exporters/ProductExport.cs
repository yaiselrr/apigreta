#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Greta.BO.BusinessLogic.Service;
using System.Data;
using System.Linq;
using Greta.BO.Api.Abstractions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.Api.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters
{
    public interface IProductExport: IModelExporter<Product, ProductExportModel>
    {
    }
    public class ProductExport : BaseExport<Product, ProductExportModel>,IProductExport
    {
        private readonly IProductRepository _repository;

        public ProductExport(ILogger<ProductExport> logger, IProductRepository repository, IMapper mapper,
            INotifier notifier)
           : base(logger, mapper, notifier)
        {
            _repository = repository;
        } 

        public override async Task<string> Process(List<string>? columns, long? storeId)
        {
            var data = _repository.GetEntity<StoreProduct>()
                .Where(x => x.State && x.Product.ProductType == ProductType.SPT)
                .WhereIf(storeId.HasValue, x => x.StoreId == storeId!.Value)
                .Include(x => x.Product)
                .ThenInclude(p => p.Department)
                .Include("Product.Category");
            _totalRows = data.Count(); 
            var cols = ConcatColumns(columns);
            var productList = data.Select( x => ConvertProductExportModel(x, _mapper)).Select(cols);
            return await Build(productList, columns);
        }

        private static ProductExportModel ConvertProductExportModel(StoreProduct x, IMapper mapper)
        {
            ProductExportModel temp = mapper.Map<ProductExportModel>(x.Product);
            temp.QtyHand = x.QtyHand;
            temp.Price = x.Price;
            temp.Cost = x.Cost;
            return temp;
        }
    }
}
