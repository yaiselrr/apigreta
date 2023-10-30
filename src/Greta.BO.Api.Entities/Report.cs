using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Greta.BO.Api.Entities
{
    public class Report : BaseEntityLong
    {
        public string Name { get; set; }

        //public string Path { get; set; }

        public Guid GuidId { get; set; }

        public ReportCategory Category { get; set; }

    }
}