using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Entities
{
    public class CoffeePreferences : IBaseEntity
    {
        [Key, ForeignKey("User")]
        public long Id { get; set; }

        public Size Size { get; set; }

        public int Sugar { get; set; }

        public DrinkType Drink { get; set; }

        public virtual User User { get; set; }
    }
}
