using CC.Models;
using CC.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CC.Filters;
using System.Threading.Tasks;
using CC.Cryptor;

namespace CC.Controllers
{
    public class AccountController : Controller
    {
        // GET, POST: Account/Create
        #region Регистрация

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateModel model)
        {
            Session["Id"] = null;
            Session["UserRole"] = null;
            Session["AdminRole"] = null;
            Session["ModerRole"] = null;

            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.NickName == model.NickName && m.UserName == model.UserName && m.UserSurname == model.UserSurname).FirstOrDefaultAsync();

                    if (user == null)
                    {
                        if (model.Password == model.ConfirmPassword)
                        {
                            var newUser = new User { NickName = model.NickName, UserName = model.UserName, UserSurname = model.UserSurname, Password = Encoding.GetCrypt(model.Password), UserRoleName = "User", UserTickets = 0, UserCoins = 2 };

                            context.Users.Add(newUser);
                            context.SaveChanges();

                            Session["Id"] = newUser.Id;
                            Session["UserRole"] = "User";

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пароли не совпадают");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такой пользователь уже существует");
                    }
                }
            }

            return View(model);
        }
        #endregion

        //GET, POST: Account/Login
        #region Авторизация

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel model)
        {
            Session["Id"] = null;
            Session["UserRole"] = null;
            Session["AdminRole"] = null;
            Session["ModerRole"] = null;

            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.NickName == model.NickName && m.UserName == model.UserName && m.UserSurname == model.UserSurname).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.Password == Encoding.GetCrypt(model.Password))
                        {
                            if (user.UserRoleName == "User")
                            {
                                Session["Id"] = user.Id.ToString();
                                Session["UserRole"] = user.UserRoleName;

                                return RedirectToAction("Index", "Home");
                            }
                            else if (user.UserRoleName == "Admin")
                            {
                                Session["Id"] = user.Id.ToString();
                                Session["AdminRole"] = user.UserRoleName;

                                return RedirectToAction("Index", "Home");
                            }
                            else if (user.UserRoleName == "Moder")
                            {
                                Session["Id"] = user.Id.ToString();
                                Session["ModerRole"] = user.UserRoleName;

                                return RedirectToAction("Index", "Home");
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

        //POST: Account/Logout
        #region Выход из аккаунта

        public ActionResult Logout()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Logout(int? id)
        {
            Session["Id"] = null;
            Session["UserRole"] = null;
            Session["AdminRole"] = null;
            Session["ModerRole"] = null;

            return RedirectToAction("Index", "Home");
        }
        #endregion

        //GET, POST: Account/GetAdmin
        #region Получение прав администратора

        //[MyAuth]
        public ActionResult GetAdmin()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetAdmin(UserGetRightsModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    int id = int.Parse(Session["Id"].ToString());

                    var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.Password == Encoding.GetCrypt(model.Password))
                        {
                            if (model.SecurityCode == "qw12po09fj")
                            {
                                user.UserRoleName = "Admin";

                                Session["AdminRole"] = user.UserRoleName;

                                Session["UserRole"] = null;
                                Session["ModerRole"] = null;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Неправильный защитный код");
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

        //GET, POST: Account/GetModer
        #region Получение прав модератора

        //[MyAuth]
        public ActionResult GetModer()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetModer(UserGetRightsModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    int id = int.Parse(Session["Id"].ToString());

                    var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.Password == Encoding.GetCrypt(model.Password))
                        {
                            if (model.SecurityCode == "bmd78zl4r1")
                            {
                                user.UserRoleName = "Moder";

                                Session["ModerRole"] = user.UserRoleName;

                                Session["UserRole"] = null;
                                Session["AdminRole"] = null;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Неправильный защитный код");
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