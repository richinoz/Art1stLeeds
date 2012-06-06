using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using log4net;

namespace ArtSite
{
    public static class Logger
    {
        public static void Error(string message, Exception exception)
        {
            ILog logger = log4net.LogManager.GetLogger("Database");
            logger.Error(message, exception);
        }

        public static void Info(string message, Exception exception)
        {
            ILog logger = log4net.LogManager.GetLogger("Database");
            logger.Info(message, exception);
        }

        public static void NotifySystemOps(Exception exception)
        {
            EMail.SendEmailToAdministrator("NotifySystemOps: ", exception.ToString());
        }


    }
}