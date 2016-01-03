using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Entities
{
    public class QueueItem : IBaseEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime StartDate { get; set; }

        public Order Order { get; set; }
    }
}
