using System.Diagnostics.CodeAnalysis;
using Greta.BO.BusinessLogic.Hubs;
using Greta.Sdk.HubClient;

namespace Greta.BO.Worker
{
    [ExcludeFromCodeCoverage]
    public class PingPongWorker : BackgroundService
    {
        private readonly IBoHubClient _client;
        private readonly ILogger<PingPongWorker> _logguer;

        public PingPongWorker(
            IBoHubClient statusHubClient,
            ILogger<PingPongWorker> logguer
        )
        {
            this._client = statusHubClient;
            this._logguer = logguer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (((HubClient)_client).isConnect)
                {
                    var ok = await _client.PingPong();
                    if (!ok)
                    {
                        _logguer.LogError("BO Ping pong fail");
                        await ((HubClient)_client).Init();
                    }

                    await Task.Delay(30000, stoppingToken);
                }
                else
                {
                    _logguer.LogWarning("SignalR bridge dont open, sending connection link");
                    try
                    {
                        await ((HubClient)_client).Init();
                    }
                    catch
                    {
                        // ignored
                    }

                    await Task.Delay(15000, stoppingToken);
                }
            }

            await WaitForCancellationAsync(stoppingToken);

            _logguer.LogInformation("Dispose realtime connection");
            await _client.connection.DisposeAsync();
        }

        private async Task WaitForCancellationAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> cancelationTaskCompletionSource = new TaskCompletionSource<bool>();
            cancellationToken.Register(
                taskCompletionSource => (((TaskCompletionSource<bool>)taskCompletionSource!)!).SetResult(true),
                cancelationTaskCompletionSource);

            try
            {
                await (cancellationToken.IsCancellationRequested
                    ? Task.CompletedTask
                    : cancelationTaskCompletionSource.Task);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}