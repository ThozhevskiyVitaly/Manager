using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Abstract
{
    public interface IEmployeesRepository:IRepository<Employee>
    {
        IEnumerable<Employee> Search(string searchString);
    }
}
