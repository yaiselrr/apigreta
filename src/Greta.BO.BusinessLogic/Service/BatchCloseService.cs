using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IBatchCloseService : IGenericBaseService<BatchClose>
    {
    }
    public class BatchCloseService: BaseService<IBatchCloseRepository, BatchClose>, IBatchCloseService
    {
        public BatchCloseService(IBatchCloseRepository repository,
            ILogger<BatchCloseService> logger)
            : base(repository, logger)
        {
        }
        
    }
}