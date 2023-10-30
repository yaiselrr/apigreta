using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Handlers.Queries.Location;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class LocationController : BaseController
{
    private readonly IMediator _mediator;

    public LocationController(
        IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Get All Countries
    /// </summary>
    /// <returns>Returns the Countries</returns>
    [HttpGet]
    [Route("[action]")]
    [ProducesResponseType(typeof(List<CountryModel>), 200)]
    public async Task<IActionResult> GetCountries()
    {
        return Ok(await _mediator.Send(new CountryGetAll.Query()));
    }

    /// <summary>
    ///     Get Provices by CountryId
    /// </summary>
    /// <param name="countryId">Country Id</param>
    /// <returns>Return List of provinces or notfount exception</returns>
    [HttpGet]
    [Route("[action]/{countryId}")]
    [ProducesResponseType(typeof(List<ProvinceModel>), 200)]
    public async Task<IActionResult> GetProvincesByCountry(long countryId)
    {
        return Ok(await _mediator.Send(new ProvincesByCountry.Query(countryId)));
    }
}