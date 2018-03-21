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
                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return View(user);
            }
        }

        #endregion

        //GET, POST: Manage/WriteDescription
        #region Добавление описания для заведений 

        //[MyAuth]
        public async Task<ActionResult> WriteDescription()
        {
            using (var context = new UserContext())
            {
                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

            }

            return View();
        }

        //[MyAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WriteDescription(AddCafeDescriptionModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var user = await context.Users.Where(m => m.Id == int.Parse(Session["Id"].ToString())).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (user.UserRoleName == "Admin")
                        {
                            var cafe = await context.Cafes.Where(m => m.Name == model.Name).FirstOrDefaultAsync();

                            if (cafe == null)
                            {
                                context.Cafes.Add(new Cafe { Name = model.Name, Description = model.Description, UserId = user.Id });
                                context.SaveChanges();

                                return RedirectToAction("AccountIndex", "Manage");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Заведение с таким именем уже существует");
                            }
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

        //GET, POST: Manage/EditDescription
        #region Редактирование описания для заведения 

        //[MyAuth]
        public async Task<ActionResult> EditDescription()
        {
            using (var contex = new UserContext())
            {
                int id = int.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                var cafe = await contex.Cafes.Where(m => m.UserId == user.Id).FirstOrDefaultAsync();

                return View(cafe);
            }
        }

        //[MyAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDescription(EditCafeDescriptionModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var cafe = await context.Cafes.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (cafe != null)
                    {
                        cafe.Name = model.Name;
                        cafe.Description = model.Description;

                        context.Entry(cafe).State = EntityState.Modified;
                        context.SaveChanges();

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такой записи ну существует");
                    }
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Manage/DeleteDescription
        #region Удаление описания для заведения

        //[MyAuth]
        public async Task<ActionResult> DeleteDescription()
        {
            using (var contex = new UserContext())
            {
                int id = int.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                var cafe = await contex.Cafes.Where(m => m.UserId == user.Id).FirstOrDefaultAsync();

                return View(cafe);
            }
        }

        //[MyAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDescription(Cafe model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new UserContext())
                {
                    var cafe = await context.Cafes.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

                    if (cafe != null)
                    {
                        context.Entry(cafe).State = EntityState.Modified;
                        context.SaveChanges();

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такого заведения не существует");
                    }
                }
            }

            return View();
        }

        #endregion

        //GET: Manage/ListOfCafes
        #region Список заведений 

        public ActionResult ListOfCafes()
        {
            using (var context = new UserContext())
            {
                return View(context.Cafes.ToList());
            }
        }

        #endregion

        //GET: Manage/GetCafe
        #region Вывод описание одного заведения

        public async Task<ActionResult> GetCafe(int? id)
        {
            using (var context = new UserContext())
            {
                var cafe = await context.Cafes.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View(cafe);
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

        //[MyAuth]
        public async Task<ActionResult> ListOfRecords()
        {
            using (var context = new UserContext())
            {
                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return View(context.Records.Where(m => m.UserId == user.Id).ToList());
            }
        }

        #endregion

        //GET: Manage/Details
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

        //GET, POST: Manage/AddRecord
        #region Добавление новостей

        //[MyAuth]
        public async Task<ActionResult> AddRecord()
        {
            using (var context = new UserContext())
            {
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

        //GET, POST: Manage/EditRecord
        #region Редактирование новостей

        //[MyAuth]
        public async Task<ActionResult> EditRecord(int? id)
        {
            using (var context = new UserContext())
            {
                int userId = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();

                if (user.UserRoleName != "Moder")
                {
                    return RedirectToAction("Login", "Account");
                }

                var record = await context.Records.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View();
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

        //GET, POST: Manage/DeleteRecord
        #region Удаление новостей

        //[MyAuth]
        public async Task<ActionResult> DeleteRecord(int? id)
        {
            using (var context = new UserContext())
            {
                int userId = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();

                if (user.UserRoleName != "Moder")
                {
                    return RedirectToAction("Login", "Account");
                }

                var record = await context.Records.Where(m => m.Id == id).FirstOrDefaultAsync();

                return View();
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

        //GET, POST: Manage/EditUserData
        #region Редактирование данных пользователя

        //[MyAuth]
        public async Task<ActionResult> EditUserData()
        {
            using (var contex = new UserContext())
            {
                int id = int.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

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
                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            return View();
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
                int id = int.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

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
        public async Task<ActionResult> UseTickets()
        {
            using (var context = new UserContext())
            {
                int id = int.Parse(Session["Id"].ToString());

                var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var model = new UseTicketsModel();

                model.Id = user.Id;

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