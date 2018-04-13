using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Context;
using System.Threading.Tasks;
using System.Data.Entity;
using CC.Models;
using AutoMapper;
using CC.Models.Abstract;
using CC.Context.ContextModels;
using CC.Filters;

namespace CC.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Record> _repository;

        public HomeController(IRepository<Record> repository)
        {
            _repository = repository;
        }

        //GET: Home/Index
        #region Главная страница

        public ActionResult Index()
        {
            var list = _repository.GetAll().ToList();

            list.Reverse();

            return View(list);
        }

        #endregion

        //GET: Home/Chat
        #region Лайв чат для админов

        [Admin]
        public ActionResult Chat()
        {
            return View();
        }

        #endregion

        //GET: Home/PriceList
        #region Прайс-лист

        public ActionResult PriceList()
        {
            return View();
        }

        #endregion
    }
}