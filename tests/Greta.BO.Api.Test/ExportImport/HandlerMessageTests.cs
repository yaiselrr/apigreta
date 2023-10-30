using Greta.BO.BusinessLogic.Handlers.DataHandlers;

namespace Greta.BO.Api.Test.ExportImport
{
    public class HandlerMessageTests
    {
        [Fact]
        public void HandlerMessageTests_instantiation()
        {
            var msg = new HandlerMessage();
            Assert.NotNull(msg);
            Assert.Equal(typeof(HandlerMessage), msg.GetType());
        }

        [Fact]
        public void HandlerMessageTest_instantiation_static_new()
        {
            var msg = HandlerMessage.New("message");
            Assert.NotNull(msg);
            Assert.Equal(typeof(HandlerMessage), msg.GetType());
            Assert.Equal("message", msg.Message);
            Assert.Equal(MessageLevel.Information, msg.MessageLevel);
        }

        [Fact]
        public void HandlerMessageTests_message_holder()
        {
            var msg = HandlerMessage.New("first message");
            msg.Message = "Second message";
            Assert.NotNull(msg);
            Assert.Equal(typeof(HandlerMessage), msg.GetType());
            // 4 because it register a MessageLevel message
            Assert.Equal(4, msg.AllMessages.Count);
        }

        [Fact]
        public void HandlerMessageTests_duplicated_message()
        {
            var msg = HandlerMessage.New("first message");
            msg.Message = "second message";
            msg.Message = "duplicated message";
            msg.Message = "duplicated message";
            
            Assert.NotNull(msg);
            Assert.Equal(5, msg.AllMessages.Count);
        }
    }
}
