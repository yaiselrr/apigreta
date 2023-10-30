using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Products
{
    public static class CSVGetAllProductExport
    {
        public record Query(List<string> columns) : IRequest<Response>//, IAuthorizable
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
            protected readonly IProductService _productService;







            public Handler(ILogger<Handler> logger, ICSVMappingService service, IProductService productService,IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _productService = productService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var productList= (await _productService.Get()).AsQueryable();
                var columns = ConcatColumns(request.columns);
                var filtereResult = productList.Select(columns);
                var export = new CSVExportProductResponse();
                export.CsvHeader = new List<ProductModel>();
                export.ModelHeader = request.columns;
                export.ModelImport = ModelImport.PRODUCT;
                export.Name = "Product";
                foreach (var item in filtereResult)
                {
                    var p = _mapper.Map<ProductModel>(item);
                    export.CsvHeader.Add(p);
                }


                return new Response { Data=export};
            }
            public string ConcatColumns(List<string> columns)
            {
                string selectedStament = "" ;
                string result = "";
                foreach (var item in columns)
                {
                    selectedStament = selectedStament+ item+",";
                }
                result = "new ( " + selectedStament + ")";
                return result;
            }
        }

        public record Response : CQRSResponse<CSVExportProductResponse>;
    }
}

