using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Greta.BO.BusinessLogic.Exceptions;

namespace Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record PriceBatchDetailFileImportCommand(PriceBatchDetailModel Entity) : IRequest<PriceBatchDetailFileImportResponse>;

/// <inheritdoc />
public class PriceBatchDetailFileImportHandler : IRequestHandler<PriceBatchDetailFileImportCommand, PriceBatchDetailFileImportResponse>
{
    private readonly ILogger _logger;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public PriceBatchDetailFileImportHandler(
        ILogger<PriceBatchDetailFileImportHandler> logger,
        IPriceBatchDetailService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailFileImportResponse> Handle(PriceBatchDetailFileImportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var csv = new CsvReader(new StreamReader(request.Entity.Csv.OpenReadStream()),
                new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," });
            await csv.ReadAsync();
            csv.ReadHeader();
            while (await csv.ReadAsync())
            {
                try
                {
                    var temp = new Api.Entities.PriceBatchDetail()
                    {
                        HeaderId = request.Entity.HeaderId
                    };

                    var upc = csv.GetField<String>("upc");
                    var price = csv.GetField<decimal>("price");

                    var productId = await _service.GetProductIdByUpc(upc);
                    if (productId == 0) continue;
                    _logger.LogInformation("Found product id {ProductId}",productId);
                    var exist = await _service.GetByProductAndHEader(productId, request.Entity.HeaderId);
                    if (exist != null)
                    {
                        _logger.LogWarning($"Found detail with this data");
                        continue;
                    }
                    temp.Price = price;
                    temp.ProductId = productId;
                    await _service.Post(temp);
                }
                catch
                {
                    // ignored
                }
            }

            return new PriceBatchDetailFileImportResponse() { Data = true };
        }
        catch
        {
            throw new BusinessLogicException("Csv bad format.");
        }
    }
}

/// <inheritdoc />
public record PriceBatchDetailFileImportResponse : CQRSResponse<bool>;