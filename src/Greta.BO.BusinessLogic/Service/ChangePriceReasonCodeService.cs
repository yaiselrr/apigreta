using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IChangePriceReasonCodeService : IGenericBaseService<ChangePriceReasonCode>
    {
    }
    public class ChangePriceReasonCodeService : BaseService<IChangePriceReasonCodeRepository, ChangePriceReasonCode>, IChangePriceReasonCodeService
    {
        public ChangePriceReasonCodeService(IChangePriceReasonCodeRepository repository,
            ILogger<ChangePriceReasonCodeService> logger)
            : base(repository, logger)
        {
        }
    }
}