using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface ITenderTypeService : IGenericBaseService<TenderType>
    {
        Task<TenderType> GetByName(string name, long Id = -1);
    }

    public class TenderTypeService : BaseService<ITenderTypeRepository, TenderType>, ITenderTypeService
    {
        public TenderTypeService(ITenderTypeRepository tenderTypeRepository, ILogger<TenderTypeService> logger, ISynchroService synchroService)
            : base(tenderTypeRepository, logger, synchroService, Converter)
        {
        }

        private static object Converter(TenderType from) => (LiteTenderType.Convert(from));

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">TenderType name</param>
        /// <returns>TenderType</returns>
        public async Task<TenderType> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<TenderType>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<TenderType>().Where(x => x.Name == name && x.Id != Id)
                .FirstOrDefaultAsync();
        }


        protected override IQueryable<TenderType> FilterqueryBuilder(
            TenderType filter,
            string searchstring,
            string[] splited,
            DbSet<TenderType> query)
        {
            IQueryable<TenderType> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring) || c.DisplayAs.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(!string.IsNullOrEmpty(filter.DisplayAs), c => c.DisplayAs.Contains(filter.DisplayAs));

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "displayas" && e[1] == "asc", e => e.DisplayAs)
                .OrderByDescendingCase(e => e[0] == "displayas" && e[1] == "desc", e => e.DisplayAs)
                .OrderByDefault(e => e.Name);

            return query1;
        }

        public override async Task<bool> ChangeState(long id, bool state)
        {
            var success = await _repository.ChangeStateAsync<TenderType>(id, state);
            var entity = await _repository.GetEntity<TenderType>()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            
            await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);

            return success;
        }
    }
}