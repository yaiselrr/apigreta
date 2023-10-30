using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.AnimalSpecs;

/// <summary>
/// Animal Filter Specification
/// </summary>
public sealed class AnimalFilterSpec : Specification<Animal>
{
    /// <inheritdoc />
    public AnimalFilterSpec(AnimalSearchModel filter)
    {
        bool tagRead = false;
        Query.Include(x => x.Rancher);
        Query.Include(x => x.Breed);

        if (!string.IsNullOrEmpty(filter.Search))
        {//0000010004895      0000040000044
            if (filter.Search is { Length: 13 })
            {
                //try to parse if this is a tag number from a barcode
                var stringRancher = filter.Search[..6];
                var stringTagNumber = filter.Search[7..];
                if (long.TryParse(stringRancher, out var rancherId) &&
                    int.TryParse(stringTagNumber, out var tagNumber))
                {
                    Query.Where(c => c.RancherId == rancherId && c.Tag == tagNumber.ToString());
                    tagRead = true;
                }
            }
            else
            {
                Query.Where(c => c.Tag.Contains(filter.Search));
            }
        }

        if (!tagRead)
        {
            Query.Where(c => c.StoreId.Equals(filter.StoreId));
            if (!string.IsNullOrEmpty(filter.Tag))
                Query.Where(c => c.Tag.Contains(filter.Tag));
            if (filter.RancherId > 0)
                Query.Where(c => c.RancherId == filter.RancherId);
            if (filter.BreedId > 0)
                Query.Where(c => c.BreedId == filter.BreedId);
            if (filter.DateReceived != null)
                Query.Where(c => c.DateReceived == filter.DateReceived.Value.ToUniversalTime().Date);
            if (filter.DateSlaughtered != null)
                Query.Where(c => c.DateSlaughtered == filter.DateSlaughtered.Value.ToUniversalTime().Date);
        }

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Animal>>)OrderExpressions).Add(new OrderExpressionInfo<Animal>(
            splited[0] switch
            {
                "tag" => f => f.Tag,
                "rancherId" => f => f.RancherId,
                "breedId" => f => f.BreedId,
                "dateReceived" => f => f.DateReceived,
                "dateSlaughtered" => f => f.DateSlaughtered,
                _ => f => f.DateReceived
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}