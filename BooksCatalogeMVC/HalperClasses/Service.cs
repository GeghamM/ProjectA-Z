using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BooksCatalogeMVC.HalperClasses
{
    public static class Service
    {
        public static IQueryable<Book> SortBooksBy(IQueryable<Book> books, String Order)
        {
            switch (Order)
            {
                case "Tittle":
                    books=books.OrderBy(b => b.Title);
                    break;

                case "Author":
                    books=books.OrderBy(b => b.Author.FullName);
                    break;

                case "Price":
                    books=books.OrderBy(b => b.Price);
                    break;

                case "Date":
                    books=books.OrderBy(b => b.IssueDate);
                    break;

                default:
                    books = books.OrderBy(b => b.Id);
                    break;
            }
            return books;
        }
        public static IQueryable<Book> SearchFilter(IQueryable<Book> books,String SearchString)
        {
            if(!String.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.Contains(SearchString) || s.Author.FullName.Contains(SearchString) || s.Description.Contains(SearchString));
            }
            return books;
        }

    }
}