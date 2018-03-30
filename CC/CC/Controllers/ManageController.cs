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

namespace CC.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage/AccountIndex
        #region Страница аккаунта пользователя

        //[MyAuth]
        public async Task<ActionResult> AccountIndex()
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View(user);
            }
        }

        #endregion

        //GET: Manage/ListOfUsers
        #region Список пользователей

        //[MyAuth]
        public async Task<ActionResult> ListOfUsers(string NickName)
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

                var list = await context.Users.Where(m => m.NickName.Contains(NickName) || NickName == null).ToListAsync();

                return View(list);
            }
        }

        #endregion

        //GET, POST: Manage/EditUserData
        #region Редактирование данных пользователя

        //[MyAuth]
        public async Task<ActionResult> EditUserData()
        {
            using (var contex = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                var model = new UserEditDataModel { Id = user.Id, NickName = user.NickName, UserName = user.UserName, UserSurname = user.UserSurname };

                return View(model);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserData(UserEditDataModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        user.NickName = model.NickName;
                        user.UserName = model.UserName;
                        user.UserSurname = model.UserSurname;

                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();

                        return RedirectToAction("AccountIndex", "Manage");
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

        //GET, POST: Manage/EditUserPassword
        #region Изменение пароля пользователя 

        //[MyAuth]
        public async Task<ActionResult> EditUserPassword()
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                var model = new UserEditPasswordModel { Id = user.Id };

                return View(model);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserPassword(UserEditPasswordModel model)
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
                            if (model.NewPassword == model.ConfirmPassword)
                            {
                                user.Password = Encoding.GetCrypt(model.NewPassword);

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

        #endregion

        //GET, POST: Manage/Delete
        #region Удаление аккаунта пользователя

        //[MyAuth]
        public async Task<ActionResult> Delete()
        {
            using (var contex = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                Guid id = Guid.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View(user);
            }
        }

        [HttpPost]
        //[MyAuth]
        public async Task<ActionResult> Delete(User model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        context.Entry(user).State = EntityState.Deleted;
                        context.SaveChanges();

                        Session["Id"] = null;
                        Session["UserRole"] = null;
                        Session["AdminRole"] = null;
                        Session["ModerRole"] = null;

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        #endregion

        //GET, POST: Manage/UseTickets
        #region Использование билетов для кофе

        //[MyAuth]
        public async Task<ActionResult> UseTickets(Guid? id)
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                Guid adminId = Guid.Parse(Session["Id"].ToString());

                var admin = await context.Users.Where(m => m.Id == adminId).FirstOrDefaultAsync();

                if (admin == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserTickets <= 0)
                {
                    ModelState.AddModelError("", "У Вас недостаточно купонов");
                }

                var model = new UseTicketsModel() { Id = user.Id };

                return View(model);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UseTickets(UseTicketsModel model)
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
                            if (user.UserTickets > 0)
                            {
                                user.UserTickets = user.UserTickets - 1;

                                context.Entry(user).State = EntityState.Modified;
                                context.SaveChanges();

                                return RedirectToAction("ListOfUsers", "Manage");
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

        #endregion

        //POST: Manage/SearchUser
        #region Поиск пользователя по никнейму

        //[HttpPost]
        //public async Task<ActionResult> SearchUser(string NickName)
        //{
        //    using (var context = new UserContext())
        //    {
        //        Guid id = Guid.Parse(Session["Id"].ToString());

        //        var list = await context.Users.ToListAsync();

        //        var userAdmin = await context.Users.FirstOrDefaultAsync(m => m.Id == id);

        //        if (userAdmin != null)
        //        {
        //            if (userAdmin.UserRoleName == "Admin")
        //            {
        //                var user = list.Where(m => m.NickName == NickName).ToList();

        //                if (user != null)
        //                {
        //                    ViewBag.Check = "alright";

        //                    return PartialView("_SearchUser", user);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Такого пользователя не существует");
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "У вас недостаточно прав");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Такого пользователя не существует");
        //        }
        //    }

        //    return PartialView("_SearchUser");
        //}

        #endregion
    }
}