using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ActionResponse<T>>GetAsync(int id);
        Task<ActionResponse<IEnumerable<T>>> GetAsync();
        Task<ActionResponse<T>> AddAsync(T Entity);
        Task<ActionResponse<T>> UpdateAsync(T Entity);
        Task<ActionResponse<T>> DeleteAsync(int id);
    }
    
}
