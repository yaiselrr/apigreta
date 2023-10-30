using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using LanguageExt.ClassInstances;

namespace Greta.BO.BusinessLogic.Specifications.FamilySpecs;
/// <summary>
/// Product filter using a family id
/// </summary>
public sealed class ProductByFamilyIdSpec:Specification<Product, Product>
{
    public ProductByFamilyIdSpec( 
        string searchString,
        string sortString,
        long familyId)
    {
        var splited = string.IsNullOrEmpty(sortString) ? new[] { "", "" } : sortString.Split("_");

        Query.Where(x => x.FamilyId == familyId);

        if (!string.IsNullOrEmpty(searchString))
            Query.Where(c =>
                c.Name.Contains(searchString) ||
                c.UPC.Contains(searchString)
            );
        
        Expression<Func<Product, object>> orderExpression = splited[0] switch
        {
            "productName" => f => f.Name,
            _ => f => f.UPC
        };

        switch (splited[1])
        {
            case "desc":
                Query.OrderByDescending(orderExpression);
                break;
            default:
                Query.OrderBy(orderExpression);
                break;
        }
        
        Query.Select(x => new Product()
        {
            Id = x.Id,
            UPC = x.UPC,
            Name = x.Name
        });
    }
}