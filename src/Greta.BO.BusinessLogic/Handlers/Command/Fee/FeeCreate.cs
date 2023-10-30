using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Fee;

/// <summary>
/// Command for create new Fee
/// </summary>
/// <param name="Entity"></param>
public record FeeCreateCommand(FeeModel Entity) : IRequest<FeeCreateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Fee).ToLower()}")
    };
}

///<inheritdoc/>
public class FeeCreateValidator : AbstractValidator<FeeCreateCommand>
{
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public FeeCreateValidator(IFeeService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Fee name already exists");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var feeExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Fee>(name));
        return feeExist == null;
    }
}

///<inheritdoc/>
public class FeeCreateHandler : IRequestHandler<FeeCreateCommand, FeeCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FeeCreateHandler(ILogger<FeeCreateHandler> logger, IFeeService service, IMapper mapper)
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
    public async Task<FeeCreateResponse> Handle(FeeCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Fee>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Fee {ResultName} for user {ResultUserCreatorId}", result.Name, result.UserCreatorId);
        return new FeeCreateResponse { Data = _mapper.Map<FeeModel>(result)};
    }
}

///<inheritdoc/>
public record FeeCreateResponse : CQRSResponse<FeeModel>;
