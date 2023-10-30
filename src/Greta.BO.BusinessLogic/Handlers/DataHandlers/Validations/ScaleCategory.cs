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
    public class ScaleCategoryValidationDepartmentId : ImportValidation<ScaleCategory>
    {
        private readonly INotifier _notifier;
        public ScaleCategoryValidationDepartmentId(INotifier notifier) => _notifier = notifier;
        public override async Task<(object?, bool)> Validate(ScaleCategory param, 
            Func<ScaleCategory, Task<(object?, bool)>> next, params object[] args)
        {
            var dpId = param.DepartmentId > 0;
            if(!dpId)
            {
                var msg = args.FirstOrDefault().ToHandlerMessage();
                msg.Message = "DepartmentId value is required";
                await _notifier.NotifyErrorAsync(msg);
                return (msg, false);
            }
            return await next(param);
        }
    }

    public class ScaleCategoryValidationName : ImportValidation<ScaleCategory>
    {
        private readonly INotifier _notifier;
        public ScaleCategoryValidationName(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(ScaleCategory parameter, 
            Func<ScaleCategory, Task<(object?, bool)>> next, params object[] args)
        {
            if (!string.IsNullOrEmpty(parameter.Name))
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "ScaleCategory Name value is required";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }

    public class ScaleCategoryValidationCategoryId : ImportValidation<ScaleCategory>
    {
        private readonly INotifier _notifier;
        public ScaleCategoryValidationCategoryId(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(ScaleCategory parameter, 
            Func<ScaleCategory, Task<(object?, bool)>> next, params object[] args)
        {
            if (parameter.CategoryId > 0)
                return await next(parameter);
            var errorMsg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            errorMsg.ErrorMessage = "CategoryId value is required";
            await _notifier.NotifyErrorAsync(errorMsg);
            return (errorMsg, false);
        }
    }

    public class ScaleCategoryValidationExistsDepartment : ImportValidation<ScaleCategory>
    {
        private readonly INotifier _notifier;
        private readonly IDepartmentService _dptService;
        public ScaleCategoryValidationExistsDepartment(INotifier notifier, IDepartmentService dptService)
        {
            _notifier = notifier;
            _dptService = dptService;
        }

        public override async Task<(object?, bool)> Validate(ScaleCategory param,
            Func<ScaleCategory, Task<(object?, bool)>> next, params object[] args)
        {
            var dptId = (await _dptService.GetIdFromDepartmentId((int)param.DepartmentId) > 0);
            if (dptId)
                return await next(param);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.Message = "Department was not found";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }
}
