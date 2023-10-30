using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities;
public class CutListDetail : BaseEntityLong
{
    public long CutListId { get; set; }
    public long ProductId { get; set; }
    public int Pack { get; set; }
    public decimal Thick { get; set; }

    public CutList CutList { get; set; }
    public Product Product { get; set; }
}