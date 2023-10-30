using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Greta.BO.Api.Test.ExportImport
{
    public class FakeNotifier : INotifier
    {
        public HandlerMessage Message { get; set; }

        public void SendMessage(HandlerMessage msg)
        {
            Message = msg;
        }

        public Task NotifyUpdateAsync(HandlerMessage msg)
        {
            Message = msg;
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        public void NotifyUpdate(HandlerMessage msg)
        {
            Message = msg;
            Console.WriteLine(msg);
        }

        public Task NotifyErrorAsync(HandlerMessage msg)
        {
            Message = msg;
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        public void NotifyError(HandlerMessage msg)
        {
            Message = msg;
            Console.WriteLine(msg);
        }
    }

    public class FakeNotifierTests
    {
        private readonly IServiceProvider _provider;

        public FakeNotifierTests()
        {
            _provider = new ServiceCollection()
                .AddSingleton<INotifier, FakeNotifier>()
                .BuildServiceProvider();
        }

        [Fact]
        public void FakeNotifierTests_Instantiation()
        {
            var notifier = _provider.GetRequiredService<INotifier>();

            Assert.True(notifier.GetType() == typeof(FakeNotifier));
        }

        [Fact]
        public async Task FakeNotifierTests_send_update_message_async()
        {
            var notifier = _provider.GetRequiredService<INotifier>();

            await notifier.NotifyUpdateAsync(HandlerMessage.New("Test message"));

            Assert.Equal(MessageLevel.Information, notifier.Message.MessageLevel);
        }

        [Fact]
        public void FakeNotifierTests_send_update_message()
        {
            var notifier = _provider.GetRequiredService<INotifier>();

            notifier.NotifyUpdate(HandlerMessage.New("Test mesage"));

            Assert.Equal(MessageLevel.Information, notifier.Message.MessageLevel);
        }
    }
}