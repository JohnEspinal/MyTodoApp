using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTodoApp;

namespace MyTodoApp.Controllers
{
    public class NotesController : Controller
    {
        private TodoDBEntities db = new TodoDBEntities();

        // GET: Notes
        public ActionResult Index()
        {
            var note = db.Note.Include(n => n.Type).Include(n => n.User);
            return View(note.ToList());
        }

        // GET: Notes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            ViewBag.TypeId = new SelectList(db.Type, "Id", "TypeName");
            ViewBag.UserId = new SelectList(db.User, "Id", "Name");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Seq,Title,Description,ReminderDate,TypeId,Status,UserId,CreationDate")] Note note)
        {
            if (ModelState.IsValid)
            {
                note.Id = Guid.NewGuid();
                note.CreationDate = DateTime.Now;
                db.Note.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(db.Type, "Id", "TypeName", note.TypeId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", note.UserId);
            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            note.CreationDate = DateTime.Now;
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeId = new SelectList(db.Type, "Id", "TypeName", note.TypeId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", note.UserId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Seq,Title,Description,ReminderDate,TypeId,Status,UserId,CreationDate")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = new SelectList(db.Type, "Id", "TypeName", note.TypeId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", note.UserId);
            return View(note);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Note note = db.Note.Find(id);
            db.Note.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
