using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtSite.Controllers;
using ArtSite.Models;

namespace ArtSite.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {            
            AccountController accountController = new AccountController(null);

            LogOnModel logOnModel = new LogOnModel(){Password = "bleakhouse", UserName = "ellie mcverry"};
            var ret =  accountController.LogOn(logOnModel, null);

        }
    }
}
