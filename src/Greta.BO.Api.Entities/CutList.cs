using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities;

public class CutList : BaseEntityLong, IFullSyncronizable
{
    public long AnimalId { get; set; }
    public long CustomerId { get; set; }
    public CutListType CutListType { get; set; }
    public string SpecialInstruction { get; set; }
    public List<CutListDetail> CutListDetails { get; set; }

    public Animal Animal { get; set; }
    public Customer Customer { get; set; }

    public long? CutListTemplateId { get; set; }
    public CutListTemplate CutListTemplate { get; set; }
}