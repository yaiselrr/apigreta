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
/// Command for update a Fee
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record FeeUpdateCommand(long Id, FeeModel Entity) : IRequest<FeeUpdateResponse>, IAuthorizable
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
public class FeeUpdateValidator : AbstractValidator<FeeUpdateCommand>
{
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public FeeUpdateValidator(IFeeService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Fee name already exists");
    }

    private async Task<bool> NameUnique(FeeUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var feeExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Fee>(name, command.Id));
        return feeExist == null;
    }
}

///<inheritdoc/>
public class FeeUpdateHandler : IRequestHandler<FeeUpdateCommand, FeeUpdateResponse>
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
    public FeeUpdateHandler(ILogger<FeeUpdateHandler> logger, IFeeService service, IMapper mapper)
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
    public async Task<FeeUpdateResponse> Handle(FeeUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Fee>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Fee {RequestId} update successfully", request.Id);
        return new FeeUpdateResponse { Data = success};
    }
}

///<inheritdoc/>
public record FeeUpdateResponse : CQRSResponse<bool>;
