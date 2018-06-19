using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Reflection;
using DataLayer.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using CompanyEmployeesManager.Controllers;
using DataLayer;
using DataLayer.Abstract;

namespace CompanyEmployeesManager.Utils
{
    public class AutofacConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EmployeesContext>()
                .As<DbContext>()
                .WithParameter("connectionString", "EmployeesContext")
                .InstancePerRequest();
            builder.RegisterType<EmployeesManagerServiceLogger>()
                .As<IServiceLogger>()
                .WithParameter("name", "EmployeeManagerService")
                .InstancePerRequest();
            builder.RegisterType<EmployeesController>();
            builder.RegisterType<EmployeesRepository>()
                   .As<IEmployeesRepository>();
 
            Container = builder.Build();
            return Container;
        }
    }
}