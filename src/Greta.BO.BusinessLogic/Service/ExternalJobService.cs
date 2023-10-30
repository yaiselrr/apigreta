using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IExternalJobService : IGenericBaseService<ExternalJob>
    {
        Task<long> CreateTask(ExternalJobType type, object data, string rawData);
        Task<List<ExternalJob>> GetByType(ExternalJobType type);
        Task<bool> SentTask(long taskId);
        Task<bool> FailTask(long taskId);
        Task<bool> CompleteTask(long taskId);
    }

    public class ExternalJobService : BaseService<IExternalJobRepository, ExternalJob>, IExternalJobService
    {
        public ExternalJobService(IExternalJobRepository repository,
            ILogger<ExternalJobService> logger)
            : base(repository, logger)
        {
        }

        public async Task<List<ExternalJob>> GetByType(ExternalJobType type) =>
            await _repository.GetEntity<ExternalJob>().Where(x => x.Type == type).ToListAsync();

        public async Task<long> CreateTask(ExternalJobType type, object data, string rawData = null)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            var json = JsonSerializer.Serialize(data, options);

            return await _repository.CreateAsync(new ExternalJob()
            {
                State = true,
                Status = ExternalJobStatus.Init,
                Type = type,
                Data = json,
                RawData = rawData
            });
        }

        public async Task<bool> SentTask(long taskId) => await UpdateStatus(taskId, ExternalJobStatus.Sent);
        public async Task<bool> FailTask(long taskId) => await UpdateStatus(taskId, ExternalJobStatus.Fail);
        public async Task<bool> CompleteTask(long taskId) => await UpdateStatus(taskId, ExternalJobStatus.Complete);

        private async Task<bool> UpdateStatus(long taskId, ExternalJobStatus status)
        {
            var task = await _repository.GetEntity<ExternalJob>().FirstOrDefaultAsync(x => x.Id == taskId);
            task.Status = status;
            return await _repository.UpdateAsync(task.Id, task);
        }
    }
}