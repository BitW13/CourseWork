using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Context;
using System.Threading.Tasks;
using System.Data.Entity;
using CC.Models;

namespace CC.Controllers
{
    public class HomeController : Controller
    {
        //GET: Home/Index
        #region Главная страница

        public async Task<ActionResult> Index()
        {
            using (var context = new UserContext())
            {
                var list = await context.Records.ToListAsync();

                list.Reverse();

                return View(list);
            }
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

                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
        }

        #endregion

        //GET: Home/PriceList
        #region Прайс-лист

        public ActionResult PriceList()
        {
            return View();
        }

        #endregion
    }
}