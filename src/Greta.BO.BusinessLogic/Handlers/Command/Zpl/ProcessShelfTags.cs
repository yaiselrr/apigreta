using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ShelfTags;
using Greta.Sdk.LabelConverter;
using Greta.Sdk.LabelConverter.models;
using MediatR;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Command.Zpl;
/// <summary>
/// Process all shelf tags and get the zpl code for all.
/// </summary>
/// <param name="Model"></param>
public record ProcessShelfTagsCommand(ProcessShelfTagModel Model) : IRequest<ProcessShelfTagsSelectedResponse>;


/// <summary>
/// handler for process all shelf tags and get the zpl code for all handler
/// </summary>
public class ProcessShelfTagsHandler : IRequestHandler<ProcessShelfTagsCommand, ProcessShelfTagsSelectedResponse>
{
    private readonly IShelfTagService _shelfTagService;
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="shelfTagService"></param>
    /// <param name="mediator"></param>
    public ProcessShelfTagsHandler(
        IShelfTagService shelfTagService,
        IMediator mediator
    )
    {
        _shelfTagService = shelfTagService;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<ProcessShelfTagsSelectedResponse> Handle(ProcessShelfTagsCommand request,
        CancellationToken cancellationToken)
    {
        var newModel = new ProcessShelfTagSelectedModel
        {
            StoreId = request.Model.StoreId,
            TagId = request.Model.TagId,
            ShelfTagIds = 
                (await _shelfTagService.Get(new ShelfTagPrintFilterSpec(request), cancellationToken))
                    .ToList()
        };

        return await _mediator.Send(new ProcessShelfTagsSelectedCommand(newModel), cancellationToken);
    }
}

