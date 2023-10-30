using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IVendorContactService : IGenericBaseService<VendorContact>
    {
        Task<List<VendorContact>> GetByVendor(long vendorId);
        Task<VendorContact> GetByContactVendor(string contact, long vendorId, long Id = -1);
    }

    public class VendorContactService : BaseService<IVendorContactRepository, VendorContact>, IVendorContactService
    {
        public VendorContactService(IVendorContactRepository vendorRepository, ILogger<VendorContactService> logger)
            : base(vendorRepository, logger)
        {
        }

        /// <summary>
        ///     Get list of vendor contact by vendor Id
        /// </summary>
        /// <param name="vendorId">Vendor Id</param>
        /// <returns>Customer</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<List<VendorContact>> GetByVendor(long vendorId)
        {
            if (vendorId < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<VendorContact>().Where(x => x.VendorId == vendorId).ToListAsync();
            return entity;
        }

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="contact">VendorContact contact</param>
        /// <param name="vendorId">VendorContact Vendor id</param>
        /// <returns>VendorContact</returns>
        public async Task<VendorContact> GetByContactVendor(string contact, long vendorId, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<VendorContact>()
                    .Where(x => x.Contact == contact && x.VendorId != vendorId).FirstOrDefaultAsync();
            return await _repository.GetEntity<VendorContact>()
                .Where(x => x.Contact == contact && x.VendorId != vendorId && x.Id != Id).FirstOrDefaultAsync();
        }
    }
}