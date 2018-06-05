using smileRed.Backend.Helpers;
using smileRed.Backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace smileRed.Backend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.CheckRolesAndSuperUser();
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.LocalDataContext, Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckSuperUser();
        }
    }
}
