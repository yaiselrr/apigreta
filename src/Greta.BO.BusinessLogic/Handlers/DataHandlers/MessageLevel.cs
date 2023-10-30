namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    ///  diferent message level, it used to determ how to handle
    ///  the message, in back and front
    /// </summary>
    public enum MessageLevel : byte
    {
        /// <summary>Debugging message</summary>
        Debug = 1,

        /// <summary>simple information to inform to user, about current step and all that</summary>
        Information = 2,

        /// <summary>Warning message that could be handle as a minor error or so</summary>
        Warning = 3,

        /// <summary>
        /// Error that could or not affect the import/export process how the system
        /// react to it should be determ by the message handler.
        /// </summary>
        Error = 4
    }
}
