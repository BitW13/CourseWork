﻿using CC.Models;
using System;
using System.Web;
using System.Web.Mvc;
using CC.Filters;
using CC.Cryptor;
using AutoMapper;
using CC.Models.Abstract;
using CC.Context.ContextModels;

namespace CC.Controllers
{
    public class AccountController : Controller
    {
        private IRepository<User> _repository;

        public AccountController(IRepository<User> repository)
        {
            _repository = repository;
        }

        // GET, POST: Account/Create
        #region Регистрация

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateModel model)
        {
            ClearCookie();

            if (ModelState.IsValid)
            {
                User oldUser = new User { NickName = model.NickName, UserName = model.UserName, UserSurname = model.UserSurname };

                User newUser = _repository.GetElement(oldUser);

                if (newUser == null)
                {
                    if (model.ConfirmPassword == model.Password)
                    {
                        Guid id = Guid.NewGuid();

                        newUser = new User { Id = id, NickName = Encoding.GetCrypt(model.NickName), Password = Encoding.GetCrypt(model.Password), UserCoins = 4, UserRoleName = "User", UserName = Encoding.GetCrypt(model.UserName), UserSurname = Encoding.GetCrypt(model.UserSurname), UserTickets = 0 };

                        _repository.Create(newUser);

                        const int timeout = 262800;

                        Response.Cookies["LoggedIn"].Value = "Accepted";
                        Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddMinutes(timeout);

                        Response.Cookies["User"].Value = newUser.UserName;
                        Response.Cookies["User"].Expires = DateTime.Now.AddMinutes(timeout);

                        Response.Cookies["Id"].Value = Encoding.GetCrypt(newUser.Id.ToString());
                        Response.Cookies["Id"].Expires = DateTime.Now.AddMinutes(timeout);

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
        public ActionResult Login(UserLoginModel model)
        {
            ClearCookie();

            if (ModelState.IsValid)
            {
                var getConvert = new User { UserName = Encoding.GetCrypt(model.UserName), NickName = Encoding.GetCrypt(model.NickName), UserSurname = Encoding.GetCrypt(model.UserSurname) };

                var user = _repository.GetElement(getConvert);

                if (user != null)
                {
                    if (user.Password == Encoding.GetCrypt(model.Password))
                    {
                        const int timeout = 262800;

                        Response.Cookies["Id"].Value = Encoding.GetCrypt(user.Id.ToString());
                        Response.Cookies["Id"].Expires = DateTime.Now.AddMinutes(timeout);

                        Response.Cookies["LoggedIn"].Value = "Accepted";
                        Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddMinutes(timeout);

                        if (user.UserRoleName == "User")
                        {
                            Response.Cookies["User"].Value = user.UserRoleName;
                            Response.Cookies["User"].Expires = DateTime.Now.AddMinutes(timeout);
                        }
                        else if (user.UserRoleName == "Admin")
                        {
                            Response.Cookies["Admin"].Value = user.UserRoleName;
                            Response.Cookies["Admin"].Expires = DateTime.Now.AddMinutes(timeout);
                        }
                        else if (user.UserRoleName == "Moder")
                        {
                            Response.Cookies["Moder"].Value = user.UserRoleName;
                            Response.Cookies["Moder"].Expires = DateTime.Now.AddMinutes(timeout);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такого пользователя не существует");
                }
            }

            return View(model);
        }
        #endregion

        #region Метод удаления куков

        public void ClearCookie()
        {
            const int negativeTime = -263000;

            if (Request.Cookies["Id"] != null)
            {
                Response.Cookies["Id"].Expires = DateTime.Now.AddMinutes(negativeTime);
                Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddMinutes(negativeTime);
                Response.Cookies["User"].Expires = DateTime.Now.AddMinutes(negativeTime);
                Response.Cookies["Admin"].Expires = DateTime.Now.AddMinutes(negativeTime);
                Response.Cookies["Moder"].Expires = DateTime.Now.AddMinutes(negativeTime);
            }
        }

        #endregion

        //POST: Account/Logout
        #region Выход из аккаунта

        [HttpPost]
        public ActionResult Logout()
        {
            ClearCookie();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        //GET, POST: Account/GetAdmin
        #region Получение прав администратора

        [MyAuth]
        public ActionResult GetAdmin()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var user = _repository.GetElementById(id);

            var model = new UserGetRightsModel { Id = user.Id };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAdmin(UserGetRightsModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                if (model.SecurityCode == "qw12po09fj")
                {
                    if (user.Password == Encoding.GetCrypt(model.Password))
                    {
                        user.UserRoleName = "Admin";
                        Session["AdminRole"] = user.UserRoleName;

                        Session["UserRole"] = null;
                        Session["ModerRole"] = null;

                        _repository.Update(user);

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный код");
                }
            }

            return View(model);
        }
        #endregion

        //GET, POST: Account/GetModer
        #region Получение прав модератора

        [MyAuth]
        public ActionResult GetModer()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var user = _repository.GetElementById(id);

            var model = new UserGetRightsModel { Id = user.Id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetModer(UserGetRightsModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                if (model.SecurityCode == "bmd78zl4r1")
                {
                    if (user.Password == Encoding.GetCrypt(model.Password))
                    {
                        user.UserRoleName = "Moder";
                        Session["ModelRole"] = user.UserRoleName;

                        Session["UserRole"] = null;
                        Session["AdminRole"] = null;

                        _repository.Update(user);

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный код");
                }
            }

            return View(model);
        }
        #endregion
    }
}