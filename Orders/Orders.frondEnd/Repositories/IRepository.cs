

namespace Orders.frondEnd.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>>GetASync<T>(string url);
        Task<HttpResponseWrapper<object>>PostAsync<T>(string url, T model);

        Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);
    }
}
