#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    /// Base message content to be send over handlers
    /// </summary>
    public class HandlerMessage
    {
        protected Dictionary<string, MessageLevel> _messages = new Dictionary<string, MessageLevel>
        {
            {"Initializing", MessageLevel.Information }
        };
        protected string _model = "";
        protected Exception? _innerException = null;

        /// <summary>
        /// returns a new <see cref="HandlerMessage"/> instance
        /// </summary>
        public HandlerMessage()
        {
            CreationDate = DateTime.UtcNow;
            Model = "";
            Message = "";
            CurrentRow = 0;
            MessageLevel = MessageLevel.Information;
            Stage = Stage.Initialiazing;
        }

        public HandlerMessage(string model, int currenRow, string message, int? totalRows = null)
        {
            CreationDate = DateTime.UtcNow;
            Model = model;
            CurrentRow = currenRow;
            Message = message;
            MessageLevel = MessageLevel.Information;
            Stage = Stage.Initialiazing;
            if(totalRows.HasValue)
                TotalRows = totalRows.Value;

        }

        public HandlerMessage(string model, int currentRow, string message, MessageLevel level,
            Exception? innerException = null)
        {
            CreationDate = DateTime.UtcNow;
            Model = model;
            CurrentRow = currentRow;
            Message = message;
            Stage = Stage.Initialiazing;
            MessageLevel = level;
            if(innerException != null)
            {
                InnerException = innerException;
                MessageLevel = MessageLevel.Error;
            }
        }

        public HandlerMessage(string model, int currentRow, int insertedRows, int updatedRows,
            int failedRows, int processedRows, int totalRows, List<string> messages, List<string> errors)
        {
            Model = model;
            CurrentRow = currentRow;
            InsertedRows = insertedRows;
            UpdatedRows = updatedRows;
            ProcessedRows = processedRows;
            FailedRows = failedRows;
            TotalRows = totalRows;
            if(messages.Any())
                messages.ForEach(x =>
                {
                    if (!_messages.Keys.Contains(x)) 
                        _messages.Add(x, MessageLevel.Information);
                });  
            if(errors.Any())
                errors.ForEach(x =>
                {
                    if (!_messages.Keys.Contains(x))
                        _messages.Add(x, MessageLevel.Error);
                });
        }

        /// <summary>
        /// stage name in with importatin/exportation process is
        /// </summary>
        public Stage Stage { get; set; } = Stage.Initialiazing;

        /// <summary>
        /// model that's been processed
        /// </summary>
        public string Model
        {
            get { return _model; }
            set
            {
                if (string.IsNullOrWhiteSpace(_model))
                    _model = value;
            }
        }

        /// <summary></summary>
        public int CurrentRow { get; set; } = 0;

        /// <summary>
        /// total rows number
        /// </summary>
        public int TotalRows { get; set; } = 0;

        /// <summary></summary>
        public int InsertedRows { get; set; } = 0;

        /// <summary></summary>
        public int UpdatedRows { get; set; } = 0;

        /// <summary></summary>
        public int FailedRows { get; set; } = 0;

        /// <summary> </summary>
        public int ProcessedRows { get; set; } = 0;

        /// <summary>
        /// date of message launch
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Message content to display
        /// </summary>
        public string Message
        {
            get
            {
                return _messages.Keys.Last();
            }
            set 
            {
                if(!_messages.ContainsKey(value))
                    _messages.Add(value, this.MessageLevel);
            }
        }

        public string ErrorMessage
        {
            get => _messages.Where(x => x.Value == MessageLevel.Error)
                            .Select((x => x.Key)).Last();
            set => _messages.Add(value, MessageLevel.Error);
        }

        /// <summary>
        /// returns only those messages that are Errors
        /// </summary>
        public List<string> Errors => _messages.Where(x => x.Value == MessageLevel.Error).Select(x => x.Key).ToList();

        /// <summary>
        /// returns only inormation messages
        /// </summary>
        public List<string> Informations =>
            _messages.Where(x => x.Value == MessageLevel.Information).Select(x => x.Key).ToList();

        /// <summary>
        /// return only Debug messages
        /// </summary>
        public List<string> Debugs => _messages.Where(x => x.Value == MessageLevel.Debug).Select(x => x.Key).ToList();

        /// <summary>
        /// return only Warnings
        /// </summary>
        public List<string> Warnings =>
            _messages.Where(x => x.Value == MessageLevel.Warning).Select(x => x.Key).ToList();

        /// <summary>
        /// return all mmesages
        /// </summary>
        public List<string> AllMessages => _messages.Select(x => $"{x.Value} - {x.Key}").ToList();

        /// <summary>
        /// level message, commonly it will be used the Debug level so system could track
        /// the import/export process. If Information level it will not only keep the message
        /// history but means that the front should be updated
        /// </summary>
        public MessageLevel MessageLevel { get; set; } = MessageLevel.Information;

        /// <summary>
        /// Exception message, this will be used in case of an error message and 
        /// final front or consummer should determ what to do with it.
        /// </summary>
        public Exception? InnerException
        {
            get { return _innerException; }
            set
            {
                if (value != null)
                {
                    this.MessageLevel = MessageLevel.Error;
                    this.Message = value!.Message;
                    _innerException = value;
                }
            }
        }

        /// <summary>
        /// returns a new instance of <see cref="HandlerMessage"/>, missed data will be taken from
        /// the previous message, this reduce the amount of code to notify operation.
        /// </summary>
        public HandlerMessage New(string message, string? model = null, int? currentRow = null, Stage? stage = null,
            int? totalRows = null, MessageLevel? level = null, Exception? ex = null)
        {
            var result = new HandlerMessage
            {
                Message = message,
                CreationDate = DateTime.UtcNow
            };

            if (stage.HasValue) Stage = stage.Value;
            if (!string.IsNullOrEmpty(model)) Model = model;
            if (currentRow.HasValue) CurrentRow = currentRow.Value;
            if (totalRows.HasValue) TotalRows = totalRows.Value;
            if (level.HasValue) MessageLevel = (MessageLevel)level;
            result._messages = this._messages;
            if (ex != null)
            {
                this.MessageLevel = MessageLevel.Error;
                InnerException = ex;
            }

            return this;
        }
        
        /// <summary>
        /// returns a new <see cref="HandlerMessage"/> instance, is suppose to simplify the instanciation
        /// process.
        /// </summary>
        public static HandlerMessage New(string message, int currentRow = 0, Stage? stage = null,
            string? model = null, int? totalRows = null, MessageLevel? level = MessageLevel.Information,
            Exception? exception = null)
        {
            var result = new HandlerMessage
            {
                Model = !string.IsNullOrEmpty(model) ? model: "",
                CurrentRow = currentRow,
                CreationDate = DateTime.UtcNow,
                MessageLevel = level != null ? level.Value : MessageLevel.Information
            };

            if (!string.IsNullOrEmpty(message))
            {
                result.Message = message;
            }
            else
            {
                if (exception != null)
                {
                    result.Message = exception.Message;
                }
            }

            if (exception != null)
            {
                result.MessageLevel = MessageLevel.Error;
                result.InnerException = exception;
            }

            if (totalRows.HasValue && totalRows.Value > 0 && totalRows.Value > currentRow)
            {
                totalRows = totalRows.Value;
            }

            if (stage.HasValue)
            {
                result.Stage = stage.Value;
            }

            return result;
        }

        /// <summary>
        /// returns a new <see cref="HandlerMessage"/> instance is suppose to simplify the instantiation
        /// process
        /// </summary>
        public static HandlerMessage New(string model, Exception exception, Stage? stage = null)
        {
            var result = new HandlerMessage
            {
                Model = model,
                CurrentRow = 0,
                CreationDate = DateTime.UtcNow,
                MessageLevel = MessageLevel.Error,
                Message = exception.Message,
                InnerException = exception
            };

            if (stage.HasValue)
            {
                result.Stage = stage.Value;
            }

            return result;
        }

        /// <summary>
                /// creates a new <see cref="HandlerMessage"/> using a message as a source
                /// it only changes the message. depending on isError choose which message type
                /// to generate Information or Error
                /// </summary>
                /// <param name="parent"></param>
                /// <param name="source"></param>
                /// <param name="isError"></param>
                /// <returns></returns>
        public static HandlerMessage From(HandlerMessage parent, string message, bool isError = false)
        {
            if(isError)
                parent.ErrorMessage = message;
            else
                parent.Message = message;
            return parent;
        }

        /// <summary>
        /// creates a new <see cref="HandlerMessage"/> using a message as a source
        /// it only changes the message. depending on isError choose which message type
        /// to generate Information or Error
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="source"></param>
        /// <param name="isError"></param>
        /// <returns></returns>
        public static HandlerMessage From(HandlerMessage parent, HandlerMessage source, bool isError = false)
        {
            if(isError)
                parent.ErrorMessage = source.ErrorMessage;
            else
                parent.Message = source.Message;
            return parent;
        }
    }
}