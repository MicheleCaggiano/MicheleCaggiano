using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MicheleCaggiano.Web.Model;

namespace MicheleCaggiano.Web.Controllers
{
    public class ArticoloesController : Controller
    {
        private ModelContainer db = new ModelContainer();

        // GET: Articoloes
        public ActionResult Index()
        {
            return View(db.Articolo.ToList());
        }

        // GET: Articoloes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articolo articolo = db.Articolo.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }
            return View(articolo);
        }

        // GET: Articoloes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articoloes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Titolo,Testo,AuthInfo,Cancellato,Pubblicato,DataPubblicazione")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
                db.Articolo.Add(articolo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articolo);
        }

        // GET: Articoloes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articolo articolo = db.Articolo.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }
            return View(articolo);
        }

        // POST: Articoloes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titolo,Testo,AuthInfo,Cancellato,Pubblicato,DataPubblicazione")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articolo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articolo);
        }

        // GET: Articoloes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articolo articolo = db.Articolo.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }
            return View(articolo);
        }

        // POST: Articoloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articolo articolo = db.Articolo.Find(id);
            db.Articolo.Remove(articolo);
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
