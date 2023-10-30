using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Notifiers
{
    public class SignalRNotifier : INotifier
    {
        private readonly IHubContext<FrontHub, IFrontHub> _handler;

        private HandlerMessage _message; // last message
        
        public SignalRNotifier(IHubContext<FrontHub, IFrontHub> handler)
        {
            _handler = handler;
        }

        public HandlerMessage Message => _message;

        public async Task NotifyUpdateAsync(HandlerMessage msg)
        {
            msg.MessageLevel = MessageLevel.Information;
            _message = msg;
            await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
        }

        public void NotifyUpdate(HandlerMessage msg)
        {
            Task.Factory.StartNew(async () => await NotifyUpdateAsync(msg));
        }

        public async Task NotifyErrorAsync(HandlerMessage msg)
        {
            msg.MessageLevel = MessageLevel.Error;
            _message = msg;
            await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
        }

        public void NotifyError(HandlerMessage msg)
        {
            Task.Factory.StartNew(async () => await NotifyErrorAsync(msg));
        }

        private object AsHubMessage(HandlerMessage msg)
        {
            return new 
            {
                Stage = msg.Stage.ToString(),
                Finish = msg.Stage == Stage.Completed,
                InsertedRowCount = msg.InsertedRows,
                FailedRowCount = msg.FailedRows,
                UpdatedRowCount = msg.UpdatedRows,
                ProcessedRowCount = msg.ProcessedRows,
                Messages = msg.AllMessages,
                msg.TotalRows,
                msg.CurrentRow,
                msg.Errors,
                msg.Message
            };
        }
        
    }
}
