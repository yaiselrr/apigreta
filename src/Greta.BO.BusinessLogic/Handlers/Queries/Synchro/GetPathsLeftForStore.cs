using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Synchro
{
    public static class GetPathsLeftForStore
    {
        public record Query(long storeId) : IRequest<List<Api.Entities.Synchro>>;

        public class Handler : IRequestHandler<Query, List<Api.Entities.Synchro>>
        {
            private readonly ISynchroService _service;

            public Handler(ISynchroService service)
            {
                _service = service;
            }

            public async Task<List<Api.Entities.Synchro>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _service.GetPathsLeftForStore(request.storeId);
            }
        }
    }
}