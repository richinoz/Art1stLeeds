using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtSite.DataAccess;
using ArtSite.Models;

namespace ArtSite.Filters
{
    class AdminPermissions:IPermissions
    {

        public string Values
        {
            get { return "99"; }
        }
    }

    public class AdminPermissionsAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(base.AuthorizeCore(httpContext))
            {
                if(Permissions.HasPermission(httpContext.User, new AdminPermissions()))
                    return true;

                var url = httpContext.Request.Url;
                var final = "http://" + url.Authority + @"/account/logon";
                httpContext.Response.Redirect(final);
            }

            return false;

        }
       
    }

}