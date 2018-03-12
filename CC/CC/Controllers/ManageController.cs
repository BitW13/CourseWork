using CC.Context;
using CC.Models;
using CC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CC.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage/AccountIndex
        #region Страница аккаунта пользователя

        [MyAuth]
        public ActionResult AccountIndex()
        {
            using (var context = new UserContext())
            {
                return View(context.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefault());
            }
        }

        #endregion

        //GET: Manage/ListOfUsers
        #region Список пользователей

        [MyAuth]
        public ActionResult ListOfUsers()
        {
            using (var context = new UserContext())
            {
                return View(context.Users.ToList());
            }
        }

        #endregion

        //GET: Manage/AllRecords
        #region Список всех новостей

        public ActionResult AllRecords()
        {
            using (var context = new UserContext())
            {
                return View(context.Records.ToList());
            }
        }


        #endregion

        //GET: Manage/ListOfRecords
        #region Список новостей определенного пользователя

        [MyAuth]
        public ActionResult ListOfRecords()
        {
            using (var context = new UserContext())
            {
                var user = context.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefault();

                return View(context.Records.Where(m => m.UserId == user.Id).ToList());
            }
        }

        #endregion

        //GET: Manage/Details
        #region Новость детально

        public ActionResult Details(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Records.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        #endregion

        //GET, POST: Manage/AddRecord
        #region Добавление новостей

        [MyAuth]
        public ActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord(RecordAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.UserRoleName == "Moder")
                        {
                            context.Records.Add(new Record { NickName = model.NickName, Title = model.Title, Description = model.Description, UserId = user.Id });
                            context.SaveChanges();

                            return RedirectToAction("AccountIndex", "Manage");
                        }
                        else
                        {
                            ModelState.AddModelError("", "У Вас недостаточно прав");
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

        //GET, POST: Manage/EditRecord
        #region Редактирование новостей

        [MyAuth]
        public ActionResult EditRecord(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Records.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecord(RecordEditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.UserId).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.UserRoleName == "Moder")
                        {
                            var record = context.Records.Where(m => m.Id == model.Id).FirstOrDefault();

                            record.Title = model.Title;
                            record.Description = model.Description;

                            context.Entry(record).State = EntityState.Modified;
                            context.SaveChanges();

                            return RedirectToAction("AccountIndex", "Manage");
                        }
                        else
                        {
                            ModelState.AddModelError("", "У Вас недостаточно прав");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователя с таким никнеймом не существует");
                    }
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Manage/DeleteRecord
        #region Удаление новостей

        [MyAuth]
        public ActionResult DeleteRecord(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Records.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(Record model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.UserId);

                    if (user != null)
                    {
                        var record = context.Records.Where(m => m.Id == model.Id).FirstOrDefault();

                        if (record != null)
                        {
                            context.Entry(record).State = EntityState.Deleted;
                            context.SaveChanges();

                            return RedirectToAction("AccountIndex", "Manage");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Такой записи не существует");
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

        //GET, POST: Manage/EditUserData
        #region Редактирование данных пользователя

        [MyAuth]
        public ActionResult EditUserData(int? id)
        {
            using (var contex = new UserContext())
            {
                return View(contex.Users.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserData(UserEditDataModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.Id).FirstOrDefault();

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

        [MyAuth]
        public ActionResult EditUserPassword(int? id)
        {
            return View();
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserPassword(UserEditPasswordModel model)
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

        [MyAuth]
        public ActionResult Delete()
        {
            using (var contex = new UserContext())
            {
                var user = contex.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefault();

                return View(user);
            }
        }

        [HttpPost]
        [MyAuth]
        public ActionResult Delete(User model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.Id).FirstOrDefault();

                    if (user != null)
                    {
                        context.Entry(user).State = EntityState.Deleted;
                        context.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        #endregion

        //GET, POST: Manage/UseTickets
        #region Использование билетов для кофе

        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult UseTickets()
        {
            using (var context = new UserContext())
            {
                return View(context.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefault());
            }
        }

        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult UseTickets(UseTicketsModel model)
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
                            user.UserTickets = user.UserTickets - 1;

                            context.Entry(user).State = EntityState.Modified;
                            context.SaveChanges();

                            return RedirectToAction("ListOfUsers", "Manage");
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