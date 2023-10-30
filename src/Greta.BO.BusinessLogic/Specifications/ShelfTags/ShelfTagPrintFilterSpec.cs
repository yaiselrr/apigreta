using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Handlers.Command.Zpl;

namespace Greta.BO.BusinessLogic.Specifications.ShelfTags;

/// <summary>
/// ShelfTag print Filter Specification
/// </summary>
public sealed class ShelfTagPrintFilterSpec : Specification<ShelfTag, long>
{
    /// <inheritdoc />
    public ShelfTagPrintFilterSpec(ProcessShelfTagsCommand filter)
    {
        Query.Where(c => c.StoreId == filter.Model.StoreId);

        if (filter.Model.CategoryId != -1)
            Query.Where(c => c.CategoryId == filter.Model.CategoryId);

        if (filter.Model.DepartmentId != -1)
            Query.Where(c => c.DepartmentId == filter.Model.DepartmentId);

        if (filter.Model.BinLocationId != -1)
            Query.Where(c => c.BinLocationId == filter.Model.BinLocationId);

        if (filter.Model.VendorId != -1)
            Query.Where(c => c.VendorId == filter.Model.VendorId);

        Query.Select(tag => tag.Id);
    }
}