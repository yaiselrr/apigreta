using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.FunctionGroup;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <param name="applicationId"></param>
public record FunctionGroupGetAllQuery(long applicationId) : IRequest<FunctionGroupGetAllResponse>;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class FunctionGroupGetAllHandler : IRequestHandler<FunctionGroupGetAllQuery, FunctionGroupGetAllResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFunctionGroupService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FunctionGroupGetAllHandler(ILogger<FunctionGroupGetAllHandler> logger, IFunctionGroupService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FunctionGroupGetAllResponse> Handle(FunctionGroupGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.GetByAplication(request.applicationId);
        var r = _mapper.Map<List<FunctionGroupModel>>(entities);
        return new FunctionGroupGetAllResponse { Data = r};
    }
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public record FunctionGroupGetAllResponse : CQRSResponse<List<FunctionGroupModel>>;
