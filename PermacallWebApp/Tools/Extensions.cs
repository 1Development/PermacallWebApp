
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace Tools
{
    public static class Extensions
    {
        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (InvalidCastException)
            {
                return -1;
            }

        }
        public static int ToInt(this float obj)
        {
            try
            {
                return Math.Round(obj).ToInt();
            }
            catch (InvalidCastException)
            {
                return -1;
            }

        }

        public static IHtmlContent LiActionLink(this IHtmlHelper html, string text, string action, string controller)
        {
            var context = html.ViewContext;
            var routeValues = context.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            var classString = (currentAction.Equals(action, StringComparison.InvariantCulture) && currentController.Equals(controller, StringComparison.InvariantCulture) ?
                " active" : string.Empty);
            var actionLink = html.ActionLink(text, action, controller, null, new { @class = "nav-link text-light" }); ;
             
            return new HtmlContentBuilder()
                .AppendHtml(html.Raw($"<li class=\"nav-item{classString}\" role=\"presentation\">"))
                .AppendHtml(actionLink)
                .AppendHtml(html.Raw("</li>"));
        }
    }
}