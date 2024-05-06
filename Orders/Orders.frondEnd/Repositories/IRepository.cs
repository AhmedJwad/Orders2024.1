﻿

namespace Orders.frondEnd.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>>GetASync<T>(string url);
        Task<HttpResponseWrapper<object>>PostAsync<T>(string url, T model);
        Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);
        Task<HttpResponseWrapper<object>>DeleteAsync<T>(string url);
        Task<HttpResponseWrapper<object>>PutAsync<T>(string url, T model);
        Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model);
        Task<HttpResponseWrapper<object>> GetAsync(string url);


    }
}
