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

                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View(user);
            }
        }

        #endregion

        //GET: Manage/ListOfUsers
        #region Список пользователей

        //[MyAuth]
        public async Task<ActionResult> ListOfUsers()
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

                return View(context.Users.ToList());
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

                int id = int.Parse(Session["Id"].ToString());

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

                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                //if (user == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}
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
                        if (user.Password == model.Password)
                        {
                            if (model.NewPassword == model.ConfirmPassword)
                            {
                                user.Password = model.NewPassword;

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

                int id = int.Parse(Session["Id"].ToString());

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
        public async Task<ActionResult> UseTickets(int? id)
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int adminId = int.Parse(Session["Id"].ToString());

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

                var model = new UseTicketsModel() { Id = user.Id};

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
                        if (user.Password == model.Password)
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
                                ModelState.AddModelError("", "У Вас недостаточно купонов");
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
    }
}