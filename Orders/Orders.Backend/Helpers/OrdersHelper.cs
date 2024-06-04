using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using Orders.Shared.Responses;

namespace Orders.Backend.Helpers
{
    public class OrdersHelper : IOrdersHelper
    {
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly ITemporalOrdersUnitOfWork _temporalOrdersUnitOfWork;
        private readonly IOrdersUnitOfWork _ordersUnitOfWork;
        private readonly IProductsUnitOfWork _productsUnitOfWork;

        public OrdersHelper(IUsersUnitOfWork usersUnitOfWork, ITemporalOrdersUnitOfWork temporalOrdersUnitOfWork,
            IOrdersUnitOfWork ordersUnitOfWork, IProductsUnitOfWork productsUnitOfWork)
        {
           _usersUnitOfWork = usersUnitOfWork;
           _temporalOrdersUnitOfWork = temporalOrdersUnitOfWork;
           _ordersUnitOfWork = ordersUnitOfWork;
           _productsUnitOfWork = productsUnitOfWork;
        }
        public async Task<ActionResponse<bool>> ProcessOrderAsync(string email, string remarks)
        {
           var user=await _usersUnitOfWork.GetUserAsync(email);
            if(user==null)
            {
                return new ActionResponse<bool>
                { wasSuccess = false , Message= "Invalid user" };
            }
            var actionTemporalOrders=await _temporalOrdersUnitOfWork.GetAsync(email);
            if (!actionTemporalOrders.wasSuccess)
            {
                return new ActionResponse<bool>
                {
                    wasSuccess = false,
                    Message= "There is no detail in the order"
                };
            }
            var temporalOrders = actionTemporalOrders.Result as List<TemporalOrder>;
            var response = await CheckInventoryAsync(temporalOrders!);
            if(!response.wasSuccess)
            {
                return response;
            }
            var order = new Order
            {
                Date = DateTime.UtcNow,
                User = user,
                OrderDetails = new List<OrderDetail>(),
                Remarks = remarks,
                OrderStatus = OrderStatus.New,
            };

            foreach (var item in temporalOrders!)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    Product=item.Product,
                    Quantity=item.Quantity,
                    Remarks=item.Remarks,
                });
                var actionProduct = await _productsUnitOfWork.GetAsync(item.Product!.Id);
                if(actionProduct.wasSuccess)
                {
                    var product = actionProduct.Result;
                    if(product!=null)
                    {
                        product.Stock -= item.Quantity;
                        await _productsUnitOfWork.UpdateAsync(product);
                    }
                }
                await _temporalOrdersUnitOfWork.DeleteAsync(item.Id);
            }
            await _ordersUnitOfWork.AddAsync(order);
            return response;
           
        }

        private async Task<ActionResponse<bool>> CheckInventoryAsync(List<TemporalOrder> temporalOrders)
        {
            var response = new ActionResponse<bool>() { wasSuccess = true };
            foreach (var item in temporalOrders)
            {
                var actionProduct = await _productsUnitOfWork.GetAsync(item.Product!.Id);
                if(!actionProduct.wasSuccess)
                {
                    response.wasSuccess = false;
                    response.Message= $"The product {item.Product!.Id} is no longer available";
                    return response;
                }
                var product = actionProduct.Result;
                if(product==null)
                {
                    response.wasSuccess = false;
                    response.Message= $"The product {item.Product!.Id} is no longer available";
                    return response;
                }
                if(product.Stock <item.Quantity)
                {
                    response.wasSuccess = false;
                    response.Message= $"Sorry, we do not have enough stock of the product " +
                        $"{item.Product!.Name}" +
                        $" to take your order. Please reduce the quantity or replace it with another.";
                }
            }
            return response;
        }
    }
}
