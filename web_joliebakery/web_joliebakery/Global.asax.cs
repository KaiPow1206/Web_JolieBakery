using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using web_joliebakery.Models;

namespace web_joliebakery
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            cart cart = new cart();
            Session["CART"] = cart;
            Session["UNIT"] = 0;
        }
        protected void Session_End()
        {
            Session.Remove("CART");
            Session.Remove("UNIT");
        }
    }
}
