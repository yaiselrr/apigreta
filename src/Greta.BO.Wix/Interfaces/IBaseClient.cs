using RestSharp;

namespace Greta.BO.Wix.Interfaces;

public interface IBaseClient
{
    ValueTask<Parameter> RefreshAccess(string refreshToken);
}