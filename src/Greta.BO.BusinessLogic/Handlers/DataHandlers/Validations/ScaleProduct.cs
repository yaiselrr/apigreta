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
    public class ScaleProductValidationUPC : ImportValidation<ScaleProduct>
    {
        private readonly INotifier _notifier;
        public ScaleProductValidationUPC(INotifier notifier)
        {
            _notifier = notifier;
        }

        public override async Task<(object?, bool)> Validate(ScaleProduct param, 
            Func<ScaleProduct, Task<(object?, bool)>> next, params object[] args)
        {
            var upcValid = string.IsNullOrWhiteSpace(param.UPC);
            if (!upcValid)
                return await next(param);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.ErrorMessage = "UPC value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class ScaleProductValidationUPCLength : ImportValidation<ScaleProduct>
    {
        private readonly INotifier _notifier;
        public ScaleProductValidationUPCLength(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(ScaleProduct parameter, 
            Func<ScaleProduct, Task<(object?, bool)>> next, params object[] args)
        {
            if (parameter.UPC.Length <= 5)
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "UPC could be greater than 5";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }

    public class ScaleProductValidationCategoryId : ImportValidation<ScaleProduct>
    {
        private readonly INotifier _notifier;
        public ScaleProductValidationCategoryId(INotifier notifier) => _notifier = notifier;
        public override async Task<(object?, bool)> Validate(ScaleProduct parameter, 
            Func<ScaleProduct, Task<(object?, bool)>> next, params object[] args)
        {
            if (parameter.CategoryId > 0)
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "CategoryId value is required";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }

    public class ScaleProductValidationDepartmentId : ImportValidation<ScaleProduct>
    {
        private readonly INotifier _notifier;
        public ScaleProductValidationDepartmentId(INotifier notifier) => _notifier = notifier;
        public override async Task<(object?, bool)> Validate(ScaleProduct parameter, 
            Func<ScaleProduct, Task<(object?, bool)>> next, params object[] args)
        {
            if (parameter.DepartmentId > 0)
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "DepartmentId value is required";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }

    public class ScaleProductValidationName : ImportValidation<ScaleProduct>
    {
        private readonly INotifier _notifier;
        public ScaleProductValidationName(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(ScaleProduct parameter, 
            Func<ScaleProduct, Task<(object?, bool)>> next, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(parameter.Name))
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "Name value is required";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }
    
    public class ScaleProductValidationDescription : ImportValidation<ScaleProduct>
    {
        private readonly INotifier _notifier;
        public ScaleProductValidationDescription(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(ScaleProduct parameter, 
            Func<ScaleProduct, Task<(object?, bool)>> next, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(parameter.Description1))
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "Description1 value is required";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }
}
