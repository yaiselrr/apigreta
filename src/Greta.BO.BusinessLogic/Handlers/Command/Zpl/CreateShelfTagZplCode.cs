using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.Sdk.LabelConverter;
using Greta.Sdk.LabelConverter.models;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.Zpl;

/// <summary>
/// Create a zpl code for a shelf tag
/// </summary>
/// <param name="Model">Data model</param>
/// <param name="LabelType">label Type</param>
/// <param name="QtyToPrint">Quantity</param>
public record CreateShelfTagZplCodeCommand(ShelfTagHolderModel Model,
    LabelDesign LabelType,
    int QtyToPrint) : IRequest<List<string>>;

/// <inheritdoc />
public class CreateShelfTagZplCodeHandler : IRequestHandler<CreateShelfTagZplCodeCommand, List<string>>
{
    /// <inheritdoc />
    public Task<List<string>> Handle(CreateShelfTagZplCodeCommand request, CancellationToken cancellationToken)
    {
        var tempList = new List<string>();
        var r = request.Model.ToZpl(request.LabelType);
        for (var i = 0; i < request.QtyToPrint; i++)
            tempList.Add(r);
        return Task.FromResult(tempList);
    }
}