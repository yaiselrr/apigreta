using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.TimeKeepingSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;

public record TimeKeepingFilterQuery(int CurrentPage, int PageSize, TimeKeepingUserSearchModel Filter): IRequest<TimeKeepingFilterResponse>;
public record TimeKeepingFilterResponse: CQRSResponse<Pager<TimeKeepingModel>>;


/// <inheritdoc />
public class TimeKeepingFilterValidator : AbstractValidator<TimeKeepingFilterQuery>
{
    /// <inheritdoc />
    public TimeKeepingFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

public class TimeKeepingFilterHandler: IRequestHandler<TimeKeepingFilterQuery, TimeKeepingFilterResponse>
{
    private readonly ITimeKeepingService _timeKeepingService;


    public TimeKeepingFilterHandler(ITimeKeepingService timeKeepingService)
    {
        _timeKeepingService = timeKeepingService;
    }


    public async Task<TimeKeepingFilterResponse> Handle(TimeKeepingFilterQuery request, CancellationToken cancellationToken)
    {
        var spec = new TimeKeepingFilterSpec(request.Filter);
        var entities = await _timeKeepingService.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new TimeKeepingFilterResponse { Data = entities };
    }
}