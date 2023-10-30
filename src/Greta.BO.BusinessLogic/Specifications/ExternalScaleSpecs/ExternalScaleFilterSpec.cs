using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.ExternalScaleSpecs;

/// <summary>
/// ExternalScale Filter Specification
/// </summary>
public sealed class ExternalScaleFilterSpec : Specification<ExternalScale>
{
    /// <inheritdoc />
    public ExternalScaleFilterSpec(ExternalScaleSearchModel filter)
    {
        Query.Include(x => x.Store);
        Query.Include(x => x.SyncDevice);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Ip, $"%{filter.Search}%");
            Query.Search(c => c.Port, $"%{filter.Search}%");
            Query.Search(c => c.SyncDevice.Name, $"%{filter.Search}%");
        }

        if (!string.IsNullOrEmpty(filter.Ip))
            Query.Where(c => c.Ip.Contains(filter.Ip));
        if (filter.ScaleBrandId >= 0)
            Query.Where(c => ((int)c.ExternalScaleType) == filter.ScaleBrandId);
        if (filter.StoreId > 0)
            Query.Where(c => c.StoreId == filter.StoreId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<ExternalScale>>)OrderExpressions).Add(new OrderExpressionInfo<ExternalScale>(
            splited[0] switch
            {
                "ip" => f => f.Ip,
                "port" => f => f.Port,
                "storeId" => f => f.StoreId,
                _ => f => f.Ip
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}