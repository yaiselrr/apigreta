#nullable  enable

using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    /// System notifier
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// <see cref="HandlerMessage"/> instance with message to send
        /// </summary>
        HandlerMessage Message { get; }

        /// <summary>
        /// send an async <see cref="HandlerMessage"/> message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task NotifyUpdateAsync(HandlerMessage msg);

        /// <summary>
        /// send a sync <see cref="HandlerMessage"/> message
        /// </summary>
        /// <param name="msg"></param>
        void NotifyUpdate(HandlerMessage msg);
        
        /// <summary>
        /// send an async <see cref="HandlerMessage"/> message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task NotifyErrorAsync(HandlerMessage msg);

        /// <summary>
        /// send a sync <see cref="HandlerMessage"/> message
        /// </summary>
        /// <param name="msg"></param>
        void NotifyError(HandlerMessage msg);
    }
}
