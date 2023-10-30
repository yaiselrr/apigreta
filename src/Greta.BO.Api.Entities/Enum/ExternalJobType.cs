namespace Greta.BO.Api.Entities.Enum
{
    public enum ExternalJobType
    {
        ScaleUpdate,
        CleanDatabase,
        FullSynchro,
        //Remove all data for this client and reboot for use on other client 
        Unassign
    }
    
    public enum ExternalJobStatus
    {
        Init,
        Sent,
        Fail,
        Complete,
        Sending,
        Processing
    }
}