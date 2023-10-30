using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers
{
    public class DepartmentImport : BaseImport<Department>
    {
        public DepartmentImport(ILogger<DepartmentImport> logger, IServiceProvider provider, 
            ILogger<Introspector<Department>> inLogger, INotifier notifier) 
            : base(logger, provider, inLogger, notifier)
        {
            
        }        

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var dptService = Provider.GetRequiredService<IDepartmentService>();
            List<string> errors = new();
            _currentRow = 0;
            _totalRows = 0;
            _insertedRows = 0;
            _updatedRows = 0;
            _failedRows = 0;
            var validations = new ValidationPipeline<Department>(_notifier)
                .Add<DepartmentValidationId>();
            var departments = mapping.MapToEntityList<Department>(
                    csv, 
                    null,
                    (hm) =>
                    {
                        if (hm.MessageLevel == MessageLevel.Information)
                        {
                            Notify(HandlerMessage.From(BuildMessage(), hm));
                        }
                        else
                        {
                            Notify(HandlerMessage.From(BuildMessage(), hm, true));
                        }
                    });
            _totalRows = departments.Count;
            Notify("Initializing");            
            for(var i = 0; i < departments.Count; i++)
            {
                _currentRow++;
                try
                {
                    var validation = await validations.Execute(departments[i], BuildMessage());
                    if (!validation.IsValid)
                    {
                        _failedRows++;
                        errors.Add(((HandlerMessage)validation.Msg!).Message);
                        NotifyError(BuildMessage());
                    }
                    else
                   {
                        var department = await dptService.GetByDepartment(departments[i].DepartmentId, false);
                        if(department == null)
                        {
                            departments[i].BackgroundColor ??= "#000000";
                            departments[i].ForegroundColor ??= "#FFFFFF";
                            await dptService.Post(departments[i]);
                            _insertedRows++;
                        }
                        else
                        {
                            departments[i].Id = department.Id;
                            departments[i].UserCreatorId = department.UserCreatorId;
                            await dptService.Put(department.Id, departments[i]);
                            _updatedRows++;
                        }
                   }
                }
                catch (System.Exception error)
                {
                    _failedRows++;
                    var msgEr = BuildMessage();
                    if (error.InnerException != null &&
                        error.InnerException.Message.Contains("duplicate"))
                    {
                        if (error.InnerException.Message.Contains("_Name"))
                            msgEr.ErrorMessage = "Name value must be unique";
                        if (error.InnerException.Message.Contains("_DepartmentId"))
                            msgEr.ErrorMessage = "DepartmentId value must be unique";
                    }
                    else if (error.Message.Contains("conflicted with the FOREIGN KEY constraint") || error.InnerException != null && error.InnerException.Message.Contains("conflicted with the FOREIGN KEY constraint"))
                    {
                        msgEr.ErrorMessage =  $"Ocurred an conflicted with a FOREIGN KEY.";
                    }
                    else
                    {
                        msgEr.ErrorMessage = $"{error.Message}{error.InnerException?.Message}";
                    }
                    NotifyError(msgEr);
                }
                Notify($"Processing departments {_currentRow} of {_totalRows}");
            }
            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            NotifyUpdate(msg);
        }
    }
}
