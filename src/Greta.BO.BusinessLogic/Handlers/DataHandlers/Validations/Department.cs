#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations
{
    public class DepartmentValidationId : ImportValidation<Department>
    {
        private readonly INotifier _notifier;
        public DepartmentValidationId(INotifier notifier) => _notifier = notifier;
        public override async Task<(object?, bool)> Validate(Department param, 
            Func<Department, Task<(object?, bool)>> next, params object[] args)
        {
            if(param.DepartmentId == 0)
            {
                var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
                msg.Message = "DepartmentId value is required";
                await _notifier.NotifyErrorAsync(msg);
                return (msg, false);
            }
            return await next(param);
        }
    }
}
