using CC.Context;
using CC.Filters;
using CC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CC.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order/OrderInex
        public ActionResult OrderIndex()
        {
            return View();
        }

        //GET: Order/GetCoins
        [MyAuth]
        public ActionResult GetCoins(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Users.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult GetCoins(UserGetCoins model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.Id).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Password == model.Password)
                        {
                            if (model.SecretKey == "hdieo986vck4")
                            {
                                user.UserCoins = user.UserCoins + 5;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Неверный пароль");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такого пользователя не существует");
                    }
                }
            }

            return View();
        }

        //GET: Order/GetTickets
        [MyAuth]
        public ActionResult GetTickets(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Users.Where(m => m.Id == id).FirstOrDefault().UserTickets);
            }
        }

        //POST: Order/GetTickets
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult GetTickets(UserGetTickets model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.Id).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Password == model.Password)
                        {
                            if (user.UserCoins >= 2)
                            {
                                user.UserTickets = user.UserTickets + 2;
                                user.UserCoins = user.UserCoins - 2;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", "У Вас недостаточно Coffee-Coins");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Неверный пароль");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такого пользователя не существует");
                    }
                }
            }

            return View();
        }
    }
}