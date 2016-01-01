using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Entities
{
    public class User : IBaseEntity
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsActive { get; set; }

        public string ContentType { get; set; }

        public long? CompanyId { get; set; }

        public virtual CoffeePreferences CoffeePreferences { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
