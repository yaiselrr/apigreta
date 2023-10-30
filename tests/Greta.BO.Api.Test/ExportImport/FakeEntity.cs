using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.Api.Test.ExportImport
{
    public class FakeEntity : BaseEntityLong
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class BaseEntityLong : IEntityLong<string>
    {
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
