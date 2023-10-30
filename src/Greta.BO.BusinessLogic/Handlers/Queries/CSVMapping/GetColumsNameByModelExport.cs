using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Column;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CSVMapping
{
    public static class GetColumsNameByModelExport
    {
        public record Query(ModelImport modelExport) : IRequest<Response>
        {

        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly ICSVMappingService _service;

            public Handler(ILogger<Handler> logger, ICSVMappingService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response
                    {Data = _service.GetColumsNameByModelExport(request.modelExport) });
            }
        }

        public record Response : CQRSResponse<List<ColumnNameModel>>;
    }
}