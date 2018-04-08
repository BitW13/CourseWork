using CC.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace CC.Filters
{
    public class MyAuthAttribute : FilterAttribute, IActionFilter
    {
        #region Фильт для проверки аунтификации пользователя 

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["LoggedIn"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
            }
        }

        //public void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    //using (var context = new UserContext())
        //    //{
        //    //    var user = context.Users;

        //    //    if (user == null)
        //    //    {
        //    //        filterContext.Result = new HttpNotFoundResult();
        //    //    }
        //    //}

        //    var user = filterContext.HttpContext.User;

        //    if (user == null || !user.Identity.IsAuthenticated)
        //    {
        //        filterContext.Result = new HttpUnauthorizedResult();
        //    }
        //}

        //public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    var user = filterContext.HttpContext.User;

        //    if (user == null || !user.Identity.IsAuthenticated)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
        //    }
        //}

        #endregion
    }
}