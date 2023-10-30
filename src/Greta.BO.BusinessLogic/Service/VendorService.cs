using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Helpers;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IVendorService : IGenericBaseService<Vendor>
    {
        Task<Vendor> GetByName(string name, long Id = -1);
        Task<VendorVendorContact> GetWithImages(long id);
    }

    public class VendorService : BaseService<IVendorRepository, Vendor>, IVendorService
    {
        public VendorService(IVendorRepository vendorRepository, ILogger<VendorService> logger, ISynchroService synchroService)
            : base(vendorRepository, logger, synchroService, Converter)
        {
        }
        private static object Converter(Vendor from) => (LiteVendor.Convert(from));

        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Customer</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<Vendor> Get(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<Vendor>()
                .Include(x => x.VendorContacts)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return entity;
        }

        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Customer</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<VendorVendorContact> GetWithImages(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var root = await _repository.GetEntity<Vendor>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            // preserve only for example
            // var contacts = await (from c in this._repository.GetEntity<VendorContact>()
            //                       join img1 in this._repository.GetEntity<Image>()
            //                             on new { c.Id } equals new { img1.OwnerId, img1.Type = 4 } into gvi
            //                       from u in gvi.DefaultIfEmpty()
            //                       where c.VendorId == id
            //                       select new VendorContactImage()
            //                       {
            //                           VendorContact = c,
            //                           //   Phone = c.Phone,
            //                           //   Fax = c.Fax,
            //                           //   Email = c.Email,
            //                           //   VendorId = c.VendorId,
            //                           // Val = gvi,
            //                           Image = u,
            //                           // Image = u != null ? new
            //                           // {
            //                           //     Id = u.Id,
            //                           //     Path = u.Path
            //                           // } : null
            //                       }
            //                         ).ToListAsync();

            var contacts = await _repository.GetEntity<VendorContact>()
                .LeftJoin(
                    _repository.GetEntity<Image>().Where(x => x.Type == ImageType.VENDORCONTACT),
                    contact => contact.Id,
                    image => image.OwnerId,
                    (contacts, image) => new VendorContactImage
                    {
                        VendorContact = contacts,
                        Image = image
                    }
                )
                .Where(x =>
                    x.VendorContact.VendorId == id)
                .ToListAsync();


            return new VendorVendorContact
            {
                Vendor = root,
                VendorContactImages = contacts
            };
        }

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">Vendor name</param>
        /// <returns>Vendor</returns>
        public async Task<Vendor> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<Vendor>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<Vendor>().Where(x => x.Name == name && x.Id != Id).FirstOrDefaultAsync();
        }

        protected override IQueryable<Vendor> FilterqueryBuilder(
            Vendor filter,
            string searchstring,
            string[] splited,
            DbSet<Vendor> query)
        {
            IQueryable<Vendor> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring)
                                          || c.AccountNumber.Contains(searchstring)
                                          || c.CountryName.Contains(searchstring)
                                          || c.ProvinceName.Contains(searchstring)
                                          || c.CityName.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(!string.IsNullOrEmpty(filter.AccountNumber),
                        c => c.AccountNumber.Contains(filter.AccountNumber))
                    .WhereIf(filter.CityName != null, c => c.CityName == filter.CityName)
                    .WhereIf(filter.CountryId > 0, c => c.CountryId == filter.CountryId)
                    .WhereIf(filter.ProvinceId > 0, c => c.ProvinceId == filter.ProvinceId);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "accountnumber" && e[1] == "asc", e => e.AccountNumber)
                .OrderByDescendingCase(e => e[0] == "accountnumber" && e[1] == "desc", e => e.AccountNumber)
                .OrderByCase(e => e[0] == "countryid" && e[1] == "asc", e => e.CountryName)
                .OrderByDescendingCase(e => e[0] == "countryid" && e[1] == "desc", e => e.CountryName)
                .OrderByCase(e => e[0] == "provinceid" && e[1] == "asc", e => e.CountryName)
                .OrderByDescendingCase(e => e[0] == "provinceid" && e[1] == "desc", e => e.ProvinceName)
                .OrderByCase(e => e[0] == "cityname" && e[1] == "asc", e => e.CountryName)
                .OrderByDescendingCase(e => e[0] == "cityname" && e[1] == "desc", e => e.CityName)
                .OrderByDefault(e => e.Name);

            return query1;
        }
    }
}