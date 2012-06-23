using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtSite.Controllers;
using ArtSite.DataAccess;
using ArtSite.Filters;
using ArtSite.Models;
using MvcMiniProfiler;

namespace ArtSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterCustomFilters()
        {
            IEnumerable<Func<ControllerContext, ActionDescriptor, object>> conditions =
                new Func<ControllerContext, ActionDescriptor, object>[] { 
    
             (c, a) => c.Controller.GetType() != typeof(HomeController) ? 
                                    new AdminPermissionsAttribute() : null,
                (c, a) => a.ActionName.StartsWith("About") ? new AdminPermissionsAttribute() : null
        };

            var provider = new ConditionalFilterProvider(conditions);
            FilterProviders.Providers.Add(provider);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var allUsers = UserDal.AllUsers;

            foreach (var user in allUsers)
            {
                var userName = user.UserName.Split(' ');

                routes.MapRoute(
                  user.UserName, // Route name
                  string.Format("{0}/{{*extra}}", user.UserName), // URL with parameters
                  new { controller = "Gallery", action = "ArtistGallery", userId = user.UserId });

                if (userName.Count() > 1)
                {
                    routes.MapRoute(
                    userName[0] + "1", // Route name
                    string.Format("{0}/{{*extra}}", userName[0]), // URL with parameters
                    new { controller = "Gallery", action = "ArtistGallery", userId = user.UserId });
                }

                routes.MapRoute(
                  userName[0] + "Galleria" + user.UserId, // Route name
                  string.Format("SlideShow/{0}", userName[0]), // URL with parameters
                  new { controller = "Gallery", action = "Galleria", artistId = user.UserId });

                routes.MapRoute(
                  userName[0] + "BackGround" + user.UserId, // Route name
                  string.Format("Background/{0}", userName[0]), // URL with parameters
                  new { controller = "Gallery", action = "ArtistHome", userId = user.UserId });

                routes.MapRoute(
                 userName[0] + "Contact" + user.UserId, // Route name
                 string.Format("Contact/{0}", userName[0]), // URL with parameters
                 new { controller = "Email", action = "SendEmail", artistId = user.UserId });

                //routes.MapRoute(
                // userName[0] + "ArtistGallery" + user.UserId, // Route name
                // string.Format("Gallery/ArtistGallery/{0}", userName[0]), // URL with parameters
                // new { controller = "Gallery", action = "ArtistGallery", userId = user.UserId });
            }

            routes.MapRoute(
              "Home", // Route name
              "Home/{action}/{*id}", // URL with parameters
              new { controller = "Home", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
               "Download", // Route name
               "Download/{fileName}", // URL with parameters
               new { controller = "Download", action = "Index", fileName = UrlParameter.Optional } // Parameter defaults
           );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "LandingPage", id = UrlParameter.Optional } // Parameter defaults
            );



        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Database.SetInitializer<ArtGalleryDBContext>(new RecreateDatabaseIfModelChanges<ArtGalleryDBContext>());

            // RegisterCustomFilters();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            log4net.Config.XmlConfigurator.Configure();

            Logger.Info("start application", null);
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();

            // Handle HTTP errors
            if (exc.GetType() == typeof(HttpException))
            {
                // The Complete Error Handling Example generates
                // some errors using URLs with "NoCatch" in them;
                // ignore these here to simulate what would happen
                // if a global.asax handler were not implemented.
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;

                //Redirect HTTP errors to HttpError page
                //Server.Transfer("HttpErrorPage.aspx");
                return;
            }

            // For other kinds of errors give the user some information
            // but stay on the default page
            Response.Write("<h2>Global Page Error</h2>\n");
            Response.Write(
                "<p>" + exc.Message + "</p>\n");
            Response.Write(string.Format("Return to the <a href='/Home/Index'>" + "Default Page</a>\n"));

            // Log the exception and notify system operators
            Logger.Error("DefaultPage", exc);
            // Logger.NotifySystemOps(exc);

            // Clear the error from the server
            Server.ClearError();
        }


        protected void Session_Start()
        {
            HitCounter.IncrementHitCounter();
        }
    }
}