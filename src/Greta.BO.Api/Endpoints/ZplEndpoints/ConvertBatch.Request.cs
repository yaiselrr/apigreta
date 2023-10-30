using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ZplEndpoints;

public class ConvertBatchRequest
{
    [FromRoute(Name = "store")]public long Store { get; set; }
    [FromRoute(Name = "batchId")]public long BatchId { get; set; }
    [FromRoute(Name = "labelId")]public long LabelId { get; set; }
}