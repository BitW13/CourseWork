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
    public class RecordController : Controller
    {
        private IRepository<Record> _repostitoryRecord;
        private IRepository<User> _repostitoryUser;

        public RecordController(IRepository<Record> repository1, IRepository<User> repository2)
        {
            _repostitoryRecord = repository1;
            _repostitoryUser = repository2;
        }

        //GET: Record/AllRecords
        #region Список всех новостей

        public ActionResult AllRecords(string recordName)
        {
            var list = _repostitoryRecord.GetAll();

            if (recordName != null)
            {
                list = list.Where(m => m.Title.Contains(recordName));
            }

            return View(list.Reverse());
        }


        #endregion

        //GET: Record/ListOfRecords
        #region Список новостей определенного пользователя

        [Moder]
        public ActionResult ListOfRecords()
        {
            var id = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

            var list = _repostitoryRecord.GetElementByUserId(id);

            return View(list);
        }

        #endregion

        //GET: Record/Details
        #region Новость детально

        public ActionResult Details(Guid? id)
        {
            var record = _repostitoryRecord.GetElementById(id);

            return View(record);
        }

        #endregion

        //GET, POST: Record/AddRecord
        #region Добавление новостей

        [Moder]
        public ActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord(RecordAddModel model)
        {
            if (ModelState.IsValid)
            {
                var getConvert = new Record { NickName = model.NickName, Title = model.Title, Description = model.Description };

                var oldRecord = _repostitoryRecord.GetElement(getConvert);

                Guid userId = Guid.Parse(Decoding.GetDecrypt(HttpContext.Request.Cookies["Id"].Value));

                if (oldRecord == null)
                {
                    if (model.NickName == null)
                    {
                        var user = _repostitoryUser.GetElementById(userId);

                        _repostitoryRecord.Create(new Record { Id = Guid.NewGuid(), UserId = userId, NickName = user.NickName, Title = model.Title, Description = model.Description, RecordDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) });

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        _repostitoryRecord.Create(new Record { Id = Guid.NewGuid(), UserId = userId, NickName = Encoding.GetCrypt(model.NickName), Title = model.Title, Description = model.Description, RecordDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) });

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такое заведение уже существует");
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Record/EditRecord
        #region Редактирование новостей

        [Moder]
        public ActionResult EditRecord(Guid? id)
        {
            var record = _repostitoryRecord.GetElementById(id);

            var model = new RecordEditModel { Id = record.Id, UserId = record.UserId };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecord(RecordEditModel model)
        {
            if (ModelState.IsValid)
            {
                var record = _repostitoryRecord.GetElementById(model.Id);

                if (record != null)
                {
                    record.Title = model.Title;
                    record.Description = model.Description;

                    _repostitoryRecord.Update(record);

                    return RedirectToAction("ListOfRecords", "Record");
                }
                else
                {
                    ModelState.AddModelError("", "Такой записи не сущетвует");
                }
            }

            return View(model);
        }

        #endregion

        //GET, POST: Record/DeleteRecord
        #region Удаление новостей

        [Moder]
        public ActionResult DeleteRecord(Guid? id)
        {
            var record = _repostitoryRecord.GetElementById(id);

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(Record model)
        {
            if (ModelState.IsValid)
            {
                var record = _repostitoryRecord.GetElementById(model.Id);

                if (record != null)
                {
                    _repostitoryRecord.Delete(record.Id);

                    return RedirectToAction("ListOfRecords", "Record");
                }
                else
                {
                    ModelState.AddModelError("", "Такой записи не существует");
                }
            }

            return View(model);
        }

        #endregion

    }
}