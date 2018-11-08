using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Notes.Filters
{
    public class AdminAuthorize : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var SessionAdminKey = filterContext.HttpContext.Session["AdminKey"];
            if (SessionAdminKey != null)
                if (SessionAdminKey.ToString() == Config.PrKey)
                    return;

            filterContext.Result = new HttpUnauthorizedResult();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

            var SessionAdminKey = filterContext.HttpContext.Session["AdminKey"];
            if (SessionAdminKey != null)
                if (SessionAdminKey.ToString() == Config.PrKey)
                    return;

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
               new { action = "Index", controller = "Admin" }));

        }
    }

}