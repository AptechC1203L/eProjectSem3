using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoctorForums.Models;
using DoctorForums.DAO;

namespace DoctorForums.Controllers
{
    public class RoomsController : Controller
    {
        private MainDataClassDataContext db;
        public RoomsController()
        {
            this.db = new MainDataClassDataContext();
        }
        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = 
                from r in this.db.rooms
                select new Room {
                    Name = r.name,
                    Description = r.description,
                    Topics = r.message_threads.Select(topic => new Topic { Title = topic.title })
                };
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int id)
        {
            
            return View(db.rooms.SingleOrDefault(r => r.id == id));
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
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

        // GET: Rooms/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rooms/Edit/5
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

        // GET: Rooms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rooms/Delete/5
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
