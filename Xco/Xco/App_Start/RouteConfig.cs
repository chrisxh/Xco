using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xco
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region API Routes
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"\d+" } 
            );
            
            routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}"
            );
            #endregion


            #region MVC Routes
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Redirect",
                "{ShortenedUrlPath}", // Short url segment
                defaults: new { controller = "Home", action = "Forward", ShortenedUrlPath = RouteParameter.Optional }, // Send urls to the Redirect action of HomeController
                constraints: new { ShortenedUrlPath = @"^(?!api)\w+$" } //ignore incoming "/api*"-type requests
            );
            #endregion

        }
    }
}