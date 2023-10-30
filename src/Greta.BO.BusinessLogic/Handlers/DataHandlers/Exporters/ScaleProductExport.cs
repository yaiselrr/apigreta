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
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters
{
    public interface IScaleProductExport: IModelExporter<ScaleProduct, ScaleProductExportModel>
    {
    }
    public class ScaleProductExport : BaseExport<ScaleProduct, ScaleProductExportModel>, IScaleProductExport
    {
        private readonly IStoreProductRepository _repository;

        public ScaleProductExport(ILogger<ScaleProductExport> logger, IStoreProductRepository repository, IMapper mapper,
            INotifier notifier)
           : base(logger, mapper, notifier)
        {
            _repository = repository;
            // Model = "ScaleProduct";
        } 

        public override async Task<string> Process(List<string>? columns, long? storeId)
        {
            var data = _repository.GetEntity<StoreProduct>()
                .Where(x => x.State && x.Product.ProductType == ProductType.SLP)
                .WhereIf(storeId.HasValue, x => x.StoreId == storeId!.Value)
                .Include(x => x.Product)
                .ThenInclude(p => p.Department)
                .Include("Product.Category")
                .Include("Product.ScaleCategory");
            
            _totalRows = data.Count();
            var cols = ConcatColumns(columns);
            var list = data.Select(x => ConvertProductExportModel(x, _mapper)).Select(cols);
            return await Build(list, columns);
        }
        
        private static ScaleProductExportModel ConvertProductExportModel(StoreProduct x, IMapper mapper)
        {
            ScaleProductExportModel temp = mapper.Map<ScaleProductExportModel>(x.Product as ScaleProduct);
            temp.QtyHand = x.QtyHand;
            temp.Price = x.Price;
            temp.Cost = x.Cost;
            return temp;
        }
    }
}
