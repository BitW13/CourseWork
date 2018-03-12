using CC.Models;
using CC.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CC.Filters;

namespace CC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName && m.UserName == model.UserName && m.UserSurname == model.UserSurname).FirstOrDefault();

                    if (user == null)
                    {
                        context.Users.Add(new User { NickName = model.NickName, UserName = model.UserName, UserSurname = model.UserSurname, Password = model.Password, UserRoleName = "User", UserTickets = 0 });
                        context.SaveChanges();

                        Session["UserRole"] = "User";

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такой пользователь уже существует");
                    }
                }
            }

            return View(model);
        }

        //GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName && m.UserName == model.UserName && m.UserSurname == model.UserSurname).FirstOrDefault();

                    if (user != null)
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
                        ModelState.AddModelError("", "Такого пользователя не существует");
                    }
                }
            }

            return View(model);
        }

        //POST: Account/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            Session["Id"] = null;

            return View("Index");
        }

        //GET: Account/GetAdmin
        [MyAuth]
        public ActionResult GetAdmin()
        {
            return View();
        }

        //POST: Account/GetAdmin
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult GetAdmin(UserGetRightsModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName && m.UserName == model.UserName && m.UserSurname == model.UserSurname).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Password == model.Password)
                        {
                            if (model.SecurityCode == "qw12po09fj")
                            {
                                user.UserRoleName = "Admin";

                                Session["AdminRole"] = user.UserRoleName;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Неправильный защитный код");
                            }
                        }
                    }
                }
            }

            return View(model);
        }

        //GET: Account/GetModer
        [MyAuth]
        public ActionResult GetModer()
        {
            return View();
        }

        //POST: Account/GerModer
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult GetModer(UserGetRightsModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName && m.UserName == model.UserName && m.UserSurname == model.UserSurname).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Password == model.Password)
                        {
                            if (model.SecurityCode == "bmd78zl4r1")
                            {
                                user.UserRoleName = "Moder";

                                Session["ModerRole"] = user.UserRoleName;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Неправильный защитный код");
                            }
                        }
                    }
                }
            }

            return View(model);
        }
    }
}