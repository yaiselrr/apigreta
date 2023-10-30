namespace Greta.BO.Wix.Models;

public class WixOptions
{
    public string  Url { get; set; }
    public string?  ClientId { get; set; }
    public string?  ClientSecret { get; set; }
    public string?  WebHookApiPublicKey { get; set; }
    public string?  UrlShared { get; set; }
    public string?  RedirectUrl { get; set; }
}