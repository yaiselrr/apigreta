using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.DeviceSpecs;

/// <summary>
/// Device Filter Specification
/// </summary>
public sealed class DeviceFilterSpec : Specification<Device>
{
    /// <inheritdoc />
    public DeviceFilterSpec(DeviceSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");

        if (!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        if (filter.StoreId != -1) 
            Query.Where(x => x.StoreId == filter.StoreId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Device>>)OrderExpressions).Add(new OrderExpressionInfo<Device>(
            splited[0] switch
            {
                "connected" => f => f.SignalRConnectionId,
                "brokerAlive" => f => f.BrokerAlive,
                "workerAlive" => f => f.WorkerAlive,
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}