using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Fee;

/// <summary>
/// Query for get Fee by id
/// </summary>
/// <param name="Id"></param>
public record FeeGetByIdQuery(long Id) : IRequest<FeeGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement($"view_{nameof(Fee).ToLower()}")
    };

    /// <summary>
    /// 
    /// </summary>
    public string CacheKey => $"FeeGetById{Id}";
}

///<inheritdoc/>
public class FeeGetByIdHandler : IRequestHandler<FeeGetByIdQuery, FeeGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FeeGetByIdHandler(IFeeService service, IMapper mapper)
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
    public async Task<FeeGetByIdResponse> Handle(FeeGetByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<FeeGetByIdModel>(entity);
        return data == null ? null : new FeeGetByIdResponse { Data = data};
    }
}

///<inheritdoc/>
public record FeeGetByIdResponse : CQRSResponse<FeeGetByIdModel>;
