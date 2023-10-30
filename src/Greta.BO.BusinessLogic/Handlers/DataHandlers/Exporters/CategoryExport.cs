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
    public interface ICategoryExport: IModelExporter<Category, CategoryExportModel>
    {
    }

    public class CategoryExport : BaseExport<Category, CategoryExportModel>, ICategoryExport
    {
        private readonly ICategoryRepository _repository;

        public CategoryExport(ILogger<CategoryExport> logger, ICategoryRepository repository, IMapper mapper, INotifier notifier)
           : base(logger, mapper, notifier)
        {
            _repository = repository;
        } 

        public override async Task<string> Process(List<string>? columns, long? storeId)
        {
            // var service = _provider.GetRequiredService<ICategoryService>();
            // var data = (await service.Get()).AsQueryable();
            var data = _repository.GetEntity<Category>()
                .Where(x => x.State)
                .Include(x => x.Department);
            _totalRows = data.Count();
            var cols = ConcatColumns(columns);
            var list = data.Select(x => _mapper.Map<CategoryExportModel>(x)).Select(cols);
            return await Build(list, columns);
        }
    }
}
