namespace Greta.BO.Api.Core.Caching
{
    public interface ICacheable
    {
        string CacheKey { get; }
    }
}