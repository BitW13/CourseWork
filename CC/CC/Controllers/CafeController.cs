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
    public class CafeController : Controller
    {        
        //GET, POST: Cafe/WriteDescription
        #region Добавление описания для заведений 

        //[MyAuth]
        public async Task<ActionResult> WriteDescription()
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
                    int id = int.Parse(Session["Id"].ToString());

                    var user = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();

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

        //GET, POST: Cafe/EditDescription
        #region Редактирование описания для заведения 

        //[MyAuth]
        public async Task<ActionResult> EditDescription(int? id)
        {
            using (var contex = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                var cafe = await contex.Cafes.Where(m => m.Id == id).FirstOrDefaultAsync();

                var model = new EditCafeDescriptionModel { Id = cafe.Id, Name = cafe.Name, Description = cafe.Description };

                return View(model);
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

        //GET, POST: Cafe/DeleteDescription
        #region Удаление описания для заведения

        //[MyAuth]
        public async Task<ActionResult> DeleteDescription(int? id)
        {
            using (var contex = new UserContext())
            {
                if (Session["Id"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(Session["Id"].ToString());

                var user = await contex.Users.Where(m => m.Id == userId).FirstOrDefaultAsync();

                if (user.UserRoleName != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                var cafe = await contex.Cafes.Where(m => m.Id == id).FirstOrDefaultAsync();

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
                        context.Entry(cafe).State = EntityState.Deleted;
                        context.SaveChanges();

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такого заведения не существует");
                    }
                }
            }

            return View(model);
        }

        #endregion

        //GET: Cafe/UserCafe
        #region Описание заведений определенного пользователя

        public async Task<ActionResult> UserCafe()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int id = int.Parse(Session["Id"].ToString());

            using (var context = new UserContext())
            {
                var cafe = await context.Cafes.Where(m => m.UserId == id).ToListAsync();

                return View(cafe);


            }

        }

        #endregion

        //GET: Cafe/ListOfCafes
        #region Список заведений 

        public ActionResult ListOfCafes()
        {
            using (var context = new UserContext())
            {
                IEnumerable<Cafe> list = context.Cafes.ToList();

                return View(list);
            }
        }

        #endregion

        //GET: Cafe/GetCafe
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
    }
}