using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Implementations
{
    public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
    {
        private readonly IGenericRepository<T> _genericRepository;

        public GenericUnitOfWork(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public virtual async Task<ActionResponse<T>> AddAsync(T entity)=> await _genericRepository.AddAsync(entity);       

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)=>await _genericRepository.DeleteAsync(id);        

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()=>await _genericRepository.GetAsync();        

        public virtual async Task<ActionResponse<T>> GetAsync(int id)=>await _genericRepository.GetAsync(id);        

        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)=> await _genericRepository.UpdateAsync(entity);
        
    }
}
