using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_VD.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult SetLanguage(string name)
        {
            HttpCookie cookie = HttpContext.Request.Cookies["language"];
            if (cookie != null)
            {
                ViewBag.language = cookie.Value;
            }
            return View(name);
        }
    }
}