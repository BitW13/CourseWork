using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Context;
using System.Threading.Tasks;
using CC.Filters;
using System.Data.Entity;

namespace CC.Controllers
{
    public class HomeController : Controller
    {
        //GET: Home/Index
        #region Главная страница

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        //GET: Home/Chat
        #region Лайв чат для админов

        public async Task<ActionResult> Chat()
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
        }

        #endregion
    }
}