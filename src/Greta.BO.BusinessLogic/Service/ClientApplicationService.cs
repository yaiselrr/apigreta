using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IClientApplicationService : IGenericBaseService<ClientApplication>
    {
        Task<ClientApplication> GetById(long id);
    }

    public class ClientApplicationService : BaseService<IClientApplicationRepository, ClientApplication>,
        IClientApplicationService
    {
        public ClientApplicationService(IClientApplicationRepository repository,
            ILogger<ClientApplicationService> logger)
            : base(repository, logger)
        {
        }

        public async Task<ClientApplication> GetById(long id)
        {
            return await _repository.GetEntity<ClientApplication>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}