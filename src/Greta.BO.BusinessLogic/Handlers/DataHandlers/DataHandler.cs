#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;
using Greta.BO.BusinessLogic.Models.Dto.Column;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    /// Data handler for Import/Export operations
    /// <summary>
    public class DataHandler
    {
         private readonly ILogger _logger;

        /// <summary>
        ///   handle send notification message
        /// </summary>
        public event UINotificationHandler? Notify;

        /// <summary>
        /// returns a new <see cref="DataHandler"/> instance
        /// </summary>
        public DataHandler(ILogger logger)
        {
            _logger = logger;
            Model = "";
        } 

        /// <summary>
        /// returns a new <see cref="DataHandler"/> instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="model"></param>
        public DataHandler(ILogger logger, string model)
        {
            _logger = logger;
            Model = model;
        }

        /// <summary>
        /// name of managed entity
        /// </summary>
        public string Model { get; private set;}
    }

    /// <summary>
    /// A simple delegate to be called by notification events
    /// </summary>
    public delegate void UINotificationHandler(object? sender, HandlerMessage message);
    public delegate void UINotificationHandler<TParameter>(object? sender, TParameter parameters);
}
