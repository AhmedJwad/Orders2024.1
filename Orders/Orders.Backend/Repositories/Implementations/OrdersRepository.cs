﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using Orders.Shared.Responses;
using Org.BouncyCastle.Tls;

namespace Orders.Backend.Repositories.Implementations
{
    public class OrdersRepository :GenericRepository<Order>, IOrdersRepository
    {
        private readonly DataContext _context;
        private readonly IUsersRepository _usersRepository;

        public OrdersRepository(DataContext context, IUsersRepository usersRepository) : base(context)
        {
            _context = context;
           _usersRepository = usersRepository;
        }

        public async Task<ActionResponse<IEnumerable<Order>>> GetAsync(string email, PaginationDTO pagination)
        {
           var user=await _usersRepository.GetUserAsync(email);
            if(user==null)
            {
                return new ActionResponse<IEnumerable<Order>>
                {
                    wasSuccess = false,
                    Message = "user not exist"
                };
            }
            var queryable = _context.Orders.Include(x => x.User!)
                .Include(x => x.OrderDetails!).ThenInclude(x => x.Product).AsQueryable();
            var isAdmin = await _usersRepository.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if(!isAdmin)
            {
                queryable=queryable.Where(x=>x.User!.Email==email);  
            }
            return new ActionResponse<IEnumerable<Order>>
            {
                wasSuccess = true,
                Result = await queryable.OrderByDescending(x => x.Date).Paginate(pagination).ToListAsync()
            };
        }

        public override async Task<ActionResponse<Order>> GetAsync(int id)
        {
            var order = await _context.Orders.Include(x => x.User!)
                .ThenInclude(x => x.City!).ThenInclude(x => x.State!)
                .ThenInclude(x => x.Country).Include(x => x.OrderDetails!)
                .ThenInclude(x => x.Product).ThenInclude(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
            if(order==null)
            {
                return new ActionResponse<Order>
                {
                    wasSuccess = false,
                    Message = "product not exist"
                };
            }
            return new ActionResponse<Order>
            {
                wasSuccess = true,
                Result = order
            };
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(string email, PaginationDTO pagination)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if(user==null)
            {
                return new ActionResponse<int>
                {
                    wasSuccess = false,
                    Message = "User not exist",
                };
            }
            var queryable = _context.Orders.AsQueryable();
            var isAdmin = await _usersRepository.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if (!isAdmin)
            {
                queryable=queryable.Where(x=>x.User!.Email==email);
            }

            double count=await queryable.CountAsync();
            double totalPages =  Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                wasSuccess = true,
                Result = (int)totalPages
            };
        }

        public async Task<ActionResponse<Order>> UpdateFullAsync(string email, OrderDTO orderDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<Order>
                {
                    wasSuccess=false,
                    Message="user not exist"
                };
            }
            var isAdmin = await _usersRepository.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if(!isAdmin && orderDTO.OrderStatus!=OrderStatus.Cancelled)
            {
                return new ActionResponse<Order>
                {
                    wasSuccess = false,
                    Message = "Only allowed for administrators."
                };
            }
            var order = await _context.Orders.Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == orderDTO.Id);
            if(order ==null)
            {
                return new ActionResponse<Order>
                {
                    wasSuccess = false,
                    Message = "Request does not exist"
                };
            }
            if (orderDTO.OrderStatus == OrderStatus.Cancelled) 
            {
                await ReturnStockAsync(order);

            }
            order.OrderStatus=orderDTO.OrderStatus;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return new ActionResponse<Order>
            {
                wasSuccess = true,
                Result = order,
            };
        }

        private async Task ReturnStockAsync(Order order)
        {
            foreach (var orderDetails in order.OrderDetails!)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == orderDetails.ProductId);
                if(product != null)
                {
                    product.Stock += orderDetails.Quantity;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
