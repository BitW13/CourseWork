using CC.Context;
using CC.Filters;
using CC.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CC.Cryptor;
using CC.Models.Abstract;
using CC.Context.ContextModels;

namespace CC.Controllers
{
    public class OrderController : Controller
    {
        private IRepository<User> _repository;

        public OrderController(IRepository<User> repository)
        {
            _repository = repository;
        }

        // GET: Order/OrderInex
        #region Главная страница для получения валюты и билетов

        [MyAuth]
        public ActionResult OrderIndex()
        {
            return View();
        }

        #endregion

        //GET, POST: Order/GetCoins
        #region Получение валюты Coffee-Coin

        [MyAuth]
        public ActionResult GetCoffeeCoins()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var user = _repository.GetElementById(id);

            var model = new UserGetCoins() { Id = user.Id, UserCoins = user.UserCoins };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCoffeeCoins(UserGetCoins model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                if (user.Password == Encoding.GetCrypt(model.Password))
                {
                    if (model.SecretKey == "hdieo986vck4")
                    {
                        user.UserCoins = user.UserCoins + 5;

                        _repository.Update(user);

                        return RedirectToAction("OrderIndex", "Order");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный пароль");
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Order/GetTickets
        #region Получение купонов

        [MyAuth]
        public ActionResult GetTickets()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var user = _repository.GetElementById(id);

            var model = new UserGetTickets { Id = user.Id, CoffeeCoin = user.UserCoins, UserTickets = user.UserTickets };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTickets(UserGetTickets model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

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
                            _repository.Update(user);

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
                    ModelState.AddModelError("", "Неправильный пароль");
                }
            }

            return View(model);
        }

        #endregion

    }
}