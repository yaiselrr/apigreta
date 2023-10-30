using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IRancherService : IGenericBaseService<Rancher>
    {
        Task<Rancher> GetByName(string name, long Id = -1);
    }

    public class RancherService : BaseService<IRancherRepository, Rancher>, IRancherService
    {
        public RancherService(IRancherRepository repository, ILogger<RancherService> logger)
            : base(repository, logger)
        {
        }

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">Rancher name</param>
        /// <returns>Rancher</returns>
        public async Task<Rancher> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<Rancher>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<Rancher>().Where(x => x.Name == name && x.Id != Id).FirstOrDefaultAsync();
        }

        protected override IQueryable<Rancher> FilterqueryBuilder(
            Rancher filter,
            string searchstring,
            string[] splited,
            DbSet<Rancher> query)
        {
            IQueryable<Rancher> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Name);

            return query1;
        }
    }
}