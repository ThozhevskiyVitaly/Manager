using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Abstract
{
    public interface IRepository<T> where T:class
    {
        bool Add(T obj);
        bool Edit(T obj);
        bool Remove(T obj);
        T Get(string id);
        IEnumerable<T> GetAll();
    }
}
