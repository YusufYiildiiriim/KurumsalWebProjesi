using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KurumsalWebProjesi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            string lang = HttpContext.Current.Request.QueryString["lang"];

    
            if (!string.IsNullOrEmpty(lang))
            {
                HttpCookie langCookie = new HttpCookie("lang", lang);
                langCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            else
            {
               
                HttpCookie langCookie = HttpContext.Current.Request.Cookies["lang"];
                lang = langCookie != null ? langCookie.Value : "tr"; // default
            }

          
            try
            {
                var culture = new System.Globalization.CultureInfo(lang);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch
            {
                var defaultCulture = new System.Globalization.CultureInfo("tr");
                System.Threading.Thread.CurrentThread.CurrentCulture = defaultCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = defaultCulture;
            }

        }


    }
}
