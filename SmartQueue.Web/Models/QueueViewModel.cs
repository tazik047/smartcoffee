using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQueue.Web.Models
{
    public class QueueViewModel
    {
        public TimeSpan TimeToEnd { get; set; }

        public IEnumerable<UserQueueViewModel> Users { get; set; }
    }
}