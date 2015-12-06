using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);

        void Edit(T item);

        void Delete(T item);

        IEnumerable<T> Get();

        T Get(long id);

        IEnumerable<T> Get(Func<T, bool> predicat);
    }
}
