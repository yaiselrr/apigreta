using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Discount;

/// <summary>
/// Query for get all Discount entities
/// </summary>
public record DiscountGetAllQuery : IRequest<DiscountGetAllResponse>, IAuthorizable
{
/// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Discount).ToLower()}")
    };
}

///<inheritdoc/>
public class DiscountGetAllHandler : IRequestHandler<DiscountGetAllQuery, DiscountGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DiscountGetAllHandler(IDiscountService service, IMapper mapper)
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
    public async Task<DiscountGetAllResponse> Handle(DiscountGetAllQuery request, CancellationToken cancellationToken=default)
    {
        var entities = await _service.Get();
        return new DiscountGetAllResponse { Data = _mapper.Map<List<DiscountModel>>(entities)};
    }
}

///<inheritdoc/>
public record DiscountGetAllResponse : CQRSResponse<List<DiscountModel>>;
