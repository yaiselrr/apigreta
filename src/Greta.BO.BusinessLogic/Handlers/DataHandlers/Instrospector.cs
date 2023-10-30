#nullable enable
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Greta.Sdk.Core.Abstractions;
using Greta.BO.BusinessLogic.Models.Dto.Column;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    ///  Define the introspector methods. THis instrospector is used to gather
    ///  data from existing entities and mostly match them with provided mappings
    ///  of provided data.
    /// </summary>
    public interface IIntrospector<T>
        where T: class, IBase
    {
        /// <summary>
        /// name of processed model
        /// </summary>
        string Model { get; set; }
        
        /// <summary>
        /// returns the list of base entity properties
        /// </summary>
        /// <returns></returns>
        List<string> GetBaseProperties();
        List<string> GetAllBaseProperties();
        List<string> GetColumnNames();
        List<string> GetAllColumnsNames();
        IIntrospector<T> SetModel(string model);
        string ConcatColumns(List<string> columns);
    }

    /// <inheritdoc/>
    public class Introspector<T> : IIntrospector<T>
        where T: class, IBase
    {
        private readonly ILogger? _logger = null;
        
        protected readonly INotifier? _notifier = null;
        
        // private members
        protected int _totalRows = 0;
        protected int _currentRow = 0;
        protected int _insertedRows = 0;
        protected int _updatedRows = 0;
        protected int _processedRows = 0;
        protected int _failedRows = 0;
        protected List<string> _messages = new();
        protected List<string> _errors = new();

        public Introspector()
        {
            Model = nameof(T);
        }

        /// <summary></summary>
        public Introspector(INotifier notifier)
        {
            Model = nameof(T);
            _notifier = notifier;
        }

        /// <summary>
        /// returns a new <see cref="Introspector{T}"/> instance
        /// </summary>
        public Introspector(ILogger logger, INotifier notifier)
        {
            Model = typeof(T).GetType().Name;
            _logger = logger;
            _notifier = notifier;
        } 

        /// <summary>
        /// returns a new <see cref="Introspector{T}"/> instance
        /// </summary>
        public Introspector(ILogger logger, string model, INotifier notifier)
        {
            Model = model;
            _logger= logger;
            _notifier = notifier;
        }

        /// <summary></summary>
        public static Introspector<T> New(ILogger logger, INotifier notifier) => new Introspector<T>(logger, notifier);

        /// <summary>
        /// returns a new <see cref="Introspector{T}"/> instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="model"></param>
        /// <param name="notifier"></param>
        /// <returns></returns>
        public static Introspector<T> New(ILogger logger, string model, INotifier notifier) => new Introspector<T>(logger, model, notifier);

        /// <summary>
        /// returns a new <see cref="Introspector{T}"/> instance
        /// </summary>
        /// <param name="notifier"></param>
        /// <returns></returns>
        public static Introspector<T> New(INotifier notifier) => new Introspector<T>(notifier);

        /// <inheritdoc/>
        public string Model { get; set; }
        
        public IIntrospector<T> SetModel(string model)
        {
            Model = model;
            return this;
        }

        public void NotifyUpdate(HandlerMessage msg)
        {
            _logger?.LogInformation($"Update notification received: {msg.Message}");
            _notifier?.NotifyUpdate(msg);
        }

        public void NotifyError(HandlerMessage msg)
        {
            _logger?.LogError($"Error message received: {msg.Message}");
            _notifier?.NotifyError(msg);
        }
        
        /// <inheritdoc/>
        public List<string> GetBaseProperties() =>
            typeof(T).GetBaseProperties(includeId: false, ex => 
            {
                _logger?.LogError(ex.Message);
                _notifier?.NotifyError(HandlerMessage.New(Model, ex));
            });
        
        /// <inheritdoc/>
        public List<string> GetAllBaseProperties() =>
            typeof(T).GetBaseProperties(includeId: true, ex =>
            {
                _logger?.LogError(ex.Message);
                var msg = HandlerMessage.New(Model, ex);
                _notifier?.NotifyError(msg);
            });

        /// <summary>
        /// return a message with current information
        /// </summary>
        public HandlerMessage BuildMessage()
        {
            return new HandlerMessage(Model, _currentRow, _insertedRows, _updatedRows, _failedRows, _processedRows,
                _totalRows, _messages, _errors);
        }

        protected void Notify(string message, bool isError = false)
        {
            if(!isError)
            {
                _messages.Add(message);
                NotifyUpdate(BuildMessage());
            }
            else
            {
                _errors.Add(message);
                NotifyError(BuildMessage());
            }
        }
        
        protected void Notify(HandlerMessage msg)
        {
            if(msg.MessageLevel == MessageLevel.Information)
                NotifyUpdate(msg);
            else
                NotifyError(msg);
        }

        protected void Notify() =>
            Notify(BuildMessage());

        /// <inheritdoc/>
        public List<string> GetColumnNames() =>
            typeof(T).GetAllColumnName(ex =>
            {
                _logger?.LogError(ex.Message);
                var msg = HandlerMessage.New(Model, ex);
                _notifier?.NotifyError(msg);
            });
        
        /// <inheritdoc/>
        public List<string> GetAllColumnsNames() =>
            typeof(T).GetAllColumnName(ex =>
            {
                _logger?.LogError(ex.Message);
                var msg = HandlerMessage.New(Model, ex);
                _notifier?.NotifyError(msg);
            });

        public IQueryable<ColumnNameModel> FilterqueryBuilderAsync(ColumnNameModel filter,
           string searchstring,
           string[] splited,
           DbSet<ColumnNameModel> query)
        {
            IQueryable<ColumnNameModel> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name),
                    c => c.Name.Contains(filter.Name));

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e);

            return query1;
        }


        public string ConcatColumns(List<string> columns)
        {
            string selectedStatment = "";
            string result = string.Join(',', columns);
            // foreach(var item in columns)
            // {
            //     selectedStatment = selectedStatment + item + ",";
            // }
            result = "new ( " + result + ")";
            return result;
        }
    }
        
}
