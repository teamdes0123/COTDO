using System.Security.Claims;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;

namespace COTDO
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);         
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    var claimsData = authTicket.UserData.Split('|')
                        .Select(c => c.Split(':'))
                        .Where(parts => parts.Length == 2)
                        .Select(parts => new Claim(parts[0], parts[1]))
                        .ToList();

                    claimsData.Add(new Claim(ClaimTypes.Name, authTicket.Name));

                    var identity = new ClaimsIdentity(claimsData, "Forms");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.Current.User = principal;
                }
            }
        }
    }
}
