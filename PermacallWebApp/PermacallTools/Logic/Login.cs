using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Logic
{
    public class Login
    {
        public static bool ForceHTTPSConnection(HttpContext context, bool forceHTTPS)
        {
            if (!context.Request.IsLocal && !context.Request.IsSecureConnection && forceHTTPS)
            {
                string redirectUrl = context.Request.Url.ToString().Replace("http:", "https:");
                context.Response.Redirect(redirectUrl, false);
                context.ApplicationInstance.CompleteRequest();
                return false;
            }
            if (!context.Request.IsLocal && context.Request.IsSecureConnection && !forceHTTPS)
            {
                string redirectUrl = context.Request.Url.ToString().Replace("https:", "http:");
                context.Response.Redirect(redirectUrl, false);
                context.ApplicationInstance.CompleteRequest();
                return false;
            }
            return true;
        }
    }
}