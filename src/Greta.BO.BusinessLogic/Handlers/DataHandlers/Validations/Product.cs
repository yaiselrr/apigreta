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
    public class ProductValidationCategoryId : ImportValidation<Product>
    {
        private readonly INotifier _notifier;
        public ProductValidationCategoryId(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(Product param, 
            Func<Product, Task<(object?, bool)>> next, params object[] args)
        {
            if (param.CategoryId > 0)
                return await next(param);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.Message = $"CategoryId value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class ProductValidationDepartmentId : ImportValidation<Product>
    {
        private readonly INotifier _notifier;
        public ProductValidationDepartmentId(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(Product parameter, 
            Func<Product, Task<(object?, bool)>> next, params object[] args)
        {
            if (parameter.DepartmentId != 0)
                return await next(parameter);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.ErrorMessage = $"CategoryId value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class ProductValidationName : ImportValidation<Product>
    {
        private readonly INotifier _notifier;
        public ProductValidationName(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(Product parameter,
            Func<Product, Task<(object?, bool)>> next, params object[] args)
        {
            if (!string.IsNullOrEmpty(parameter.Name))
                return await next(parameter);
            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.ErrorMessage = "Name value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class ProductValidationUPCLength : ImportValidation<Product>
    {
        private readonly INotifier _notifier;
        public ProductValidationUPCLength(INotifier notifier) => _notifier = notifier;

        public override async Task<(object?, bool)> Validate(Product parameter,
            Func<Product, Task<(object?, bool)>> next, params object[] args)
        {
            if (parameter.UPC.Length <= 5)
                return await next(parameter);

            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.ErrorMessage = "UPC max length is 5";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }

    public class ProductValidationUPC : ImportValidation<Product>
    {
        private readonly INotifier _notifier;
        public ProductValidationUPC(INotifier notifier) => _notifier = notifier;

        public override async Task<(object? Msg, bool IsValid)> Validate(Product parameter,
            Func<Product, Task<(object?, bool)>> next, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(parameter.UPC))
                return await next(parameter);

            var msg = args.FirstOrDefault(x => x is HandlerMessage).ToHandlerMessage();
            msg.ErrorMessage = "UPC value is required";
            await _notifier.NotifyErrorAsync(msg);
            return (msg, false);
        }
    }
}
