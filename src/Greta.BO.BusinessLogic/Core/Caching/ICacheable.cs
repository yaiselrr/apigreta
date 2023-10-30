namespace Greta.BO.BusinessLogic.Core.Caching
{
    public interface ICacheable
    {
        string CacheKey { get; }
    }
}