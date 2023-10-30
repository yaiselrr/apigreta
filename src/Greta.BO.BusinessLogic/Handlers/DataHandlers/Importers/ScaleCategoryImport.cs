#nullable enable
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
    public class ScaleCategoryImport : BaseImport<ScaleCategory>
    {
        public ScaleCategoryImport(ILogger<ScaleCategoryImport> logger, IServiceProvider provider, 
            ILogger<Introspector<ScaleCategory>> inLogger, INotifier notifier)
            : base(logger, provider, inLogger, notifier)
        {
            
        }

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var departmentService = Provider.GetRequiredService<IDepartmentService>();
            var scaleCategoryService =  Provider.GetRequiredService<IScaleCategoryService>();
            List<string> errors = new();
            _currentRow = 0;
            _totalRows = 0;
            _insertedRows = 0;
            _updatedRows = 0;
            _failedRows = 0;
            var validations = new ValidationPipeline<ScaleCategory>(_notifier)
                .Add<ScaleCategoryValidationName>()
                .Add<ScaleCategoryValidationCategoryId>()
                .Add<ScaleCategoryValidationDepartmentId>()
                .Add<ScaleCategoryValidationExistsDepartment>();
            var scaleCategories = mapping.MapToEntityList<ScaleCategory>(
                csv, 
                null,
                (msg) =>
                {
                    if(msg.MessageLevel==MessageLevel.Information)
                        Notify(HandlerMessage.From(BuildMessage(), msg.Message));
                    else
                    {
                        NotifyError(HandlerMessage.From(BuildMessage(), msg.Message, true));
                    }
                });
            _totalRows = scaleCategories.Count;
            Notify("Initializing");
            for(var i = 0; i < scaleCategories.Count; i++)
            {
                _currentRow++;
                try
                {
                    var validation = await validations.Execute(scaleCategories[i], BuildMessage());
                    if (!validation.IsValid)
                    {
                        _failedRows++;
                        errors.Add(((HandlerMessage)validation.Msg!).Message);
                        NotifyError(BuildMessage());
                    }
                    else
                    {   
                        var scaleCategory = await scaleCategoryService.GetByScaleCategoryId(
                                scaleCategories[i].CategoryId);
                        scaleCategories[i].DepartmentId = await departmentService.GetIdFromDepartmentId(
                                (int)scaleCategories[i].DepartmentId);
                            if(scaleCategory == null)
                            {
                                scaleCategories[i].BackgroundColor ??= "#000000";
                                scaleCategories[i].ForegroundColor ??= "#ffffff";
                                await scaleCategoryService.Post(scaleCategories[i]);
                                _insertedRows++;
                            }
                            else
                            {
                                scaleCategory.Name = scaleCategories[i].Name;
                                await scaleCategoryService.Put(scaleCategory.Id, scaleCategory);
                                _updatedRows++;
                            }
                    }
                }
                catch(Exception error)
                {
                    _failedRows++;
                    var msgEr = BuildMessage();
                     if (error.InnerException != null &&
                        error.InnerException.Message.Contains("duplicate"))
                    {
                        if (error.InnerException.Message.Contains("_Name"))
                            msgEr.ErrorMessage = "Name value must be unique";
                        else if (error.InnerException.Message.Contains("_CategoryId"))
                            msgEr.ErrorMessage = "CategoryId value must be unique";
                    }
                    else if (error.Message.Contains("conflicted with the FOREIGN KEY constraint") || 
                            error.InnerException != null && 
                            error.InnerException.Message.Contains("conflicted with the FOREIGN KEY constraint"))
                    {
                        msgEr.ErrorMessage = $"Occurred an conflicted with a FOREIGN KEY.";
                    }
                    else
                    {
                        msgEr.ErrorMessage = $"{error.Message}{error.InnerException?.Message}";
                    }
                    NotifyError(msgEr);

                }
                Notify($"Processing Scale category {_currentRow} of {_totalRows}");
            }
            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            NotifyUpdate(msg);
        }
    }
}
