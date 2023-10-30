using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Dto;
using Greta.BO.Api.Dto.Search;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Handlers.Command.CSVMapping;
using Greta.BO.BusinessLogic.Handlers.Queries.CSVMapping;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class CSVMappingController : BaseController
{
    private readonly IMediator _mediator;

    public CSVMappingController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }


    /// <summary>
    ///     Get all the Entities
    /// </summary>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new CSVMappingGetAll.Query()));
    }

    /// <summary>
    ///     Get filter and paginate Mapping
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <param name="modelImport">Model to import</param>
    /// <returns>Returns the paginated Category</returns>
    [HttpPost]
    [Route("{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(CategoryFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, ModelImport modelImport,
        [FromBody] CSVMappingSearchDto filter)
    {
        return Ok(await _mediator.Send(new CSVMappingFilter.Query(currentPage, pageSize,
            _mapper.Map<CSVMappingSearchModel>(filter), modelImport)));
    }


    /// <summary>
    ///     Export mapping to .GMT
    /// </summary>
    /// <param name="id">mapping id</param>
    /// <returns>Return json</returns>
    [HttpGet]
    [Route("[action]/{id}")]
    public async Task<IActionResult> ExportCSVMapping(long id)
    {
        //get mapping
        var mapp = (await _mediator.Send(new CSVMappingGetById.Query(id)))?.Data;
        if (mapp == null)
            return NotFound();

        var jsonMapp =
            JsonConvert.SerializeObject(new { mapp.CsvHeader, mapp.ModelHeader, mapp.ModelImport, mapp.Name });
        // create stream and file o download
        // var buffer = Encoding.ASCII.GetBytes(jsonMapp);
        var memoryStream = new MemoryStream();
        TextWriter tw = new StreamWriter(memoryStream);

        await tw.WriteLineAsync(jsonMapp);
        await tw.FlushAsync();
        tw.Close();

        return File(memoryStream.GetBuffer(), "text/plain", $"mapp_{mapp.Name}.gmt");
    }

    /// <summary>
    ///     import mapping from .GMT
    /// </summary>
    /// <param name="importJSON">mapping in json format</param>
    /// <returns>Return mapping</returns>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(List<List<string>>), 200)]
    public async Task<IActionResult> ImportCSVMappingAsync([FromForm] CSVJsonImportModel importJSON)
    {
        var reader = new StreamReader(importJSON.JSONFile.OpenReadStream());
        var json = reader.ReadToEnd();

        var mapping = JsonConvert.DeserializeObject<CSVMappingModel>(json);
        mapping.Name = importJSON.Name;

        return await Post(mapping);
    }


    /// <summary>
    ///     Get by Id
    /// </summary>
    /// <param name="id">Model id</param>
    /// <returns>Return list of entities</returns>
    [HttpGet]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var data = await _mediator.Send(new CSVMappingGetById.Query(id));
        return data == null ? NotFound() : Ok(data);
    }

    /// <summary>
    ///     Get by ModelImport
    /// </summary>
    /// <param name="modelImport">model import from Mapping</param>
    /// <returns>Return list of entities by modelImport</returns>
    [HttpGet]
    [Route("[action]/{modelImport}")]
    public async Task<IActionResult> GetByModelImport(ModelImport modelImport)
    {
        var data = await _mediator.Send(new CSVMappingGetByModelImport.Query(modelImport));
        return data == null ? NotFound() : Ok(data);
    }

    /// <summary>
    ///     Get Headers from CSV File and Model
    /// </summary>
    /// <param name="mappDto">Filter Object</param>
    /// <returns>Return a list with csv file headers list and model headers list</returns>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(ColumnsDefinitionsAndSugestions), 200)]
    public async Task<IActionResult> GetColumnsName([FromForm] CSVImportModel mappDto)
    {
        var data = await _mediator.Send(new GetColumnsNameQuery(mappDto.CSVFile, mappDto.Separator,
            mappDto.ModelImport));
        return data == null ? NotFound() : Ok(data);
    }

    /// <summary>
    ///     Get Columns by Model
    /// </summary>
    /// <param name="modelExport">Filter Object</param>
    /// <returns>Return a list with csv file headers list and model headers list</returns>
    [HttpGet]
    [Route("[action]/{modelExport}")]
    // [ProducesResponseType(typeof(List<ColumnModelDTO>), 200)]
    public async Task<IActionResult> GetColumsNameByModelExport(ModelImport modelExport)
    {
        var data = await _mediator.Send(new GetColumsNameByModelExport.Query(modelExport));
        return data == null ? NotFound() : Ok(data);
    }

    /// <summary>
    ///     Imports  CSV File
    /// </summary>
    /// <param name="mappDto">Filter Object</param>
    /// <returns>Return a list with csv file headers list and model headers list</returns>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(List<List<string>>), 200)]
    public async Task<IActionResult> CSVMappingImport([FromForm] CSVImportModel mappDto)
    {
        // var data = await _mediator.Publish(new CSVMappingImport.Command(_mapper.Map<CSVImportModel>(mappDto)));
        // return data == null ? BadRequest() : Ok(data);

        await _mediator.Publish(new CSVMappingImport.Command(mappDto));
        return Ok();
    }


    /// <summary>
    ///     Create Mapping
    /// </summary>
    /// <param name="mapping">Mapping to create</param>
    /// <returns>Returns the paginated Vendor</returns>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Post([FromBody] CSVMappingModel mapping)
    {
        var result = await _mediator.Send(new CSVMappingCreate.Command(mapping));
        return Ok(result);
    }

    /// <summary>
    ///     Edit Mapping
    /// </summary>
    /// <param name="mapping">Mapping to update</param>
    /// <param name="entityId">Mappig Id</param>
    /// <returns>Returns the paginated Vendor</returns>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Put(long entityId, [FromBody] CSVMappingModel mapping)
    {
        return entityId < 1
            ? NotFound()
            : Ok(await _mediator.Send(new CSVMappingUpdate.Command(mapping)));
    }


    /// <summary>
    ///     Delete a Mapping by ModelImport
    /// </summary>
    /// <param name="entityId">Entity Id</param>
    /// <returns>Return true or false</returns>
    [HttpDelete]
    [Route("{entityId}")]
    // [ProducesResponseType(typeof(CategoryDelete.Response), 200)]
    public async Task<IActionResult> Delete(long entityId)
    {
        return entityId < 1 ? NotFound() : Ok(await _mediator.Send(new CSVMappingDelete.Command(entityId)));
    }

    /// <summary>
    ///     Get filter and paginate products
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter Object</param>
    /// <returns>Returns the paginated products</returns>
    [HttpPost]
    [Route("[action]/{currentPage}/{pageSize}")]
    // [ProducesResponseType(typeof(ProductFilter.Response), 200)]
    public async Task<IActionResult> Filter(int currentPage, int pageSize, [FromBody] ColumnNameSearchDto filter)
    {
        return Ok(await _mediator.Send(new ColumnNameFilter.Query(currentPage, pageSize,
            _mapper.Map<ColumnNameSearchModel>(filter))));
    }
}