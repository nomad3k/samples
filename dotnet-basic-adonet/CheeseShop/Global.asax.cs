﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;

namespace CheeseShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            _container = ControllersInstaller.Register();
        }

        protected void Application_End()
        {
            _container.Dispose();
        }
    }
}
