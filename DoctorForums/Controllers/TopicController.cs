using DoctorForums.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorForums.Controllers
{
    public class TopicController : Controller
    {
        private MainDataClassDataContext db;

        public TopicController()
        {
            this.db = new MainDataClassDataContext();
        }

        // GET: Topic
        public ActionResult Index()
        {
            return View();
        }

        // GET: Topic/Details/5
        public ActionResult Details(int id)
        {
            var topic = this.db.message_threads.SingleOrDefault(t => t.id == id);
            return View(topic);
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

        [HttpPost]
        public ActionResult CreateMessage(FormCollection collection)
        {
            try
            {
                var content = collection["content"];
                var topicId = int.Parse(collection["topic_id"]);

                // FIXME: Do some validation here
                var message = new DAO.message_table
                {
                    content = content,
                    creator_id = (Session["User"] as DAO.user).id,
                    thread_id = topicId
                };

                db.message_tables.InsertOnSubmit(message);
                db.SubmitChanges();

                return RedirectToAction("Details", new { id = topicId });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
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
