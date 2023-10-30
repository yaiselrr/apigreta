namespace Greta.BO.BusinessLogic.Handlers.DataHandlers
{
    /// <summary>
    /// stage in which the process is in
    /// </summary>
    public enum Stage
    {
        /// <summary>
        /// initializing 
        /// </summary>
        Initialiazing = 1,

        /// <summary>
        /// processing mapping
        /// </summary>
        Preprocesing = 2,

        /// <summary>
        /// Processing data
        /// </summary>
        Processing = 3,

        /// <summary>
        /// notify that process has finished and show process 
        /// results
        /// </summary>
        Completed = 4
    }
}
