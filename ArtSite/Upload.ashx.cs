using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ArtSite.Controllers;
using ArtSite.DataAccess;
using MultipleUploads;
using ArtSite.Models;

namespace ArtSite
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    public class Upload : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpPostedFile hpf = context.Request.Files["file"];                
               // if (hpf.ContentLength > 0 && FileIsPicture(hpf))
                if (hpf.ContentLength > 0)
                {

                    int chunk = context.Request["chunk"] != null ? int.Parse(context.Request["chunk"]) : 0;
                    int totalChunks = context.Request["chunks"] != null ? int.Parse(context.Request["chunks"]) : 1;

                    string fileName = context.Request["name"] ?? string.Empty; 

                    string filePath = context.Server.MapPath("MyFiles");

                    using (var fs = new FileStream(Path.Combine(filePath, fileName), chunk == 0 ? FileMode.Create : FileMode.Append))
                    {
                        var buffer = new byte[hpf.InputStream.Length];
                        hpf.InputStream.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, buffer.Length);
                    }
                   
                    //only do this part when chunk complete
                    if (chunk == totalChunks-1)
                    {
                        byte[] bufferFinal = ImageProcessor.ImageAsByteArray(Path.Combine(filePath, fileName),
                                                                             System.Drawing.Imaging.ImageFormat.Jpeg);

                        using (ArtGalleryDBContext db = new ArtGalleryDBContext())
                        {
                            var currentUser =  UserDal.AllUsers.FirstOrDefault(x => x.UserName.ToLower() == context.User.Identity.Name.ToLower());                            
                            if (currentUser == null)
                            {
                                currentUser = new LogOnModel()
                                {
                                    UserId = 99999,
                                    UserName = context.User.Identity.Name
                                };
                                Logger.Info("user could not be found for upload", null);
                            }
                            var picture = new PictureItem
                                                      {
                                                          ImageT = bufferFinal,
                                                          Created = DateTime.Now,
                                                          Artist = currentUser.UserName,
                                                          UserId = currentUser.UserId
                                                      };

                            IPictureDal pictureDal = new PictureDal(db);
                            //saveto db
                            pictureDal.Enitities.Add(picture);
                            pictureDal.SaveChanges();
                            
                        }

                    }
                    

                }
                //save any old file to myFiles...
                //else
                //{
                //    string filePath = context.Server.MapPath("MyFiles") + "\\" +
                //                        System.IO.Path.GetFileName(hpf.FileName);

                //    hpf.SaveAs(filePath);
                //}

                context.Response.Write("1");
            }
            catch (Exception ex)
            {
                Logger.Error("In upload.ashx", ex);               
            }
        }

        private static bool FileIsPicture(HttpPostedFile file)
        {
            string fileName = file.FileName.ToLower();

            return fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

