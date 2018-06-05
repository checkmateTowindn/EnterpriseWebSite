using LoginAndAuthority.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseWebSite.Web.Controllers
{
    public class AdminIndexController : Controller
    {
        [Login]
        public ActionResult Index()
        {
            return View();
        }
        [Login]
        public ActionResult AddOrEdit()
        {
            return View();
        }
    }
}