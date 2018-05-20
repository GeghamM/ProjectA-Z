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
using BooksCatalogeMVC.ViewModels;
using System.Threading.Tasks;

namespace BooksCatalogeMVC.Controllers
{
    public class BooksController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        int PageSize = 12;
        // GET: Books
        public async Task<ActionResult> Index(string SearchText, string SortDown="Not Sorted",int page=1)
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Country);
            books = Service.SearchFilter(books, SearchText);
            foreach (var item in books)
            {
                
                if (item.ImagePath == null)
                {
                    item.ImagePath = @"~\Images\404.jpg";
                }
            }
            
            books = Service.SortBooksBy(books, SortDown);
            if (books.Count() <= (PageSize * (page - 1)) + 1 || page <= 0)
            {
                page = 1;
            }
            var total = await books.CountAsync();
            books = books.Skip((page - 1) * PageSize).Take(PageSize);
            BookIndexViewModel Books4View = new BookIndexViewModel(books, page, PageSize, total, SortDown, SearchText);
            return View(Books4View);
        }
        // GET: Books/Details/id
        public ActionResult Details(int? id)
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

        public ActionResult BookSummary(int id)
        {

            Book book = db.Books.Find(id);
            List<Review> reviews = db.Reviews.Where(r => r.BookId == book.Id).ToList();
            if (book == null)
            {
                return HttpNotFound();
            }
            BookSummaryViewModel Model = new BookSummaryViewModel(book, reviews);
            
            return View(Model);
        }

        public ActionResult AddReview(string Text, int BookId, string UserId)
        {
            Review Rev = new Review();
            Rev.BookId = BookId;
            Rev.AuthorId = null;
            Rev.Text = Text;
            Rev.UserName = UserId;
            Rev.ReviewDate = DateTime.Now;
            db.Reviews.Add(Rev);
            db.SaveChanges();
            return RedirectToAction("BookSummary", new { id = BookId });
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName");
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Title,AuthorID,IssueCountryID,Price,IssueDate,Description,PageCount,Source")] Book book, HttpPostedFileBase file)
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

        // GET: Books/Edit/id
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops", "ErrorPageHandler", new { id = 400 });
            }

            Book book = db.Books.Find(id);
                
            if (book == null)
            {
                return RedirectToAction("Oops", "ErrorPageHandler", new {id=404 });
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName", book.AuthorID);
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name", book.IssueCountryID);
            return View(book);
        }

        // POST: Books/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,AuthorID,IssueCountryID,Price,IssueDate,Description,PageCount,ImagePath,Source")] Book book,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Image sourceimage = System.Drawing.Image.FromStream(file.InputStream);
                    string filename = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                    string path = Server.MapPath(@"~\Images\BookCovers\" + filename);
                    ImageHandler.EditeAndSave(sourceimage, 360, 640, path);
                    if (book.ImagePath != null)
                    {
                        System.IO.File.Delete(Server.MapPath(book.ImagePath));
                    }
                    db.Books.First(b => b.Id == book.Id).ImagePath = @"~\Images\BookCovers\" + filename;
                }
                db.Books.First(b => b.Id == book.Id).Title = book.Title;
                db.Books.First(b => b.Id == book.Id).AuthorID = book.AuthorID;
                db.Books.First(b => b.Id == book.Id).IssueCountryID = book.IssueCountryID;
                db.Books.First(b => b.Id == book.Id).Price = book.Price;
                db.Books.First(b => b.Id == book.Id).IssueDate = book.IssueDate;
                db.Books.First(b => b.Id == book.Id).Description = book.Description;
                db.Books.First(b => b.Id == book.Id).PageCount = book.PageCount;
                db.Books.First(b => b.Id == book.Id).Source = book.Source;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FullName", book.AuthorID);
            ViewBag.IssueCountryID = new SelectList(db.Countires, "ID", "Name", book.IssueCountryID);
            return View(book);
        }

        // GET: Books/Delete/id
        [Authorize]
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

        // POST: Books/Delete/id
        [Authorize]
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
        //For Filling base with books for testing
        [Authorize]
        public ActionResult Create50()
        {
            if(!db.Authors.Any())
            {
                db.Authors.Add(new Author { BirthDate = DateTime.Now, Description = "1", FullName = "Author 1", ImagePath = null, });
            }
            if (!db.Countires.Any())
            {
                db.Countires.Add(new Country { Code = 1, Name = "Country 1" });
            }
            db.SaveChanges();
            for (int i=0;i<50;i++)
            {
                Random r = new Random();
                db.Books.Add(new Book
                {
                    Title = "Book No" + i,
                    IssueCountryID = db.Countires.First().ID,
                    AuthorID = db.Authors.First().ID,
                    PageCount = r.Next(10, 300),
                    ImagePath = null,
                    IssueDate = DateTime.Now,
                    Description = "Personal computer designers and developers typically assume every pixel on the computer screen is 100% addressable. By contrast, broadcast and interactive TV designers and developers account for action- or title-safe areas, assuming there are areas on screen that are not addressable by the application. For Windows Media Center you need not be concerned with action- or title-safe areas because of the following:",
                    Price = r.Next(2, 50),
                });
            }
            db.SaveChanges();
            return RedirectToAction("index");
        }
        //for deleting all Books from db
        [Authorize]
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
