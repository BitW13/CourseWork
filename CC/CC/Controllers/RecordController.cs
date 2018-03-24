using CC.Context;
using CC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CC.Controllers
{
    public class RecordController : Controller
    {
        //GET: Record/AllRecords
        #region Список всех новостей

        public ActionResult AllRecords()
        {
            using (var context = new UserContext())
            {
                return View(context.Records.ToList());
            }
        }


        #endregion

        //GET: Record/ListOfRecords
        #region Список новостей определенного пользователя

        //[MyAuth]
        public async Task<ActionResult> ListOfRecords()
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Moder")
                {
                    return RedirectToAction("Login", "Account");
                }

                return View(context.Records.Where(m => m.UserId == user.Id).ToList());
            }
        }

        #endregion

        //GET: Record/Details
        #region Новость детально

        public async Task<ActionResult> Details(int? id)
        {
            using (var context = new UserContext())
            {
                var record = await context.Records.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View(record);
            }
        }

        #endregion

        //GET, POST: Record/AddRecord
        #region Добавление новостей

        //[MyAuth]
        public async Task<ActionResult> AddRecord()
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Moder")
                {
                    return RedirectToAction("Login", "Account");
                }

                var model = new RecordAddModel { Id = user.Id };

                return View(model);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRecord(RecordAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.UserRoleName == "Moder")
                        {
                            context.Records.Add(new Record { NickName = model.NickName, Title = model.Title, Description = model.Description, UserId = user.Id });
                            context.SaveChanges();

                            return RedirectToAction("Index", "Home");
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

        //GET, POST: Record/EditRecord
        #region Редактирование новостей

        //[MyAuth]
        public async Task<ActionResult> EditRecord(int? id)
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();

                if (user.UserRoleName != "Moder")
                {
                    return RedirectToAction("Login", "Account");
                }

                var record = await context.Records.Where(m => m.Id == id).FirstOrDefaultAsync();

                var model = new RecordEditModel { Id = record.Id, Title = record.Title, UserId = user.Id, Description = record.Description };

                return View(model);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRecord(RecordEditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.UserId).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.UserRoleName == "Moder")
                        {
                            var record = await context.Records.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

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

        //GET, POST: Record/DeleteRecord
        #region Удаление новостей

        //[MyAuth]
        public async Task<ActionResult> DeleteRecord(int? id)
        {
            using (var context = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();

                if (user.UserRoleName != "Moder")
                {
                    return RedirectToAction("Login", "Account");
                }

                var record = await context.Records.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View(record);
            }
        }

        [HttpPost]
        //[MyAuth]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRecord(Record model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == model.UserId).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        var record = await context.Records.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

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

    }
}