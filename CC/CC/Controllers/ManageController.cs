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
        //GET: Manage/ListOfUsers
        [MyAuth]
        public ActionResult ListOfUsers()
        {
            using (var context = new UserContext())
            {
                return View(context.Users.ToList());
            }
        }

        //GET: Manage/AllRecords
        public ActionResult AllRecords()
        {
            using (var context = new UserContext())
            {
                return View(context.Records.ToList());
            }
        }

        //GET: Manage/ListOfRecords
        [MyAuth]
        public ActionResult ListOfRecords()
        {
            using (var context = new UserContext())
            {
                var user = context.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefault();

                return View(context.Records.Where(m => m.UserId == user.Id).ToList());
            }
        }

        //POS: Manage/ListOfRecords
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult ListOfRecords(RecordEditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.Id == model.UserId).FirstOrDefault();

                    if (user != null)
                    {
                        var record = context.Records.Where(m => m.Id == model.Id).FirstOrDefault();

                        if (record != null)
                        {
                            record.Title = model.Title;
                            record.Description = model.Description;

                            context.Entry(record).State = EntityState.Modified;
                            context.SaveChanges();

                            return RedirectToAction("AccountIndex", "Account");
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

            return View(model);
        }

        //GET: Manage/Details
        public ActionResult Details(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Records.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        //GET: Manage/UserTickets
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult UseTickets(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Users.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        //POST: Manage/UseTickets
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

        // GET: Manage/Acc
        [MyAuth]
        public ActionResult AccountIndex(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Users.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        //GET: Manage/EditUserData
        [MyAuth]
        public ActionResult EditUserData(int? id)
        {
            using (var contex = new UserContext())
            {
                return View(contex.Users.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        //POST: Manage/EditUserData
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

        //GET: Manage/EditUserPassword
        [MyAuth]
        public ActionResult EditUserPassword(int? id)
        {
            return View();
        }

        //POST: Manage/EditUserPassword
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

        //GET: Manage/Delete
        [MyAuth]
        public ActionResult Delete(int? id)
        {
            using (var contex = new UserContext())
            {
                var user = contex.Users.Where(m => m.Id == id).FirstOrDefault();

                return View(user);
            }
        }

        //POST: Manage/Delete
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

        //GET: Manage/AddRecord
        [MyAuth]
        public ActionResult AddRecord()
        {
            return View();
        }

        //POST: Manage/AddRecord
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord(RecordAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.UserRoleName == "Moder")
                        {
                            context.Records.Add(new Record { NickName = model.NickName, Title = model.Title, Description = model.Description });
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

        //GET: Manage/EditRecord
        [MyAuth]
        public ActionResult EditRecord(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Records.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        //POST: Manage/EditRecord
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecord(RecordAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName).FirstOrDefault();

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

        //GET: Manage/DeleteRecord
        [MyAuth]
        public ActionResult DeleteRecord(int? id)
        {
            using (var context = new UserContext())
            {
                return View(context.Records.Where(m => m.Id == id).FirstOrDefault());
            }
        }

        //POST: Manage/DeleteRecord
        [HttpPost]
        [MyAuth]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(Record model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.Where(m => m.NickName == model.NickName);

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
    }
}