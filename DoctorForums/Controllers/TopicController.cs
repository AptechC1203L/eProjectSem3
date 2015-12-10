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

            var loggedInUser = Session["User"] as DAO.user;

            if (loggedInUser != null)
            {
                var record = (from likeRecord in db.user_interacts
                              where (likeRecord.user_id == loggedInUser.id)
                              && (likeRecord.target_id == topic.id)
                              && (likeRecord.target_table == "message_thread")
                              select likeRecord).SingleOrDefault();
                if (record != null)
                {
                    ViewBag.Interaction = "Like";
                }
                else
                {
                    ViewBag.Interaction = "Unlike";
                }
            }
            else
            {
                ViewBag.Interaction = "Nothing";
            }           
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
            var subject = collection["subject"];
            var content = collection["content"];
            var roomId = int.Parse(collection["room_id"]);
            var creator = Session["User"] as DAO.user;

            // FIXME: It would have been nicer if we could create both the 
            // topic and the associated message in one transaction.
            var newTopic = new message_thread
            {
                title = subject,
                creator_id = (Session["User"] as user).id,
                room_id = roomId,
                created_at = DateTime.Now
            };

            db.message_threads.InsertOnSubmit(newTopic);
            db.SubmitChanges();

            var newMessage = new message_table
            {
                content = content,
                thread_id = newTopic.id,
                creator_id = creator.id,
                created_at = DateTime.Now
            };

            var notifications = newTopic.room.moderations.Select(m =>
            {
                return new DAO.notification
                {
                    user_id = m.user_id,
                    content = String.Format(
                        "{0} has created a new topic in {1}.",
                        creator.full_name,
                        newTopic.room.name),
                    url = Url.Action("Details", "Topic", new { id = newTopic.id }),
                    created_at = DateTime.Now
                };
            });

            db.message_tables.InsertOnSubmit(newMessage);
            db.notifications.InsertAllOnSubmit(notifications);
            db.SubmitChanges();

            return RedirectToAction("Details", new { id = newTopic.id });
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
                    thread_id = topicId,
                    created_at = DateTime.Now
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
        
        public ActionResult Like(FormCollection collection)
        {
            var loggedInUser = Session["User"] as DAO.user;
            int target_id = int.Parse(collection["topic_id"]);
            var record = (from likeRecord in db.user_interacts
                          where (likeRecord.user_id == loggedInUser.id)
                          && (likeRecord.target_id == target_id)
                          && (likeRecord.target_table == "message_thread")
                          select likeRecord).SingleOrDefault();
            if (record == null)
            {
                var newLikeRecord = new DAO.user_interact();
                newLikeRecord.user_id = loggedInUser.id;
                newLikeRecord.target_table = "message_thread";
                newLikeRecord.target_id = target_id;
                newLikeRecord.content = "like";
                db.user_interacts.InsertOnSubmit(newLikeRecord);

                db.SubmitChanges();

                return this.Content("");
            }
            else
            {
                db.user_interacts.DeleteOnSubmit(record);
                db.SubmitChanges();

                return this.Content("");
            }
        }
    }
}
