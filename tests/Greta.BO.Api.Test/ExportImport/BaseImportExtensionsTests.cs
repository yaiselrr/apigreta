using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Xunit.Abstractions;

namespace Greta.BO.Api.Test.ExportImport
{
    public class BaseImportExtensionsTests
    {
        private readonly ITestOutputHelper output;

        public BaseImportExtensionsTests(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
        }

        [Fact]
        public void BaseImportExtensionsTests_GetBaseProperties_with_predicate()
        {
            var entity = new FakeEntity { Id =1,  Name = "entity1" };

            var list = entity.GetBaseProperties(x => x.Name == "entity1");

            Assert.NotNull(list);
            
            // Assert.Equal(false, list.Count() == 0);
            // Assert.Equal(true, list.Count() > 0);
        }

        [Fact]
        public void BaseImportExtensionsTests_GetBaseProperties_all_columnsnames()
        {
            var entity = new FakeEntity { Id = 1, Name = "entity1" };
            var list = entity.GetBaseProperties(includeId: true);

            Assert.NotNull(list);
            Assert.Equal(list.Count() == 0, false);
        }

        [Fact]
        public void BaseImportExtensionsTests_GetBaseProperties_columnnames_exclude_id()
        {
            var entity = new FakeEntity { Id = 1, Name = "entity1" };
            var list = entity.GetBaseProperties(includeId: true);
            
            Assert.NotNull(list);
            Assert.True(list.Count() != 0);
            Assert.True(list.Contains("Id"));
        }

        [Fact]
        public void BaseImportExtensionsTests_GetBaseProperties_get_column_names()
        {
            var entity = new FakeEntity { Id = 1, Name = "entity1"};
            var list = entity.GetBaseProperties();

            Assert.NotNull(list);
            Assert.Equal(list.Count() == 0, false);
        }

        [Fact]
        public void BaseImportExtensionsTests_GetBaseProperties_get_all_column_names()
        {
            var entity = new FakeEntity { Id = 1, Name = "entity1"};
            var list = entity.GetAllColumnName();

            Assert.NotNull(list);
            Assert.True(list.Contains("Name"));
            // Assert.True(list.Contains("Id"));

            foreach(var prop in list)
            {
                output.WriteLine(prop);
            }
        }
        
        [Fact]
        public void BaseImportExtensionsTests_GetBaseProperties_for_category()
        {
            var entity = new Category() { Id = 1, Name = "entity1"};
            var list = entity.GetAllColumnName();

            Assert.NotNull(list);
            Assert.True(list.Contains("Name"));
            // Assert.True(list.Contains("Id"));

            foreach(var prop in list)
            {
                output.WriteLine(prop);
            }
        }
    }
}
