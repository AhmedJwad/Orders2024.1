using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;

        private readonly DbSet<T> _entity;
        public GenericRepository(DataContext context)
        {

            _context = context;
            _entity = _context.Set<T>();
        }
        public async Task<ActionResponse<T>> AddAsync(T Entity)
        {
            _context.Add(Entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    wasSuccess = true,
                    Result = Entity,
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

       

        public async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            var raw = await _entity.FindAsync(id);
            if(raw==null)
            {
                return new ActionResponse<T>
                {
                    wasSuccess = false,
                    Message = "Record not found",
                };

            }
            try
            {
                _context.Remove(raw);
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    wasSuccess = true,
                };
            }
            catch (Exception)
            {

                return new ActionResponse<T>
                {
                    wasSuccess = false,
                    Message = "It cannot be deleted, because it has related records.",
                };
            }
        }

        public async Task<ActionResponse<T>> GetAsync(int id)
        {
            var row=await _entity.FindAsync(id);
            if(row==null) 
            {
                return new ActionResponse<T>
                {
                    wasSuccess = false,
                    Message = "record not found",
                };
            }
            return new ActionResponse<T>
            {
                wasSuccess = true,
                Result = row,
            };
        }

        public async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                wasSuccess = true,
                Result = await _entity.ToListAsync(),
            };
        }

        public async Task<ActionResponse<T>> UpdateAsync(T Entity)
        {
            _context.Update(Entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    wasSuccess = true,
                    Result = Entity,
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }
        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                wasSuccess = false,
                Message = exception.Message,
            };
        }

        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                wasSuccess = false,
                Message = "The record you are trying to create already exists.",
            };
        }

    }
}
