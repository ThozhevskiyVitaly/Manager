using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyEmployeesManager.Models
{
    public class PageEmployeesViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<EmployeeViewModel> Employees { get; set; }
    }
}