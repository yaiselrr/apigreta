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

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters
{
    public interface IFamilyExport: IModelExporter<Family, FamilyExportModel>
    {
    }
    public class FamilyExport : BaseExport<Family, FamilyExportModel>,IFamilyExport
    {
        private readonly IFamilyRepository _repository;

        public FamilyExport(ILogger<FamilyExport> logger, IFamilyRepository repository, IMapper mapper,
             INotifier notifier)
           : base(logger, mapper, notifier)
        {
            _repository = repository;
        } 

        public override async Task<string> Process(List<string>? columns, long? storeId)
        {
            var data = _repository.GetEntity<Family>().Where(x => x.State);
            
            _totalRows = data.Count(); 
            var cols = ConcatColumns(columns);
            var list = data.Select(x => new FamilyExportModel(){Id = x.Id, Name = x.Name}).Select(cols);
            return await Build(list, columns);
        }
    }
}
