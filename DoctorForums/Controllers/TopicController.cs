using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoctorForums.Models;

namespace DoctorForums.Controllers
{
    public class TopicViewModel
    {
        public Topic Topic { get; set; }
        public string NewMessageContent { get; set; }
    }

    public class TopicController : Controller
    {
        private MainLinqDataDataContext db;

        public TopicController()
        {
            this.db = new Models.MainLinqDataDataContext();
        }

        // GET: Topic
        public ActionResult Index()
        {
            return View();
        }

        // GET: Topic/Details/5
        public ActionResult Details(int id)
        {
            var vm = new TopicViewModel { Topic = db.Topics.SingleOrDefault(topic => topic.Id == id) };
            return View(vm);
        }

        // GET: Topic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topic/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Topic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Topic/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Topic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Topic/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
