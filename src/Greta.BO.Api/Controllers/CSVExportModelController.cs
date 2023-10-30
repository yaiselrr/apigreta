using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Queries.CSVMapping;
using Greta.BO.BusinessLogic.Handlers.Queries.Products;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Greta.BO.Api.Controllers;

[Route("api/[controller]")]
public class CSVExportModelController : BaseController
{
    private readonly IMediator _mediator;

    public CSVExportModelController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Export mapping to .CSV
    /// </summary>
    /// <param name="columns">columns </param>
    /// <returns>Return json</returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> ExportCSVProduct([FromBody] List<string> columns)
    {
        //get mapping

        var result = (await _mediator.Send(new CSVGetAllProductExport.Query(columns))).Data;
        if (result == null)
            return NotFound();

        var jsonMapp =
            JsonConvert.SerializeObject(new { result.CsvHeader, result.ModelHeader, result.ModelImport, result.Name });
        // create stream and file o download
        var buffer = Encoding.ASCII.GetBytes(jsonMapp);
        var memoryStream = new MemoryStream();
        TextWriter tw = new StreamWriter(memoryStream);

        tw.WriteLine(jsonMapp);
        tw.Flush();
        tw.Close();

        return File(memoryStream.GetBuffer(), "text/plain", $"mapp_{result.Name}.csv");
    }

    /// <summary>
    ///   Export entity data into .CSV 
    /// </summary>
    [HttpPost]
    [Route("Export")]
    public async Task<IActionResult> ExportCSV([FromBody] CSVExportEntityInputModel payload)
    {
        var request =
            (await _mediator.Send(new CSVEntityExport.Query(payload.Model, payload.Columns, payload.StoreIds)));
        if (request.Data == null)
            return new NotFoundResult();

        // create stream and file o download
        var buffer = Encoding.ASCII.GetBytes(request.Data);
        var memoryStream = new MemoryStream();
        TextWriter tw = new StreamWriter(memoryStream);
        tw.WriteLine(request.Data);
        tw.Flush();
        tw.Close();

        return File(memoryStream.GetBuffer(), "text/csv", $"{payload.Model}_export.csv");
    }
}