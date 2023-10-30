using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.ShelfTag;

/// <summary>
/// Add all product on the category to a shelfTag list
/// </summary>
/// <param name="CategoryId"></param>
public record ShelfTagAddByCategoryCommand(long CategoryId) : IRequest<ShelfTagAddByCategoryResponse>;

/// <summary>
/// Add all product on the category to a shelfTag list response
/// </summary>
public record ShelfTagAddByCategoryResponse : CQRSResponse<bool>;

/// <summary>
/// Add all product on the category to a shelfTag list handler
/// </summary>
public class ShelfTagAddByCategoryHandler : IRequestHandler<ShelfTagAddByCategoryCommand, ShelfTagAddByCategoryResponse>
{
    private readonly IShelfTagService _shelfTagService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shelfTagService"></param>
    public ShelfTagAddByCategoryHandler(
        IShelfTagService shelfTagService
    )
    {
        _shelfTagService = shelfTagService;
    }

    /// <inheritdoc />
    public async Task<ShelfTagAddByCategoryResponse> Handle(ShelfTagAddByCategoryCommand request,
        CancellationToken cancellationToken = default)
    {
        var response = await _shelfTagService.PostFromCategory(request.CategoryId, cancellationToken);
        return new ShelfTagAddByCategoryResponse() { Data = response };
    }
}