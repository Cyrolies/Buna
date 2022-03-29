using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace BunaPortal.HTMLHelpers
{
    public class SessionTimeoutAttribute : System.Web.Mvc.ActionFilterAttribute
	{
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login"); //"Login", "Account"
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}