using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ArtSite.DataAccess;
using ArtSite.Models;

namespace ArtSite.Controllers
{
    public class EmailController : Controller
    {
        //
        // GET: /Email/


        public ActionResult SendEmail(long artistId)
        {
            IUserDal userDal = new UserDal(new ArtGalleryDBContext());
            var user = userDal.Enitities.Find(artistId);

            EMailView eMailView = new EMailView() {  LogOnModel = user};
            
            ViewBag.Menu = "_ArtistHome";
            ViewBag.Model = user;
            ViewBag.Title = user.UserName;

            return View(eMailView);
        }

        public ActionResult SendEmailByName(string artistName)
        {
            var db = new ArtGalleryDBContext();
            var artist = Helpers.Helpers.GetUserForName(artistName,db);            

            EMailView eMailView = new EMailView() { LogOnModel = artist };
            ViewBag.Menu = "_ArtistHome";
            ViewBag.Model = artist;
            ViewBag.Title = artist.UserName;

            return View("SendEmail", eMailView);

        }


        [HttpPost]
        public ActionResult SendEmail(EMailView model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
               
                EMail eMail = new EMail();
                try
                {
                    //make sure to address is ok (as this set by system not user)
                    Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                    if (model.LogOnModel.Email == null)
                    {
                        ModelState.AddModelError("", "The 'To' address is blank");

                    }
                    else
                    {

                        var match = regex.Match(model.LogOnModel.Email);
                        if (match.Success)
                        {
                            string message = string.Format("Message from {0}\n{1}", model.EmailFrom, model.Message);

                            eMail.SendEmailAsync(new MailMessage(EMail.UserName, model.LogOnModel.Email,
                                              "Do Not Reply - Art1st site contact from " + model.EmailFrom, message));

                        }
                        else
                        {
                            //log message sent
                            string message = string.Format("{0} /n from {1}", model.Message, model.EmailFrom);

                            Logger.Error("Failed to send email to " + model.LogOnModel.UserName, null);
                            EMail.SendEmailToAdministrator("Failed to send email to " + model.LogOnModel.UserName,
                                                           message);
                        }
                        //do stuff
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                catch(FormatException ex)
                {
                    ModelState.AddModelError("", "Please enter a valid email address");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error in sendEmail", ex);
                    ModelState.AddModelError("", ex.ToString());
                }
              
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
