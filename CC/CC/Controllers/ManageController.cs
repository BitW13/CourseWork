using CC.Context;
using CC.Models;
using CC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using CC.Cryptor;
using AutoMapper;
using CC.Controllers;
using CC.Models.Abstract;
using CC.Context.ContextModels;

namespace CC.Controllers
{
    public class ManageController : Controller
    {
        private IRepository<User> _repository;

        public ManageController(IRepository<User> repository)
        {
            _repository = repository;
        }

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

        // GET: Manage/AccountIndex
        #region Страница аккаунта пользователя

        [MyAuth]
        public ActionResult AccountIndex()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var encodeUser = (_repository.GetElementById(id));

            var user = new User { Id = id, UserCoins = encodeUser.UserCoins, NickName = Decoding.GetDecrypt(encodeUser.NickName), UserName = Decoding.GetDecrypt(encodeUser.UserName), UserSurname = Decoding.GetDecrypt(encodeUser.UserSurname), UserRoleName = encodeUser.UserRoleName };

            return View(user);
        }

        #endregion

        //GET: Manage/ListOfUsers
        #region Список пользователей

        [MyAuth]
        [Admin]
        public ActionResult ListOfUsers(string NickName)
        {
            var list = _repository.GetAll()/*.Where(m => m.NickName.Contains(NickName) || NickName == null)*/;

            return View(list);
        }

        #endregion

        //GET, POST: Manage/EditUserData
        #region Редактирование данных пользователя

        [MyAuth]
        public ActionResult EditUserData()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var user = _repository.GetElementById(id);

            var model = new UserEditDataModel { Id = user.Id, NickName = Decoding.GetDecrypt(user.NickName), UserName = Decoding.GetDecrypt(user.UserName), UserSurname = Decoding.GetDecrypt(user.UserSurname) };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserData(UserEditDataModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                if (user != null)
                {
                    user.NickName = Encoding.GetCrypt(model.NickName);
                    user.UserName = Encoding.GetCrypt(model.UserName);
                    user.UserSurname = Encoding.GetCrypt(model.UserSurname);

                    _repository.Update(user);

                    return RedirectToAction("AccountIndex", "Manage");
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Manage/EditUserPassword
        #region Изменение пароля пользователя 

        [MyAuth]
        public ActionResult EditUserPassword()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var user = _repository.GetElementById(id);

            var model = new UserEditPasswordModel { Id = user.Id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserPassword(UserEditPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                if (user.Password == Encoding.GetCrypt(model.Password))
                {
                    user.Password = Encoding.GetCrypt(model.NewPassword);

                    _repository.Update(user);

                    return RedirectToAction("AccountIndex", "Manage");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль");
                }
            }

            return View();
        }

        #endregion

        //GET, POST: Manage/Delete
        #region Удаление аккаунта пользователя

        [MyAuth]
        public ActionResult Delete()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var encodeUser = _repository.GetElementById(id);

            var user = new User { Id = id, NickName = Decoding.GetDecrypt(encodeUser.NickName), UserName = Decoding.GetDecrypt(encodeUser.UserName), UserSurname = Decoding.GetDecrypt(encodeUser.UserSurname), UserRoleName = encodeUser.UserRoleName, UserCoins = encodeUser.UserCoins, UserTickets = encodeUser.UserTickets };

            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                _repository.Delete(user.Id);

                ClearCookie();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        #endregion

        //GET, POST: Manage/UseTickets
        #region Использование билетов для кофе

        [MyAuth]
        [Admin]
        public ActionResult UseTickets(Guid? id)
        {
            var user = _repository.GetElementById(id);

            if (user.UserTickets <= 0)
            {
                ModelState.AddModelError("", "У Вас недостаточно купонов");
            }

            var model = new UseTicketsModel { Id = user.Id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UseTickets(UseTicketsModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.GetElementById(model.Id);

                if (user != null)
                {
                    if (user.Password == Encoding.GetCrypt(model.Password))
                    {
                        if (user.UserTickets > 0)
                        {
                            user.UserTickets = user.UserTickets - 1;

                            _repository.Update(user);

                            return RedirectToAction("ListOfUsers", "Manage");
                        }
                        else
                        {
                            ModelState.AddModelError("", "У вас недостаточно купонов");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный пароль");
                    }
                }
            }

            return View();
        }

        #endregion

    }
}