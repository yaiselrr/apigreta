using System;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Newtonsoft.Json;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class SynchroDetailRepository : OperationBase<long, string, SynchroDetail>, ISynchroDetailRepository
    {
        public SynchroDetailRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        public async Task<bool> CreateSynchroDetail<TData>(long storeId, TData data, SynchroType type, Func<TData, object>? converter = null)
        {
            var obj = converter == null ? data : converter(data);

            var toStore = new SynchroDetail
            {
                Entity = obj!.GetType().Name,
                Type = type,
                //Data = JsonSerializer.Serialize(data, opts),
                Data = DefaultConverter(obj),
                SynchroId = storeId
            };
            var nresult = await base.CreateAsync(toStore);
            return nresult > 0L;
        }

        public static string DefaultConverter<TData>(TData data) => JsonConvert.SerializeObject(data, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
    }
}