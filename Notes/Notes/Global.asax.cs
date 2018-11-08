using Notes.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Notes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //инициализируем базу данных
            //Database.SetInitializer(new UserDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
