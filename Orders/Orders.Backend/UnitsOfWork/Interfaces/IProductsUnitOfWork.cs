﻿using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
    public interface IProductsUnitOfWork
    {
        Task<ActionResponse<Product>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<Product>> AddFullAsync(ProductDTO productDTO);

        Task<ActionResponse<Product>> UpdateFullAsync(ProductDTO productDTO);
        Task<ActionResponse<ImageDTO>> AddImageAsync(ImageDTO imageDTO);
        Task<ActionResponse<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO);
        Task<ActionResponse<Product>> DeleteAsync(int id);


    }
}
