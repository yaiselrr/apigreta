#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text;
using AutoMapper;
using Greta.Sdk.Core.Abstractions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Extensions;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters
{
    public interface IModelExporter<T, T1> : IIntrospector<T>
        where T: class, IBase
        where T1: class
    {
        Task<string> Process(List<string>? columns, long? storeId);
    }

    public abstract class BaseExport<T, T1> : Introspector<T>, IModelExporter<T, T1>
        where T : class, IBase
        where T1: class
    {
        private readonly ILogger _logger;
        protected readonly IMapper _mapper;

        public BaseExport(ILogger logger, IMapper mapper, INotifier notifier) : 
            base(logger, nameof(T), notifier)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public abstract Task<string> Process(List<string>? columns, long? storeId);


        protected async Task<string> Build(IQueryable filteredResult, List<string>? columns)
        {
            var export = new StringBuilder();
            export.Append(string.Join(",", columns) + "\r\n");
            foreach (var item in filteredResult)
            {
                _currentRow++;
                try
                {
                    var content = "";
                    foreach (var column in columns!)
                    {
                        // retrieve from data
                        var val = item.GetType().GetProperty(column)?.GetValue(item);
                        if (val == null)
                        {
                            content += content.Length == 0 ? "" : ",";
                        }
                        else
                        {
                            content += content.Length == 0 ? $"{StringToCSVCell(val.ToString())}" : $",{StringToCSVCell(val.ToString())}";
                        }
                    }
                    export.Append(content + "\r\n");
                    _insertedRows++;
                    _processedRows++;
                    Notify();
                }
                catch(Exception ex)
                {
                    _failedRows++;
                    _errors.Add($"line : {_currentRow} - {ex.Message}");
                    var errorMsg = BuildMessage();
                    errorMsg.ErrorMessage = $"Error: line : {_currentRow} - {ex.Message}";
                    NotifyError(errorMsg);
                }
            }
            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            NotifyUpdate(msg);

            return await Task.FromResult(export.ToString());
        }
        
        /// <summary>
        /// Turn a string into a CSV cell output
        /// </summary>
        /// <param name="str">String to output</param>
        /// <returns>The CSV cell formatted string</returns>
        public static string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }
    }
}
