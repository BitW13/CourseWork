using CC.Context;
using CC.Filters;
using CC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CC.Cryptor;

namespace CC.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order/OrderInex
        #region Главная страница для получения валюты и билетов

        public ActionResult OrderIndex()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        #endregion

        //GET, POST: Order/GetCoins
        #region Получение валюты Coffee-Coin

        //[MyAuth]
        public async Task<ActionResult> GetCoffeeCoins()
        {
            using (var context = new UserContext())
            {
                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user1 = new UserGetCoins();

                user1.Id = user.Id;
                user1.UserCoins = user.UserCoins;

                return View(user1);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetCoffeeCoins(UserGetCoins model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.Password == Encoding.GetCrypt(model.Password))
                        {
                            if (model.SecretKey == "hdieo986vck4")
                            {
                                user.UserCoins = user.UserCoins + 5;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("OrderIndex", "Order");
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

            return View(model);
        }

        #endregion

        //GET, POST: Order/GetTickets
        #region Получение купонов

        //[MyAuth]
        public async Task<ActionResult> GetTickets()
        {
            using (var context = new UserContext())
            {
                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user1 = new UserGetTickets { UserTickets = user.UserTickets, Id = user.Id, CoffeeCoin = user.UserCoins };

                return View(user1);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetTickets(UserGetTickets model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.Password == Encoding.GetCrypt(model.Password))
                        {
                            if (user.UserCoins >= 2)
                            {
                                user.UserTickets = user.UserTickets + model.UserTickets;
                                user.UserCoins = user.UserCoins - (model.UserTickets * 2);

                                if (user.UserCoins < 0)
                                {
                                    ModelState.AddModelError("", "Вы не можете купить столько купонов, у Вас не хватает средств");
                                }
                                else
                                {
                                    context.Entry(user).State = EntityState.Modified;
                                    context.SaveChanges();

                                    return RedirectToAction("OrderIndex", "Order");
                                }
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

            return View(model);
        }

        #endregion
    }
}