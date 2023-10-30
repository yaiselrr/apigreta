#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CSVMapping
{
    public static class CSVEntityExport
    {
        public record Query(string model, List<string>? columns, long? storeId) : IRequest<Response>;
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ILogger _logger;
            private readonly ICategoryExport _categoryExport;
            private readonly IDepartmentExport _departmentExport;
            private readonly IFamilyExport _familyExport;
            private readonly IProductExport _productExport;
            private readonly IScaleCategoryExport _scaleCategoryExport;
            private readonly IScaleProductExport _scaleProductExport;

            public Handler(
                ILogger<Handler> logger,
                ICategoryExport categoryExport,
                IDepartmentExport departmentExport,
                IFamilyExport familyExport,
                IProductExport productExport,
                IScaleCategoryExport scaleCategoryExport,
                IScaleProductExport scaleProductExport)
            {
                _logger = logger;
                _categoryExport = categoryExport;
                _departmentExport = departmentExport;
                _familyExport = familyExport;
                _productExport = productExport;
                _scaleCategoryExport = scaleCategoryExport;
                _scaleProductExport = scaleProductExport;

            }
            
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Starting export");
                var result = string.Empty;
                if(request.model.ToLower() == "category")
                {
                    var columnsCat = (request.columns != null && request.columns.Count > 0) ? request.columns : 
                        new CategoryExportModel().GetBaseProperties(includeId: false);
                    result = await _categoryExport.Process(columnsCat , request.storeId);
                } 
                else if(request.model.ToLower() == "department")
                {
                    var columnsDepartment = (request.columns != null && request.columns.Count > 0) ? request.columns : 
                        new DepartmentExportModel().GetBaseProperties(includeId: false);
                    result = await _departmentExport.Process(columnsDepartment, request.storeId);
                }
                else if(request.model.ToLower() == "family")
                {
                    var columnsFfamily = (request.columns != null && request.columns.Count > 0) ? request.columns : 
                        new FamilyExportModel().GetBaseProperties(includeId: false);
                    result = await _familyExport.Process(columnsFfamily, request.storeId);
                }
                else if(request.model.ToLower() == "product")
                {
                    var columnsProduct = (request.columns != null && request.columns.Count > 0) ? request.columns : 
                        new ProductExportModel().GetBaseProperties(includeId: false);
                    result = await _productExport.Process(columnsProduct, request.storeId);
                }
                else if(request.model.ToLower() == "scale_category")
                {
                    var columnsScaleCategory = (request.columns != null && request.columns.Count > 0) ? request.columns : 
                        new ScaleCategoryExportModel().GetBaseProperties(includeId: false);
                    result = await _scaleCategoryExport.Process(columnsScaleCategory, request.storeId);
                }
                else if(request.model.ToLower() == "scale_product")
                {
                    var columnsScaleProduct = (request.columns != null && request.columns.Count > 0) ? request.columns : 
                        new ScaleProductExportModel().GetBaseProperties(includeId: false);
                    result = await _scaleProductExport.Process(columnsScaleProduct, request.storeId);
                }
                _logger.LogInformation("Ending export");
                return new Response { Data = result };
            }
        }

        public record Response : CQRSResponse<string> {}
    }
}
