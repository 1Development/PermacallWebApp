using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Logic
{
    public class Login
    {
        public static void ForceHTTPSConnection(HttpContext context, bool forceHTTPS)
        {
            if (!context.Request.IsLocal && !context.Request.IsSecureConnection && forceHTTPS)
            {
                string redirectUrl = context.Request.Url.ToString().Replace("http:", "https:");
                context.Response.Redirect(redirectUrl, false);
                context.ApplicationInstance.CompleteRequest();
            }
            if (!context.Request.IsLocal && context.Request.IsSecureConnection && !forceHTTPS)
            {
                string redirectUrl = context.Request.Url.ToString().Replace("https:", "http:");
                context.Response.Redirect(redirectUrl, false);
                context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}