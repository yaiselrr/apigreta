using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IScaleReasonCodesService : IGenericBaseService<ScaleReasonCodes>
    {
        Task<ScaleReasonCodes> GetByName(string name, long Id = -1);
    }

    public class ScaleReasonCodesService : BaseService<IScaleReasonCodesRepository, ScaleReasonCodes>, IScaleReasonCodesService
    {
        public ScaleReasonCodesService(IScaleReasonCodesRepository repository,
            ISynchroService synchroService,
            ILogger<ScaleReasonCodesService> logger)
            : base(repository, logger, synchroService, Converter)
        {
        }

        private static object Converter(ScaleReasonCodes from) => LiteScaleReasonCodes.Convert(from);

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">ScaleCategory name</param>
        /// <returns>ScaleCategory</returns>
        public async Task<ScaleReasonCodes> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<ScaleReasonCodes>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<ScaleReasonCodes>().Where(x => x.Name == name && x.Id != Id)
                .FirstOrDefaultAsync();
        }       

        protected override IQueryable<ScaleReasonCodes> FilterqueryBuilder(
            ScaleReasonCodes filter,
            string searchstring,
            string[] splited,
            DbSet<ScaleReasonCodes> query)
        {
            IQueryable<ScaleReasonCodes> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Name);

            return query1.IgnoreAutoIncludes();
        }
    }
}
