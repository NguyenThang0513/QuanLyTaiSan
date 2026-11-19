using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace He_thong_Quan_Ly_Tai_San_Cong_Ty
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

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current?.Request?.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        string userData = ticket.UserData ?? string.Empty;
                        string[] parts = userData.Split('|');
                        string[] roles = parts.Length > 0 && !string.IsNullOrEmpty(parts[0])
                            ? new string[] { parts[0] }
                            : new string[0];

                        var identity = new FormsIdentity(ticket);
                        var principal = new System.Security.Principal.GenericPrincipal(identity, roles);
                        HttpContext.Current.User = principal;
                        System.Threading.Thread.CurrentPrincipal = principal;
                    }
                }
                catch
                {
                    // Bỏ qua nếu giải mã cookie thất bại
                }
            }
        }
    }
}
