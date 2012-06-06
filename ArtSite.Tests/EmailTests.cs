using System;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtSite.Tests
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void ShouldSendEmail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("rich@gmail.com");
            mail.To.Add("rich__smith@hotmail.com");
            mail.Subject = "Test Mail" + DateTime.Now.ToString("hh:mm:ss");
            mail.Body = "This is for testing SMTP mail";

            EMail eMail = new EMail();
            

            var events = eMail.SendEmail(mail);

            // wait for all emails to signal
            WaitHandle.WaitAll(events.ToArray());
           // 

        }

        [TestMethod]
        public void ShouldSendEmailAsync()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("rich@gmail.com");
            mail.To.Add("rich__smith@hotmail.com");
            mail.Subject = "Test Mail" + DateTime.Now.ToString("hh:mm:ss");
            mail.Body = "This is for testing SMTP mail";

            EMail eMail = new EMail();


            var ret= eMail.SendEmailAsync(mail);

            // wait for all emails to signal
            // 
            while(!ret.IsCompleted)
            {}

        }
    }
}
