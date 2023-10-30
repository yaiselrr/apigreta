#nullable  enable
using Ardalis.Specification;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.Core.Abstractions;
using Greta.Sdk.Core.Models.Pager;

namespace Greta.BO.Api.Test.ExportImport
{
    public interface IFakeService : IGenericBaseService<FakeEntity>
    {
    }

    public class FakeService : IFakeService 
    {
        private List<FakeEntity?> _data = new List<FakeEntity?>
        {
            new FakeEntity { Id = 1, Name = "entity1" } ,
            new FakeEntity { Id = 2, Name = "entity2" },
            new FakeEntity { Id = 3, Name = "entity3" },
            new FakeEntity { Id = 4, Name = "entity4" } ,
        };

        public Task<IReadOnlyList<FakeEntity>> Get(Specification<FakeEntity> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TResult>> Get<TResult>(Specification<FakeEntity, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<FakeEntity> Get(SingleResultSpecification<FakeEntity> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Get<TResult>(SingleResultSpecification<FakeEntity, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<FakeEntity>> Filter(int currentPage, int pageSize, FakeEntity filter, string search, string sort)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<FakeEntity>> FilterSpec(int currentPage, int pageSize, ISpecification<FakeEntity> spec, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<TResult>> FilterSpec<TResult>(int currentPage, int pageSize, ISpecification<FakeEntity, TResult> spec,
            CancellationToken cancellationToken = default) where TResult : class, IDtoLong<string>
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeState(long id, bool state)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(List<long> ids)
        {
            throw new NotImplementedException();
        }

        public Task<List<FakeEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<FakeEntity?> Get(long id) =>
            await Task.FromResult(_data.FirstOrDefault(x => x.Id == id));

        public Task<Pager<FakeEntity>> Page(int currentPage, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<FakeEntity> Post(FakeEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Put(long id, FakeEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

