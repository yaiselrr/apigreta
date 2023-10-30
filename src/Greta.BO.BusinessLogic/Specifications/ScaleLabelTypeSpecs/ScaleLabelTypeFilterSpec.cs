using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.ScaleLabelTypeSpecs;

/// <summary>
/// ScaleLabelType Filter Specification
/// </summary>
public sealed class ScaleLabelTypeFilterSpec : Specification<ScaleLabelType, ScaleLabelType>
{
    /// <inheritdoc />
    public ScaleLabelTypeFilterSpec(ScaleLabelTypeSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.Name, $"%{filter.Search}%");

        if (!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));

        switch (filter.Type)
        {
            case LabelDesignMode.ShelTag:
                Query.Where(c => c.ScaleType == ScaleType.SHELFTAG);
                break;
            case LabelDesignMode.Animal:
                Query.Where(c => c.ScaleType == ScaleType.ANIMAL);
                break;
            case LabelDesignMode.Label:
            default:
                Query.Where(c => c.ScaleType == ScaleType.GRETALABEL || c.ScaleType == ScaleType.EXTERNAL);
                break;
        }

        Query.Select(x => new ScaleLabelType()
        {
            Id = x.Id,
            State = x.State,
            Name = x.Name,
            LabelId = x.LabelId,
            ScaleType = x.ScaleType
        });

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<ScaleLabelType>>)OrderExpressions).Add(new OrderExpressionInfo<ScaleLabelType>(
            splited[0] switch
            {
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}