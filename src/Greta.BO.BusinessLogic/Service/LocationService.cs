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
    public interface ILocationService : IBaseService
    {
        Task<List<Country>> GetCountries();
        Task<Country> GetCountryById(long id);
        Task<List<Province>> GetProvincesByCountry(long countryId);
    }

    public class LocationService : ILocationService
    {

        private readonly ICountryRepository _countryrepository;

        private readonly ILogger<LocationService> _logger;
        private readonly IProvinceRepository _provincerepository;

        public LocationService(
            ICountryRepository countryRepository,
            IProvinceRepository provincerepository,
            ILogger<LocationService> logger)
        {
            _countryrepository = countryRepository;
            _provincerepository = provincerepository;
            _logger = logger;
        }

        /// <summary>
        ///     Get all Countries
        /// </summary>
        /// <returns></returns>
        public async Task<List<Country>> GetCountries()
        {
            var entities = await _countryrepository.GetEntity<Country>().ToListAsync();
            return entities;
        }

        /// <summary>
        ///     Get Country by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Country</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<Country> GetCountryById(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _countryrepository.GetEntity<Country>().FindAsync(id);
            return entity;
        }

        /// <summary>
        ///     Get All Provinces by countryId
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <returns>Provinces for countryId passed by parameters</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<List<Province>> GetProvincesByCountry(long countryId)
        {
            if (countryId < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entities = await _provincerepository.GetEntity<Province>()
                .Where(x => x.CountryId == countryId)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return entities;
        }
    }
}