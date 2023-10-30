using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListDetail;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record CutListDetailCreateCommand(CutListDetailListModel Entity) : IRequest<CutListDetailCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListDetailCreateHandler : IRequestHandler<CutListDetailCreateCommand, CutListDetailCreateResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListDetailService _service;
    private readonly ICutListService _serviceCutList;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="serviceCutList"></param>
    /// <param name="mapper"></param>
    public CutListDetailCreateHandler(
        ICutListDetailService service,
        ICutListService serviceCutList,
        IMapper mapper)
    {
        _service = service;
        _serviceCutList = serviceCutList;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListDetailCreateResponse> Handle(CutListDetailCreateCommand request, CancellationToken cancellationToken)
    {
        var cutList = await _serviceCutList.GetWithDetails(request.Entity.CutList);
        var tempDetails = cutList.CutListDetails;
        cutList.CutListDetails = new List<Api.Entities.CutListDetail>();
        await _serviceCutList.Put(cutList.Id, cutList);

        var entity = _mapper.Map<List<Api.Entities.CutListDetail>>(request.Entity.Elements);
        var result = await _service.PostMultiple(entity, cancellationToken);

        if (!result)
        {
            cutList.CutListDetails = tempDetails;
            await _serviceCutList.Put(cutList.Id, cutList);
        }
        return new CutListDetailCreateResponse { Data = result };
    }
}

/// <inheritdoc />
public record CutListDetailCreateResponse : CQRSResponse<bool>;