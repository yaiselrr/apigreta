using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Abstractions
{
    /// <inheritdoc />
    public interface ICsvMappingRepository : IOperationBase<long, string, CSVMapping>
    {
    }
}