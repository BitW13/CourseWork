using AutoMapper;
using CC.Context;
using CC.Context.ContextModels;
using CC.Cryptor;
using CC.Filters;
using CC.Models;
using CC.Models.Abstract;
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
        private IRepository<Cafe> _repositoryCafe;
        private IRepository<User> _repositoryUser;

        public CafeController(IRepository<Cafe> repository1, IRepository<User> repository2)
        {
            _repositoryCafe = repository1;
            _repositoryUser = repository2;
        }

        //GET, POST: Cafe/WriteDescription
        #region Добавление описания для заведений 

        [Admin]
        public ActionResult WriteDescription()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WriteDescription(AddCafeDescriptionModel model)
        {
            if (ModelState.IsValid)
            {
                Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

                var getConvert = new Cafe { Name = model.Name, Description = model.Description };

                var user = _repositoryUser.GetElementById(id);

                var oldCafe = _repositoryCafe.GetElement(getConvert);

                if (oldCafe == null)
                {
                    _repositoryCafe.Create(new Cafe {Id = Guid.NewGuid(), UserId = id, Name = model.Name, Description = model.Description });

                    return RedirectToAction("AccountIndex", "Manage");
                }
                else
                {
                    ModelState.AddModelError("", "Такое заведение уже существует");
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Cafe/EditDescription
        #region Редактирование описания для заведения 

        [Admin]
        public ActionResult EditDescription(Guid? id)
        {
            var cafe = _repositoryCafe.GetElementById(id);

            var model = new EditCafeDescriptionModel { Id = cafe.Id, Description = cafe.Description, Name = cafe.Name };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDescription(EditCafeDescriptionModel model)
        {
            if (ModelState.IsValid)
            {
                var cafe = _repositoryCafe.GetElementById(model.Id);

                cafe.Name = model.Name;
                cafe.Description = model.Description;

                _repositoryCafe.Update(cafe);
            }

            return View(model);
        }

        #endregion

        //GET, POST: Cafe/DeleteDescription
        #region Удаление описания для заведения

        [Admin]
        public ActionResult DeleteDescription(Guid? id)
        {
            var cafe = _repositoryCafe.GetElementById(id);

            return View(cafe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDescription(Cafe model)
        {
            if (ModelState.IsValid)
            {
                var cafe = _repositoryCafe.GetElementById(model.Id);

                _repositoryCafe.Delete(cafe.Id);
            }

            return View(model);
        }

        #endregion

        //GET: Cafe/UserCafe
        #region Описание заведений определенного пользователя

        public ActionResult UserCafe()
        {
            Guid id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var cafe = _repositoryCafe.GetElementByUserId(id);

            return View(cafe);

        }

        #endregion

        //GET: Cafe/ListOfCafes
        #region Список заведений 

        public ActionResult ListOfCafes()
        {
            return View();
        }

        public ActionResult TableData(string cafe)
        {
            var list = _repositoryCafe.GetAll();

            if (cafe != null)
            {
                list = list.Where(m => m.Name.Contains(cafe));
            }

            return PartialView(list);
        }

        #endregion

        //GET: Cafe/GetCafe
        #region Вывод описание одного заведения

        public ActionResult GetCafe(Guid? id)
        {
            var cafe = _repositoryCafe.GetElementById(id);
            return View(cafe);
        }

        #endregion
    }
}