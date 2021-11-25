using System.Web.Mvc;
using System.Net.Http;
using System.Web.Caching;
using Common;
using System.Text;
using DALEFModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;
using CyroTechPortal.HTMLHelpers;
using System.Web;

namespace CyroTechPortal
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Home()
        {
            return View();
        }
        
        public ActionResult Hitec()
        {
            CommonHelper.CacheClear();
            CommonHelper.CacheAdd("DefaultTheme", "~/Content/css/Hitec-DarkYellow.css");
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
			if (session["User"] != null)
			{
               session["User"] = null;
            }
            return RedirectToAction("Login", "Account", new { title = "Hitec Login" });
        }
        public ActionResult ThornFarm()
        {
            CommonHelper.CacheClear();
            CommonHelper.CacheAdd("DefaultTheme", "~/Content/css/ThornFarm.css");
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            if (session["User"] != null)
            {
                session["User"] = null;
            }
            return RedirectToAction("Login", "Account",new { title = "ThornFarm Login" });
            
        }
    }
}
