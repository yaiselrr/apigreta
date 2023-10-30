using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Fee;

/// <summary>
/// Query for get all fees
/// </summary>
public record FeeGetAllQuery : IRequest<FeeGetAllResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Fee).ToLower()}")
    };
}

///<inheritdoc/>
public class FeeGetAllHandler : IRequestHandler<FeeGetAllQuery, FeeGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FeeGetAllHandler(IFeeService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FeeGetAllResponse> Handle(FeeGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new FeeGetAllResponse { Data = _mapper.Map<List<FeeGetAllModel>>(entities)};
    }
}

///<inheritdoc/>
public record FeeGetAllResponse : CQRSResponse<List<FeeGetAllModel>>;
