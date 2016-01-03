using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;

namespace SmartQueue.DAL.Repositories
{
    class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(EfDbContext context) : base(context)
        {
        }

        public Order GetByUserId(long id)
        {
            return DbSet.FirstOrDefault(o => o.UserId == id);
        }
    }
}
