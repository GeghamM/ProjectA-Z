using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksCatalogeMVC.ViewModels
{
    public class BookSummaryViewModel
    {
        public BookSummaryViewModel(Book book, List<Review> reviews)
        {
            Book = book;
            Reviews = reviews;
        }
        public Book Book { set; get; }
        public List<Review> Reviews { set; get; }
   }
}