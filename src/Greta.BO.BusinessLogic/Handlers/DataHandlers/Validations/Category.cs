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
    public class CategoryValidationName :  ImportValidation<Category>
    {
        private readonly INotifier _notifier;

        public CategoryValidationName(INotifier notifier)
        {
            _notifier = notifier;
        }

        public override async Task<(object?, bool)> Validate(Category param, Func<Category,
            Task<(object?, bool)>> next, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
                return await next(param);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.Message = "Name value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class CategoryValidationCategoryId : ImportValidation<Category>
    {
        private readonly INotifier _notifier;
        public CategoryValidationCategoryId(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(Category parameter,
            Func<Category, Task<(object?, bool)>> next, params object[] args)
        {

            if (parameter.CategoryId == 0)
                return await next(parameter);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.Message = "CategoryId value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class CategoryValidationDepartmentId : ImportValidation<Category>
    {
        private readonly INotifier _notifier;
        public CategoryValidationDepartmentId(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(Category param,
            Func<Category, Task<(object?, bool)>> next, params object[] args)
        {
            if (param.DepartmentId == 0)
                return await next(param);

            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.Message = "DepartmentId value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class CategoryValidationDepartment : ImportValidation<Category>
    {
        private readonly INotifier _notifier;
        private readonly IDepartmentService _dptService; 
        public CategoryValidationDepartment(INotifier notifier, IDepartmentService dptService) 
        {
            _notifier = notifier;
            _dptService = dptService;
        }

        public override async Task<(object?, bool)> Validate(Category param, 
            Func<Category, Task<(object?, bool)>> next, params object[] args)
        {
            var dpt = await _dptService.GetIdFromDepartmentId((int)param.DepartmentId);
            if(dpt == 0)
            {
                var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
                msg.Message = "Department not found";
                await _notifier.NotifyErrorAsync(msg);
                return (msg, false);
            }
            return await next(param);
        }
    }
}
