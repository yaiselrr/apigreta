using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;

/// <inheritdoc />
public record VendorOrderToCsvCommand
    (string StoreName, List<Api.Entities.VendorOrderDetail> Detail) : IRequest<VendorOrderToCsvResponse>;

/// <inheritdoc />
public record VendorOrderToCsvResponse : CQRSResponse<string>;

/// <inheritdoc />
public class VendorOrderToCsvHandler : IRequestHandler<VendorOrderToCsvCommand, VendorOrderToCsvResponse>
{
    private readonly IStorageProvider _storage;
    private readonly StorageOption _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="storage"></param>
    /// <param name="options"></param>
    public VendorOrderToCsvHandler(
        IStorageProvider storage,
        IOptions<StorageOption> options
    )
    {
        _storage = storage;
        _options = options.Value;
    }

    /// <inheritdoc />
    public Task<VendorOrderToCsvResponse> Handle(VendorOrderToCsvCommand request, CancellationToken cancellationToken)
    {
        var headers = new List<string>() { "UPC", "ProductCode", "Name", "Order Quantity" };
        var path = Path.GetTempPath() + Guid.NewGuid() + ".csv";
        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine(string.Join(", ", headers));

            foreach (var item in request.Detail)
            {
                var it = new List<string>()
                {
                    item.Product.UPC,
                    item.ProductCode,
                    item.Product.Name,
                    item.OrderAmount.ToString(CultureInfo.CurrentCulture), 
                };
                writer.WriteLine(string.Join(", ", it));
            }
        }

        return Task.FromResult(new VendorOrderToCsvResponse() { Data = path });
    }
}