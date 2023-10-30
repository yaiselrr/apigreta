using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CSVMapping;

public record GetColumnsNameQuery
    (IFormFile CsvFile, char Separator, ModelImport ModelImport) : IRequest<GetColumsNameResponse> //, IAuthorizable
{
    // public List<IRequirement> Requirements => new()
    // {
    //     new PermissionRequirement.Requirement($"view_csv_mapping")
    // };
}

public class GetColumsNameHandler : IRequestHandler<GetColumnsNameQuery, GetColumsNameResponse>
{
    private readonly ICSVMappingService _service;

    public GetColumsNameHandler(ICSVMappingService service)
    {
        _service = service;
    }

    public Task<GetColumsNameResponse> Handle(GetColumnsNameQuery request, CancellationToken cancellationToken)
    {
        var data = _service.GetColumsName(
            request.CsvFile, 
            request.Separator, 
            request.ModelImport
        );
        return Task.FromResult(new GetColumsNameResponse
            { Data = new ColumnsDefinitionsAndSugestions()
            {
                ColumnsHeader = data.Item1,
                ColumnsDefinitions = data.Item2,
                ColumnsSugestions = data.Item3,
            } });
    }
}

public record GetColumsNameResponse : CQRSResponse<ColumnsDefinitionsAndSugestions>;

public class ColumnsDefinitionsAndSugestions
{
    public List<string> ColumnsHeader { get; set; }
    public List<string> ColumnsDefinitions { get; set; }
    public List<string> ColumnsSugestions { get; set; }
}
