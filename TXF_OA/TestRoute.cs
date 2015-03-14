using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace TXF_OA
{
    public class TestRoute : RouteBase
    {
        private string[] urls;
        public TestRoute(params string[] targetUrls)
        {
            urls = targetUrls;
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData result = null;
            string requestedURL =
            httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;
            requestedURL = requestedURL.Substring(2).Trim('/');

            if (requestedURL.Contains(urls.ToArray().GetValue(0).ToString()))
            {
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Account");
                result.Values.Add("action", "Login");
                result.Values.Add("p", requestedURL);
            }
            return result;
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}