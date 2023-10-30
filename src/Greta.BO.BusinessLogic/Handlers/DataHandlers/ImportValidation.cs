#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    /// Define the data validator used in the importing process. Has an abstract
    /// for that is implemented on specific validators, this class will be used
    /// by the <see cref="ValidationPipeline{TParameter}"/> to execute validations
    /// </summary>
    /// <typeparam name="TParameter">Entity type to validate</typeparam>
    public abstract class ImportValidation<TParameter> where TParameter: class
    {
        /// <summary>
        /// execute the validation process for each entities.
        /// </summary>
        /// <param name="parameter"><see cref="TParameter"/> instance to validate</param>
        /// <param name="next">the next validation in the pipeline</param>
        /// <param name="args">[optional] parameters to be send to validators</param>
        /// <returns></returns>
        public abstract Task<(object? Msg, bool IsValid)> Validate(TParameter parameter, 
            Func<TParameter, Task<(object?, bool)>> next, params object[] args);
    }

    /// <summary>
    /// The validation pipeline, uses an Activations mechanism for the <see cref="ImportValidation{TParameter}"/>
    /// classes specified. The pipeline has been implemented as a Chain Of Responsibility, that
    /// breaks when one of the validations fails.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IValidationPipeline<TParameter> where TParameter : class, IBase
    {
        /// <summary>
        /// Adds a new validation type that inherit from <see cref="ImportValidation{TParameter}"/> that
        /// will be used int the validation process
        /// </summary>
        /// <typeparam name="TValidation">validation type to included</typeparam>
        /// <returns></returns>
        IValidationPipeline<TParameter> Add<TValidation>()
            where TValidation : ImportValidation<TParameter>;

        /// <summary>
        /// executes the validation pipeline.
        /// </summary>
        /// <param name="parameter"><see cref="TParameter"/> instance to validate</param>
        /// <param name="args">parameters that may be used in the validators</param>
        /// <returns><see cref="Task{object?, bool}"/> with true if no validation fails</returns>
        Task<(object? Msg, bool IsValid)> Execute(TParameter parameter, params object[] args);
        
        /// <summary>
        /// returns the list of registered validations
        /// </summary>
        List<Type> Validations { get; }
    }

    /// <summary>
    /// <inheritdoc cref="IValidationPipeline{TParameter}"/>. Validations are saved as types,
    /// When executed it creates a new instance, and calls the <see cref="ImportValidation{TParameter}.Validate"/>
    /// method of the instance and passes to it the required parameters, including the next function
    /// that's actually a call to the next validate in the pipeline or a lambda empty function if
    /// the pipeline finish. It used a <see cref="INotifier"/> to send notifications about validation
    /// status.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public class ValidationPipeline<TParameter> : IValidationPipeline<TParameter> where TParameter : class, IBase
    {
        private List<Type> _validationTypes = new();
        private INotifier _notifier;

        /// <summary>
        /// returns a new <see cref="ValidationPipeline{TParameter}"/> instance
        /// </summary>
        /// <param name="notifier"></param>
        public ValidationPipeline(INotifier notifier) => _notifier = notifier;

        /// <summary>
        /// returns the list of registered validations
        /// </summary>
        public List<Type> Validations => _validationTypes;

        /// <inheritdoc cref="IValidationPipeline{TParameter}.Add{TValidation}"/>
        public IValidationPipeline<TParameter> Add<TValidation>() where TValidation : ImportValidation<TParameter>
        {
            _validationTypes.Add(typeof(TValidation));
            return this;
        }

        /// <summary>
        /// <see cref="IValidationPipeline{TParameter}.Execute(TParameter, object[])"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="baseMsg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<(object? Msg, bool IsValid)> Execute(TParameter parameter, params object[] args)
        {
            if (_validationTypes.Count == 0)
                return (null, true);

            var baseMsg = args.FirstOrDefault(x => x is HandlerMessage);

            var index = 0;
            Func<TParameter, Task<(object?, bool)>> func = null!;
            func = (param) =>
            {
                var type = _validationTypes[index];
                ImportValidation<TParameter> validation = null;
                try
                {
                    validation = (ImportValidation<TParameter>)Activator.CreateInstance(type, _notifier);
                }
                catch
                {
                    validation = (ImportValidation<TParameter>)Activator.CreateInstance(type);
                }
                index++;
                if (index == _validationTypes.Count)
                {
                    (object?, bool) result = (null, true);
                    func = (param) => Task.FromResult(result);
                }

                return validation!.Validate(param, func, baseMsg, args);
            };

            return await func(parameter).ConfigureAwait(false);
        }
    }

}
