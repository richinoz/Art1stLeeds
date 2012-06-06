using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;

namespace ArtSite
{
    public class EMail
    {
        public static string UserName { get { return "Art1stLeeds@richinoz.com"; } }
        public static string Password { get { return "B0llocks"; } }

        private const string SmtpClient = "mail.richinoz.com";// "smtp.gmail.com"; // "mail.richinoz.com";

        public static void SendEmailToAdministrator(string title, string message)
        {
            string user = !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? (HttpContext.Current.User.Identity.Name) : "unknown";
            
            EMail e = new EMail();
            e.SendEmailAsync(new MailMessage("info@ArtSite1.com", "rich__smith@hotmail.com", title, string.Format("mailed from user'{0}': {1}", user, message)));
        }

        public List<AutoResetEvent> SendEmail(MailMessage mailMessage)
        {
          
            var events = new List<AutoResetEvent>();
            var evt = new AutoResetEvent(false);
            events.Add(evt);

            ThreadPool.QueueUserWorkItem(delegate
            {
                SendEmailOnThread(mailMessage);
                evt.Set();
            });

            return events;
        }

        private void SendEmailOnThread(object mailMessage)
        {
            try
            {
                MailMessage mail = (MailMessage) mailMessage;
                SendByEmailServer(mail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public IAsyncResult SendEmailAsync(MailMessage mailMessage)
        {
            Action<MailMessage> sendEmail = SendByEmailServer;
                                               
            return sendEmail.BeginInvoke(mailMessage, SendEmailAsyncCallbackMethod, null);
           
        }

        private void SendByEmailServer(MailMessage mail)
        {            
            SmtpClient smtpServer = new SmtpClient(SmtpClient)
                                        {
                                            Port = 26,
                                            Credentials =
                                                new System.Net.NetworkCredential(EMail.UserName, EMail.Password)
                                        };

            //smtpServer.UseDefaultCredentials = false;
           // smtpServer.EnableSsl = true;
            mail.IsBodyHtml = true;
            smtpServer.Send(mail);
        }

        private void SendByEmailServerG(MailMessage mail)
        {
            SmtpClient smtpServer = new SmtpClient(SmtpClient)
                                        {
                                            Port = 587,
                                            Credentials =
                                                new System.Net.NetworkCredential("richmcsmith@gmail.com", "B0llocks"),
                                            EnableSsl = true
                                        };


            smtpServer.Send(mail);
        }


        static void SendEmailAsyncCallbackMethod(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            Action<MailMessage> caller = (Action<MailMessage>)result.AsyncDelegate;
            try
            {
                //close the thread
                caller.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CallCreditAsyncCallbackMethod failed calling EndInvoke with error: " + ex);
            }
        }


    }
}