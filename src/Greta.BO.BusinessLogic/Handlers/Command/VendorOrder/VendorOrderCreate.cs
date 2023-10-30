using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;

/// <inheritdoc />
public record VendorOrderCreateCommand(VendorOrderModel Entity) : IRequest<VendorOrderCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderCreateHandler : IRequestHandler<VendorOrderCreateCommand, VendorOrderCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderService _service;
    private readonly IBOUserService _userService;
    private readonly IAuthenticateUser<string> _auth;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="userService"></param>
    /// <param name="auth"></param>
    /// <param name="mapper"></param>
    public VendorOrderCreateHandler(
        ILogger<VendorOrderCreateHandler> logger,
        IVendorOrderService service,
        IBOUserService userService,
        IAuthenticateUser<string> auth,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _userService = userService;
        _auth = auth;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderCreateResponse> Handle(VendorOrderCreateCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.VendorOrder>(request.Entity);

        var user = await _userService.GetByUserId(_auth.UserId);

        if (user == null)
            throw new BusinessLogicException("The user can not be null");

        entity.UserId = user.Id;
        entity.OrderedDate = null;

        var result = await _service.Post(entity);
        return new VendorOrderCreateResponse { Data = _mapper.Map<VendorOrderModel>(result) };
    }
}

/// <inheritdoc />
public record VendorOrderCreateResponse : CQRSResponse<VendorOrderModel>;