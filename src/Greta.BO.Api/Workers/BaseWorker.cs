using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Greta.BO.Api.Workers
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseWorker: BackgroundService
    {
        protected async Task WaitForCancellationAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> cancelationTaskCompletionSource = new TaskCompletionSource<bool>();
            cancellationToken.Register(taskCompletionSource => ((TaskCompletionSource<bool>)taskCompletionSource).SetResult(true), cancelationTaskCompletionSource);

            try
            {
                await (cancellationToken.IsCancellationRequested ? Task.CompletedTask : cancelationTaskCompletionSource.Task);
            }
            catch (OperationCanceledException)
            { }
        }
    }
}