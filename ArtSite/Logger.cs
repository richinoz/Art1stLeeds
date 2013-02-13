using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ArtSite.Helpers;
using Elmah;
using log4net;

namespace ArtSite
{
    public static class Logger
    {
        public static void Error(string message, Exception exception)
        {
            ILog logger = LogManager.GetLogger("Database");
            logger.Error(message, exception);
        }

        public static void Info(string message, Exception exception)
        {
            ILog logger = LogManager.GetLogger("Database");
            logger.Info(message, exception);

            LogInfoToElmah(message);            
        }

        public static void NotifySystemOps(Exception exception)
        {
            EMail.SendEmailToAdministrator("NotifySystemOps: ", exception.ToString());
        }

        private static void LogInfoToElmah(string message)
        {
            var title = ConfigurationManager.AppSettings["Title"];

            var path = HttpContext.Current.Server.MapPath("App_Data");

            var errorLog = new Elmah.XmlFileErrorLog(path) { ApplicationName = title };

            var logException = new ElmahInfoException(String.Format("{0}", message));
            errorLog.Log(new Elmah.Error(logException));
        }
    }
}