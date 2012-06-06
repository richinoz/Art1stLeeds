using System;
using System.Data.Entity;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtSite.DataAccess;
using ArtSite.Models.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtSite.Controllers;
using ArtSite.Models;
using MvcContrib.TestHelper;
using MvcContrib.TestHelper.Fakes;
using Rhino.Mocks;

namespace ArtSite.Tests.Controllers
{
    [TestClass]
    public class PictureLoaderControllerTests
    {
        [TestInitialize]
        public void setup()
        {
         
        }
        [TestMethod]
        public void ShouldGetImagesForLandingPage()
        {
            var landingPageItemViewModels = PictureEditorController.GetImagesForLandingPage();
            Assert.IsNotNull(landingPageItemViewModels);
        }

        [TestMethod]
        public void GetImagesFromDbShouldReturnList()
        {

            var ret = PictureEditorController.GetImagesFromDb( "");

            Assert.IsNotNull(ret);
        }

        [TestMethod]
        public void GetImagesFromDbShouldReturnPicturesWithATitle()
        {
            var ret = PictureEditorController.GetImagesFromDb("!untitled");

            var t = ret.Where(x => x.Title == null);

            Assert.IsTrue(t.Count()==0);
        }

        [TestMethod]
        public void GetImagesFromDbShouldReturnPicturesWithoutATitle()
        {
            var ret = PictureEditorController.GetImagesFromDb("untitled").ToList();

            var t = ret.Where(x => string.IsNullOrEmpty(x.Title));

            Assert.IsTrue(t.Count() == ret.Count());
        }

        [TestMethod]
        public void GetImagesFromDbShouldReturnAllPictures()
        {
            ArtGalleryDBContext artGalleryDb = new ArtGalleryDBContext();
            PictureDal pictureDal = new PictureDal(artGalleryDb);

            var ret = PictureEditorController.GetImagesFromDb(null);

            Assert.AreEqual(ret.Count(), pictureDal.Enitities.Count());
        }

        [TestMethod]
        public void ShouldReturnArtist()
        {
            ArtGalleryDBContext artGalleryDb = new ArtGalleryDBContext();
            PictureDal pictureDal = new PictureDal(artGalleryDb);

            var y = pictureDal.Enitities.Where(x => x.UserId == 4);

        }

        [TestMethod]
        public void ShouldReturnAllImagesForArtist()
        {
            //userId 3 = paul
            var ret = PictureEditorController.GetImagesFromDbMin("paul");
            foreach (var pictureItemNoBufferData in ret)
            {
                pictureItemNoBufferData.Artist = null;
            }

            Assert.IsTrue(ret.Count>0);
        }

        [TestMethod]
        public void DeleteShouldBeSuccessful()
        {
            var ret = PictureEditorController.GetImagesFromDbMin("");

            Assert.IsTrue(ret.Count > 0);
        }
    }
}
