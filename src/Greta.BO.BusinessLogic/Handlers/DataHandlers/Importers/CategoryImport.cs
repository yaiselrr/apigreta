using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations;
using static Greta.BO.BusinessLogic.Handlers.DataHandlers.MessageLevel;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers
{
    public class CategoryImport : BaseImport<Category>
    {
        public CategoryImport(ILogger<CategoryImport> logger, IServiceProvider provider, 
            ILogger inLogger, INotifier notifier) 
            : base(logger, provider, inLogger, notifier)
        {
            
        }        

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var catService = Provider.GetRequiredService<ICategoryService>();
            var dptService = Provider.GetRequiredService<IDepartmentService>();
            List<string> errors = new();
            _currentRow = 0;
            _totalRows = 0;
            _insertedRows = 0;
            _updatedRows = 0;
            _failedRows = 0;
            // var msg = HandlerMessage.New("Initializing");
            var validations = new ValidationPipeline<Category>(_notifier)
                .Add<CategoryValidationName>()
                .Add<CategoryValidationCategoryId>()
                .Add<CategoryValidationDepartmentId>()
                .Add<CategoryValidationDepartment>();
            var categories = mapping.MapToEntityList<Category>(
                    csv, 
                    storesIds,
                    (msg) =>
                    {
                        if (msg.MessageLevel == Information)
                            NotifyUpdate(HandlerMessage.From(BuildMessage(), msg));
                        else 
                            NotifyError(HandlerMessage.From(BuildMessage(), msg, true));
                    });
            _totalRows = categories.Count;
            Notify("Initializing");
            for(var i = 0; i < categories.Count; i++)
            {
                _currentRow++;
                if(categories[i].DepartmentId == 0) categories[i].DepartmentId = 1;
                try
                {
                    var validation = await validations.Execute(categories[i], BuildMessage());
                    if (!validation.IsValid)
                    {
                        _failedRows++;
                        errors.Add(((HandlerMessage)validation.Msg!).Message);
                        //NotifyError(BuildMessage());
                        Notify($"line{_currentRow}: {((HandlerMessage)validation.Msg!).ErrorMessage}", true);
                    }
                    else
                    {
                        categories[i].DepartmentId = await dptService.GetIdFromDepartmentId((int) categories[i].DepartmentId);
                        // {
                            var category = await catService.GetByCategoryId(categories[i].CategoryId);
                            if(category == null)
                            {
                                categories[i].Description ??= "testDescription";
                                categories[i].BackgroundColor ??= "#000000";
                                categories[i].ForegroundColor ??= "#ffffff";
                                await catService.Post(categories[i]);
                                _insertedRows++;
                                Notify($"Category {i} inserted");
                            }
                            else
                            {
                                categories[i].Id = category.Id;
                                categories[i].UserCreatorId = category.UserCreatorId;
                                await catService.Put(category.Id, categories[i]);
                                _updatedRows++;
                                Notify($"Category {i} updated");
                            }
                    }

                }
                catch(Exception ex)
                {
                    _failedRows++;
                    var msgEr = BuildMessage();
                    if(ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                    {
                        if(ex.InnerException.Message.Contains("_Name"))
                            msgEr.ErrorMessage = "Name must be unique";
                        if(ex.InnerException.Message.Contains("_CategoryId"))
                            msgEr.ErrorMessage = "CategoryId value must be unique";
                    }
                    else if( ex.Message.Contains("conflicted with the FOREIGN KEY constraint")
                          || ex.InnerException != null
                          && ex.InnerException.Message.Contains("conflicted with the FOREIGN KEY constraint"))
                        msgEr.ErrorMessage = "Occurred a conflicted with a FOREIGN KEY.";
                    else
                        msgEr.ErrorMessage = $"{ex.Message}{ex.InnerException?.Message}";
                    Notify(msgEr);
                }
                // Notify($"Processing categories {_currentRow} of (_totalRows)");
                Notify();
            }
            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            Notify(msg);
        }
    }
}
