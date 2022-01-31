using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        //ghi de

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if(Session[CommomConstants.CurrentCulture] != null) //mếu khác null thì nó set cho mặc định
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session[CommomConstants.CurrentCulture].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session[CommomConstants.CurrentCulture].ToString());
            }
            else
            {
                Session[CommomConstants.CurrentCulture] = "vi";//gans session
                Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
            }    
        }

        //changing culture  
        public ActionResult ChangeCulture(string ddCulture,string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddCulture);

            Session[CommomConstants.CurrentCulture] = ddCulture;
            return Redirect(returnUrl);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommomConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { Controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetAlert(string message,string type)
        {
            TempData["AlertMessage"] = message;
            if(type == "success")
            {
                TempData["AlertType"] = "alert-success";

            }else if(type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if(type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}