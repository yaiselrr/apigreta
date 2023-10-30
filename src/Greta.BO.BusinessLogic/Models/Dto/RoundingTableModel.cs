using System;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto;

public class RoundingTableModel : IDtoLong<string>, IMapFrom<RoundingTable>
{
    public int EndWith { get; set; }
    public int ChangeBy { get; set; }

    public long Id { get; set; }

    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}