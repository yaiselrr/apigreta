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
using Greta.BO.BusinessLogic.Specifications.Generics;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers
{
    public class FamilyImport : BaseImport<Family>
    {
        public FamilyImport(ILogger<FamilyImport> logger, IServiceProvider provider, 
            ILogger<Introspector<Family>> inLogger, INotifier notifier)
            : base(logger, provider, inLogger, notifier)
        {

        }

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var familySerice = Provider.GetRequiredService<IFamilyService>();
            List<string> errors = new();
            _currentRow = 0;
            _totalRows = 0;
            _insertedRows = 0;
            _updatedRows = 0;
            _failedRows = 0;
            var validations = new ValidationPipeline<Family>(_notifier)
                .Add<FamilyValidationName>();
            var families = mapping.MapToEntityList<Family>(
                    csv, 
                    null,
                    // notifications are a simple scratch
                    (msg) =>
                    {
                        if (msg.MessageLevel == MessageLevel.Information)
                            Notify(HandlerMessage.From(BuildMessage(), msg));
                        else
                            NotifyError(HandlerMessage.From(BuildMessage(), msg, true));
                    });
            _totalRows = families.Count;
            Notify("Initializing");            
            for(var i = 0; i < families.Count; i++)
            {
                _currentRow++;
                try
                {
                    var validation = await validations.Execute(families[i], BuildMessage());
                    if (validation.IsValid)
                    {
                        _failedRows++;
                        errors.Add(((HandlerMessage)validation.Msg!).Message);
                        NotifyError(BuildMessage());
                    }
                    else
                    {
                        var family = await familySerice.Get(new CheckUniqueNameSpec<Family>(families[i].Name));
                        if(family == null)
                        {
                            await familySerice.Post(families[i]);
                            _insertedRows++;
                        }
                        else
                        {
                            families[i].Id = family.Id;
                            families[i].UserCreatorId = family.UserCreatorId;
                            await familySerice.Put(family.Id, families[i]);
                            _updatedRows++;
                        }
                    }
                }
                catch(Exception error)
                {
                    _failedRows++;
                    var msgEr = BuildMessage();
                    if(error != null && error.InnerException.Message.Contains("duplicate"))
                    {
                        if(error.Message.Contains("_Name"))
                            msgEr.ErrorMessage = "Name value must be unique";
                    }
                    else if(error.Message.Contains("conflicted with FOREIGN KEY constraint") ||
                            error.InnerException != null && 
                            error.InnerException.Message.Contains("conflicted with the FOREIGN KEY constraint"))
                    {
                        msgEr.ErrorMessage = "Ocurred an conflicted with a FOREIGN KEY";
                    }
                    else
                    {
                        msgEr.ErrorMessage = $"{error.Message}{error.InnerException.Message}";
                    }
                    NotifyError(msgEr);

                }
                Notify($"Processing Family products {_currentRow} of {_totalRows}");
            }
            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            NotifyUpdate(msg);
        }
    }
}
