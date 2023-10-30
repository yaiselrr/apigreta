using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class CSVMapping : BaseEntityLong
    {
        public string Name { get; set; }
        public string MapperJson { get; set; }
        public ModelImport ModelImport { get; set; }
    }
}