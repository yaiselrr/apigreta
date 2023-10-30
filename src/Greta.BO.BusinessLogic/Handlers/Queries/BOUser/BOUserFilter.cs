using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Middleware;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Queries.User;

public record BOUserFilterQuery
    (int CurrentPage, int PageSize, BOUserSearchModel Filter) : IRequest<BOUserFilterResponse>, IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(User).ToLower()}")
    };
}

public class BOUserFilterValidator : AbstractValidator<BOUserFilterQuery>
{
    public BOUserFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

public class Handler : IRequestHandler<BOUserFilterQuery, BOUserFilterResponse>
{
    protected readonly IAuthenticateUser<string> _authenticateUser;
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly IBOUserService _service;
    private readonly MainOption options;

    public Handler(
        IOptions<MainOption> options,
        ILogger<Handler> logger,
        IBOUserService service,
        IMapper mapper,
        IAuthenticateUser<string> authenticateUser)
    {
        this.options = options.Value;
        _authenticateUser = authenticateUser;
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task<BOUserFilterResponse> Handle(BOUserFilterQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }

        //get pager from identity
        var pagerUsersIdentity = await _service.FilterUserAsync(
            _authenticateUser.UserId,
            options.CompanyCode, //Change this with database call to configuration
            _mapper.Map<Api.Entities.BOUser>(request.Filter),
            request.Filter.Search,
            request.Filter.Sort,
            request.CurrentPage,
            request.PageSize);
        //get usersBO
        var ids = pagerUsersIdentity.Data.Select(x => x.Id).ToList();
        var usersBo =
            await _service.GetUsersBoByFilterAsync(ids);
        if (usersBo.Count == 0)
            return new BOUserFilterResponse
                { Data = new Pager<BOUserModel>(0, new List<BOUserModel>(), 1, request.PageSize) };
        //make UserDto list
        var users = pagerUsersIdentity.Data
            .Select(e => new BOUserModel(usersBo.FirstOrDefault(u => u.UserId == e.Id), e)).ToList();

        return new BOUserFilterResponse
            { Data = new Pager<BOUserModel>(users.Count, users, request.CurrentPage, request.PageSize) };
    }
}

public record BOUserFilterResponse : CQRSResponse<Pager<BOUserModel>>;