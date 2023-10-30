using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class QtyHandTrackRepository : OperationBase<long, string, QtyHandTrack>, IQtyHandTrackRepository
    {
        public QtyHandTrackRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }
        
        public async Task<long> CreateQtyHandTrack(Product product, Store store, decimal oldQtyHand, decimal newQtyHand, CancellationToken cancellationToken = default)
        {
            var qtyHandTrack = new QtyHandTrack()
            {
                ProductId = product.Id,
                StoreId = store.Id,
                DepartmentId = product.Department.Id,
                CategoryId = product.Category.Id,
                ScaleCategoryId = null,
                //ScaleCategoryId = (product.ProductType == ProductType.SLP)? (product as ScaleProduct).ScaleCategory.Id : null,
                ProductName = product.Name,
                UPC = product.UPC,
                StoreName = store.Name,
                DepartmentName = product.Department.Name,
                CategoryName = product.Category.Name,
                ScaleCategoryName = null,
                //ScaleCategoryName = (product.ProductType == ProductType.SLP)? (product as ScaleProduct).ScaleCategory.Name : null,
                OldQtyHand = oldQtyHand,
                NewQtyHand = newQtyHand,
                Username = this.AuthenticateUser.UserName,
                UserId = this.AuthenticateUser.UserId
            };
            return await base.CreateAsync(qtyHandTrack, cancellationToken);
        }
    }
}