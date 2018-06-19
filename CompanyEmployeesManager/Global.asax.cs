using AutoMapper;
using CompanyEmployeesManager.Models;
using CompanyEmployeesManager.Utils;
using DataLayer.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace CompanyEmployeesManager
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Employee, EmployeeViewModel>();
                cfg.CreateMap<EmployeeViewModel, Employee>();
                cfg.CreateMap<EmployeeCreateViewModel, Employee>();
                cfg.AllowNullCollections = true;
                cfg.CreateMap<IEnumerable<EmployeeViewModel>, IEnumerable<Employee>>();
            });
            Bootstrapper.Run();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
