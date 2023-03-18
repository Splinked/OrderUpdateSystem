using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderUpdateSystem.Data;
using System;
using System.Linq;

namespace OrderUpdateSystem.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OrderUpdateSystemContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OrderUpdateSystemContext>>()))
            {
                // Look for any orders.
                if (context.Orders.Any())
                {
                    return;   // DB has been seeded
                }
                context.Orders.AddRange(
                    new Orders
                    {
                        //Id = 001,
                        OrderUpdateDate = DateTime.Now,
                        Status = "New",
                        OperatorId = 101
                    },
                    new Orders
                    {
                        //Id = 002,
                        OrderUpdateDate = DateTime.Now,
                        Status = "New",
                        OperatorId = 101
                    },
                    new Orders
                    {
                        //Id = 003,
                        OrderUpdateDate = DateTime.Now,
                        Status = "New",
                        OperatorId = 102
                    },
                    new Orders
                    {
                        //Id = 004,
                        OrderUpdateDate = DateTime.Now,
                        Status = "New",
                        OperatorId = 102
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
