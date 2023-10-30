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
    public class FamilyValidationName : ImportValidation<Family>
    {
        private readonly INotifier _notifier;
        public FamilyValidationName(INotifier notifier) => _notifier = notifier;
        public override async Task<(object?, bool)> Validate(Family parameter, 
            Func<Family, Task<(object?, bool)>> next, params object[] args)
        {
            if(string.IsNullOrWhiteSpace(parameter.Name))
            {
                var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
                errorMsg.Message = "Name value is required";
                _notifier.NotifyError(errorMsg);
                return (errorMsg, false);
            }
            return await next(parameter);
        }
    }
}
