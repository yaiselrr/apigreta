using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Permission : BaseEntityLong
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public long FunctionGroupId { get; set; }

        public FunctionGroup FunctionGroup { get; set; }

        public virtual List<Profiles> Profiles { get; set; }
    }
}