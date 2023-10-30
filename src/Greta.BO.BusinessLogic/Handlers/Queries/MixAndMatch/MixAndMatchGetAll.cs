using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;

/// <summary>
/// Query for get all MixAndMatch entities
/// </summary>
public record MixAndMatchGetAllQuery : IRequest<MixAndMatchGetAllResponse>, IAuthorizable
{
/// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("view_mix_and_match")
    };
}

///<inheritdoc/>
public class MixAndMatchGetAllHandler : IRequestHandler<MixAndMatchGetAllQuery, MixAndMatchGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IMixAndMatchService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public MixAndMatchGetAllHandler(IMixAndMatchService service, IMapper mapper)
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
    public async Task<MixAndMatchGetAllResponse> Handle(MixAndMatchGetAllQuery request, CancellationToken cancellationToken=default)
    {
        var entities = await _service.Get();
        return new MixAndMatchGetAllResponse { Data = _mapper.Map<List<MixAndMatchGetAllModel>>(entities)};
    }
}

///<inheritdoc/>
public record MixAndMatchGetAllResponse : CQRSResponse<List<MixAndMatchGetAllModel>>;
