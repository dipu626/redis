namespace Caching.Domain.Repositories
{
    public interface ICacheRepository
    {
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, DateTimeOffset expirationTime);
        Task<object> DeleteAsync(string key);
        Task DeleteAllAsync();
    }
}
