using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}