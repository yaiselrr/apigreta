using System;
using Greta.Sdk.Core.Abstractions;
using Greta.Sdk.EFCore.Operations;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public class SynchroBaseService<TRepository, TEntity> :BaseService<TRepository, TEntity>
        where TEntity : class, IEntityBase<long, string>
        where TRepository : IOperationBase<long, string, TEntity>
    {
        public SynchroBaseService(TRepository repository, ILogger logger, Func<TEntity, string> converter = null) : base(repository, logger, converter)
        {
        }

        public SynchroBaseService(TRepository repository, ILogger logger, ISynchroService synchroService, Func<TEntity, string> converter = null) : base(repository, logger, synchroService, converter)
        {
        }
    }
}