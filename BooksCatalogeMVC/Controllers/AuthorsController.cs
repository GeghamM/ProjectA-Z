using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Drawing;
using BooksCatalogeMVC.HalperClasses;

namespace BooksCatalogeMVC.Controllers
{
    public class AuthorsController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Authors
        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,BirthDate,Description,Img")] Author author, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (file.ContentType == "image/jpeg")
                    {
                        Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                        string filename = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                        string path = Server.MapPath(@"~\Images\AuthorImages\" + filename);
                        ImageHandler.EditeAndSave(sourceimage, 360, 640, path);
                        author.ImagePath = @"~\Images\AuthorImages\" + filename;
                        db.Authors.Add(author);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ImageError = "Selected image must be a jpg";
                    }
                }
                    db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,BirthDate,Description,Img")] Author author,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                    string filename = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                    string path = Server.MapPath(@"~\Images\AuthorImages\" + filename);
                    ImageHandler.EditeAndSave(sourceimage, 360, 640, path);
                    System.IO.File.Delete(Server.MapPath(author.ImagePath));
                    author.ImagePath = @"~\Images\AuthorImages\" + filename;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            if (author.ImagePath != null)
            {
                System.IO.File.Delete(Server.MapPath(author.ImagePath));
            }
            db.Authors.Remove(author);
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
