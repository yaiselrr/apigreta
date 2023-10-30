using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CSVMapping
{
    public static class CSVMappingGetById
    {
        public record Query(long Id) : IRequest<Response>//, IAuthorizable
        {
            // public List<IRequirement> Requirements => new()
            // {
            //     new PermissionRequirement.Requirement($"view_csv_mapping")
            // };
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

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var mapping = await _service.GetById(request.Id);
                var mappingDto = _mapper.Map<CSVMappingModel>(mapping);
                if (mapping != null)
                {
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapping.MapperJson);
                    mappingDto.ModelHeader = dictionary.Values.ToList();
                    mappingDto.CsvHeader = dictionary.Keys.ToList();
                }

                return new Response {Data = mappingDto};
            }
        }

        public record Response : CQRSResponse<CSVMappingModel>;
    }
}