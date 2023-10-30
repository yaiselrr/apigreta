using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;

/// <summary>
/// Round the price based on the rounding table
/// </summary>
/// <param name="Price"></param>
public record RoundPriceQuery(decimal Price): IRequest<RoundPriceResponse>;

/// <summary>
/// Handler for <see cref="RoundPriceQuery"/>
/// </summary>
public class RoundPriceHandler: IRequestHandler<RoundPriceQuery, RoundPriceResponse>
{
    private readonly IRoundingTableService _roundingTableService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="roundingTableService"></param>
    public RoundPriceHandler(IRoundingTableService roundingTableService)
    {
        _roundingTableService = roundingTableService;
    }

    /// <inheritdoc />
    public async Task<RoundPriceResponse> Handle(RoundPriceQuery request, CancellationToken cancellationToken = default)
    {
        var stringPrice = request.Price.ToString("N2");
        var lastDigit = int.Parse(stringPrice[^1].ToString()) ;
        var endDigit = await _roundingTableService.ChangeBy(lastDigit, cancellationToken);
        return new RoundPriceResponse{Data = endDigit < 0 ? request.Price : decimal.Parse($"{stringPrice[..^1]}{endDigit}")} ;
    }
}

/// <inheritdoc />
public record RoundPriceResponse : CQRSResponse<decimal>;