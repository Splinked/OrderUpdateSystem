using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderUpdateSystem.Models;

namespace OrderUpdateSystem.Data
{
    public class OrderUpdateSystemContext : DbContext
    {
        public OrderUpdateSystemContext (DbContextOptions<OrderUpdateSystemContext> options)
            : base(options)
        {
        }

        public DbSet<OrderUpdateSystem.Models.Orders> Orders { get; set; } = default!;
    }
}
