using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;

namespace Greta.BO.Api.Sqlserver.Repository
{
    /// <inheritdoc cref="Greta.BO.Api.Abstractions.ICsvMappingRepository" />
    public class CsvMappingRepository : OperationBase<long, string, CSVMapping>, ICsvMappingRepository
    {
        /// <inheritdoc />
        public CsvMappingRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }
    }
}