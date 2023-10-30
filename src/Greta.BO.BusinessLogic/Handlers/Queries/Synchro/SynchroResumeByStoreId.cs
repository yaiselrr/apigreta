using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Synchro
{
    public static class SynchroResumeByStoreId
    {
        public record Query(long StoreId) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ISynchroService _service;

            public Handler(ISynchroService service)
            {
                _service = service;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _service.GetStatisticsByStore(request.StoreId);
                return new Response {Data = data};
            }
        }

        public record Response : CQRSResponse<SynchroStatistics>;
    }
}