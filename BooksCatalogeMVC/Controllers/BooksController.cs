using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using DataAccessLayer.Models;
using BooksCatalogeMVC.HalperClasses;
using System.Drawing;
using System.Collections.Generic;

namespace BooksCatalogeMVC.Controllers
{
    public class BooksController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        int PageSize = 12;
        // GET: Books
        public ActionResult Index(string SearchText, string SortDown="Not Sorted",int page=1)
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Country);

            if (!String.IsNullOrEmpty(SearchText))
            {
                books = books.Where(s => s.Title.Contains(SearchText) || s.Author.FullName.Contains(SearchText) || s.Description.Contains(SearchText));
            }
            foreach (var item in books)
            {
                item.Price += (decimal)item.Country.Code / 10;
                if (item.ImagePath == null)
                {
                    item.ImagePath = @"~\Images\404.jpg";
                }
            }
            books = Service.SortBooksBy(books, SortDown);
            ViewBag.Count = books.Count();
            books = books.Skip((page - 1) * PageSize).Take(PageSize);
            ViewBag.search = SearchText;
            ViewBag.sort = SortDown;
            ViewBag.Page = page;
            ViewBag.PageSize = PageSize;
            ViewBag.Values = new SelectList(new List<string> { "Not Sorted" ,"Tittle", "Author", "Price", "Date" },SortDown);
            return View(books.ToList());
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            book.Price += (decimal)book.Country.Code / 10;
            if (book == null)
            {
                return HttpNotFound();
            }
            return PartialView(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName");
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name");
            return View();
        }

        // POST: Books/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,AuthorID,IssueCountryID,Price,IssueDate,Description,PageCount")] Book book, HttpPostedFileBase file,string[] selectedTags)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentType == "image/jpeg")
                {
                    Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                    string filename = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                    string path = Server.MapPath( @"~\Images\BookCovers\" + filename);
                    ImageHandler.EditeAndSave(sourceimage, 360, 640, path);
                    book.ImagePath = @"~\Images\BookCovers\" + filename;
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                if(file != null)
                {
                    ViewBag.ImageError = "Selected image must be a jpg";
                }
                else
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName", book.AuthorID);
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name", book.IssueCountryID);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books
                .Where(i => i.Id == id)
                .Single();
                
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName", book.AuthorID);
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name", book.IssueCountryID);
            return View(book);
        }

        // POST: Books/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,AuthorID,IssueCountryID,Price,IssueDate,Description,PageCount")] Book book, string[] selectedTags,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                    string filename = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                    string path = Server.MapPath(@"~\Images\BookCovers\" + filename);
                    ImageHandler.EditeAndSave(sourceimage, 360, 640, path);
                    System.IO.File.Delete(Server.MapPath(book.ImagePath));
                    book.ImagePath = @"~\Images\BookCovers\" + filename;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName", book.AuthorID);
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name", book.IssueCountryID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return PartialView(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            if (book.ImagePath != null)
            {
                System.IO.File.Delete(Server.MapPath(book.ImagePath));
            }
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create50()
        {
            for(int i=0;i<50;i++)
            {
                db.Books.Add(new Book
                {
                    Title = "Book No" + i,
                    IssueCountryID = 1,
                    AuthorID = 2,
                    PageCount = 15,
                    ImagePath = null,
                    IssueDate=DateTime.Now,
                    Description= "Personal computer designers and developers typically assume every pixel on the computer screen is 100% addressable. By contrast, broadcast and interactive TV designers and developers account for action- or title-safe areas, assuming there are areas on screen that are not addressable by the application. For Windows Media Center you need not be concerned with action- or title-safe areas because of the following:",
                    Price=15,
                });
            }
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Deleteall()
        {
            db.Books.RemoveRange(db.Books);
            db.SaveChanges();
            return RedirectToAction("index");
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
