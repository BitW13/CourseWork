using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Filters;

namespace CC.Controllers
{
    public class HomeController : Controller
    {
        #region Главная страница

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}