using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IScaleLabelDefinitionService : IGenericBaseService<ScaleLabelDefinition>
    {
        Task<long> GetIdByName(string toString);
    }

    public class ScaleLabelDefinitionService : BaseService<IScaleLabelDefinitionRepository, ScaleLabelDefinition>,
        IScaleLabelDefinitionService
    {
        public ScaleLabelDefinitionService(IScaleLabelDefinitionRepository repository,
            ISynchroService synchroService,
            ILogger<ScaleLabelDefinition> logger)
            : base(repository, logger, synchroService, Converter)
        {
        }

        private static object Converter(ScaleLabelDefinition from) => (LiteScaleLabelDefinition.Convert(from));

        protected override IQueryable<ScaleLabelDefinition> FilterqueryBuilder(
            ScaleLabelDefinition filter,
            string searchstring,
            string[] splited,
            DbSet<ScaleLabelDefinition> query)
        {
            IQueryable<ScaleLabelDefinition> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c =>
                    c.ScaleLabelType1.Name.Contains(searchstring) ||
                    c.ScaleLabelType2.Name.Contains(searchstring)
                );
            else
                query1 = query.WhereIf(filter.ScaleBrandId > 0, c => c.ScaleBrandId == filter.ScaleBrandId)
                    .WhereIf(filter.ScaleLabelType1Id > 0, c => c.ScaleLabelType1Id == filter.ScaleLabelType1Id)
                    .WhereIf(filter.ScaleLabelType2Id > 0, c => c.ScaleLabelType2Id == filter.ScaleLabelType2Id);
            if (filter.ScaleProductId > -1)
                query1 = query1.Where(x => x.ScaleProductId == filter.ScaleProductId);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "scalelabeltype1id" && e[1] == "asc", e => e.ScaleLabelType1.Name)
                .OrderByDescendingCase(e => e[0] == "scalelabeltype1id" && e[1] == "desc", e => e.ScaleLabelType1.Name)
                .OrderByCase(e => e[0] == "scalelabeltype2id" && e[1] == "asc", e => e.ScaleLabelType2.Name)
                .OrderByDescendingCase(e => e[0] == "scalelabeltype2id" && e[1] == "desc", e => e.ScaleLabelType2.Name)
                .OrderByDefault(e => e.ScaleLabelType1.Name)
                //includes
                .Include(x => x.ScaleLabelType1)
                .Include(x => x.ScaleLabelType2);


            return query1.Select(x => new ScaleLabelDefinition()
            {
                Id = x.Id,
                ScaleProductId = x.ScaleProductId,
                ScaleLabelType1Id = x.ScaleLabelType1Id,
                ScaleLabelType2Id = x.ScaleLabelType2Id,
                ScaleBrandId  = x.ScaleBrandId,

                ScaleProduct = x.ScaleProduct,
                ScaleLabelType1 = x.ScaleLabelType1 == null ? null : new ScaleLabelType()
                {
                    Name = x.ScaleLabelType1.Name,
                    LabelId = x.ScaleLabelType1.LabelId,
                    ScaleType = x.ScaleLabelType1.ScaleType,
                },
                ScaleLabelType2 = x.ScaleLabelType2 == null ? null : new ScaleLabelType()
                {
                    Name = x.ScaleLabelType2.Name,
                    LabelId = x.ScaleLabelType2.LabelId,
                    ScaleType = x.ScaleLabelType2.ScaleType,
                }
            });
        }

        public async Task<long> GetIdByName(string name)
        {
            return await _repository.GetEntity<ScaleLabelType>()
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }
    }
}