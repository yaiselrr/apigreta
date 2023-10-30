namespace Greta.BO.Api.Entities.Enum
{
    public enum ActionDeviceType
    {
        REBOOT,
        CLEARCACHE,
        CLOSESESSION,
        UPDATE,
        OBTAINSTATUS,
        CLOSEAPPS
    }
    
    public enum Command
    {
        ReloadApps,
        Close,
        CleanCache,
        ForceLogOut,
        Reboot,
        IsAppOpen,
        GetInternalLogTree,
        GetLogFile,
        GetLoginEmployee,
        OpenApp
    }
}