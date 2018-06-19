using System.Web.Http;

namespace CompanyEmployeesManager.Utils
{
    public class Bootstrapper
    {
        public static void Run()
        {
            //Configure AutoFac  
            AutofacConfig.Initialize(GlobalConfiguration.Configuration);
        }
    }
}