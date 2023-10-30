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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetailCredit;

/// <inheritdoc />
public record VendorOrderDetailCreditToCsvCommand(string StoreName,
    List<Api.Entities.VendorOrderDetailCredit> DetailCredits) : IRequest<VendorOrderDetailCreditToCsvResponse>;

/// <inheritdoc />
public record VendorOrderDetailCreditToCsvResponse : CQRSResponse<string>;

/// <inheritdoc />
public class VendorOrderDetailCreditToCsvHandler : IRequestHandler<VendorOrderDetailCreditToCsvCommand,
    VendorOrderDetailCreditToCsvResponse>
{
    private readonly IStorageProvider _storage;
    private readonly StorageOption _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="storage"></param>
    /// <param name="options"></param>
    public VendorOrderDetailCreditToCsvHandler(
        IStorageProvider storage,
        IOptions<StorageOption> options
    )
    {
        _storage = storage;
        _options = options.Value;
    }

    /// <inheritdoc />
    public Task<VendorOrderDetailCreditToCsvResponse> Handle(VendorOrderDetailCreditToCsvCommand request,
        CancellationToken cancellationToken)
    {
        var headers = new List<string>()
            { "UPC", "ProductCode", "Name", "Credit Reason", "Quantity", "Cost", "Amount" };
        var path = Path.GetTempPath() + Guid.NewGuid() + ".csv";
        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine(string.Join(", ", headers));

            foreach (var item in request.DetailCredits)
            {
                var it = new List<string>()
                {
                    item.ProductUpc, 
                    item.ProductCode,
                    item.ProductName, 
                    item.CreditReason.ToString(),
                    item.CreditQuantity.ToString(CultureInfo.CurrentCulture),
                    item.CreditCost.ToString(CultureInfo.CurrentCulture),
                    item.CreditAmount.ToString(CultureInfo.CurrentCulture),
                };
                writer.WriteLine(string.Join(", ", it));
            }
        }

        return Task.FromResult(new VendorOrderDetailCreditToCsvResponse() { Data = path });
    }
}