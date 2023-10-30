using Greta.BO.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Models.Dto.Column
{
    public class ColumnNameModel: BaseEntityLong
    {
        public bool Exported { get; set; }
        public string Name { get; set; }
    }
}
