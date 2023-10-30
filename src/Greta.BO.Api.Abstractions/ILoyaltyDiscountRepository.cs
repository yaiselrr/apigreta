using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface ILoyaltyDiscountRepository: IOperationBase<long, string, LoyaltyDiscount>
    {
    }
}