using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ArtSite.DataAccess;
using ArtSite.Models;

namespace ArtSite.Controllers
{
    public class AccountController : Controller
    {
        private IUserDal _userDal;

        public AccountController(IUserDal userDal)
        {
            _userDal = userDal;
        }
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                //if (ValidateUser(model.UserName, model.Password))
                {
                    //check that user has logonModel record too!
                    var logOnModel = UserDal.AllUsers.FirstOrDefault(x => x.UserName == model.UserName);

                    if (logOnModel == null)
                    {
                        CreateUser(model.UserName, model.Email);
                    }
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
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
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public bool ValidateUser(string userName, string password)
        {
            var user = _userDal.Enitities.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
            if (user != null)
                if (user.UserName.ToLower() == userName.ToLower() && user.Password == password)
                    return true;

            return false;
        }


        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    var user = CreateUser(model.UserName, model.Email);

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", ErrorCodeToString(createStatus));


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private LogOnModel CreateUser(string name, string email)
        {
            name = name.ToLower();
            var user = new LogOnModel()
            {
                UserName = name,
                Password = "Bogus-exists in AspNet db",
                Email = email,
                Permissions = "0",
                Active = false
            };

            _userDal.Enitities.Add(user);
            try
            {
                _userDal.SaveChanges();
            }
            catch (Exception ex)
            {
                var u = _userDal.Enitities.FirstOrDefault(x => x.UserName == name);
                if (u != null)
                {
                    Logger.Error("user already exists in CreateUser", ex);
                }
                else
                    throw;

            }

            return user;
        }
        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception ex)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgottenPassword(LogOnModel logOnModel)
        {
            try
            {

                var currentUser = Membership.GetUser(logOnModel.UserName);

                if (currentUser != null)
                {
                    
                    var password = currentUser.ResetPassword();

                    var emailSender = new EMail();

                    var body = string.Format("Hi {0}!\r\nYour new password is {1}", currentUser.UserName, password);

                    var mailMessage = new MailMessage("noreply@artsite.com", currentUser.Email, "ArtSite details",
                                                              body);
                    var result = emailSender.SendEmailAsync(mailMessage);

                    return RedirectToAction("PassWordSent", new { emailAddress = currentUser.Email });


                }
                else
                {
                    ModelState.AddModelError("", string.Format("'{0}' is not registered", logOnModel.Email));                    
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
              
            }
            return View();
        }

        public ActionResult PasswordSent(string emailAddress)
        {
            ViewBag.EMail = emailAddress;
            return View();
        }

       

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
