using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    public interface IChangePriceReasonCodeRepository: IOperationBase<long, string, ChangePriceReasonCode>
    {
    }
}