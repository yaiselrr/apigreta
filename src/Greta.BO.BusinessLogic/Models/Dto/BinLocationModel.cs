using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class BinLocationModel : IDtoLong<string>, IMapFrom<BinLocation>
    {
        public string Name { get; set; }
        [Required] public int Aisle { get; set; }
        [Required] public int Side { get; set; }
        [Required] public int Section { get; set; }
        [Required] public int Shelf { get; set; }
        [Required] public long Store { get; set; }
        
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}