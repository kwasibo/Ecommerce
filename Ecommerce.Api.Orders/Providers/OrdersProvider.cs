using AutoMapper;
using Ecommerce.Api.Orders.Db;
using Ecommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
       public async Task<(bool IsSucess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();

                if (orders != null && orders.Any())
                {
                    foreach(Order order in orders)
                    {
                        order.Items = dbContext.OrderItems.Where(i => i.OrderId == order.Id).ToList();
                    }
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }


        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                
                dbContext.Orders.Add(new Db.Order() { Id = 1, CustomerId= 1, OrderDate = DateTime.Now, Total = 100 });
                dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 2, OrderDate = DateTime.Now.AddDays(-2), Total = 50 });

                dbContext.OrderItems.Add(new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 100 });
                dbContext.OrderItems.Add(new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 100 });
                dbContext.OrderItems.Add(new OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 100 });

                dbContext.SaveChanges();
            }

        }
    }
}
