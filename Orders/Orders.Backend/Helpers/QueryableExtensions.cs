using Orders.Shared.DTOs;
using System.Runtime.CompilerServices;

namespace Orders.Backend.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable.Skip((paginationDTO.Page - 1) * paginationDTO.RecordsNumber).Take(paginationDTO.RecordsNumber);
        }
    }
}
