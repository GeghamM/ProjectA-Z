using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooksCatalogeMVC.ViewModels
{
    public class BookIndexViewModel
    {
        public BookIndexViewModel(IQueryable<Book> books,int current,int size,int total,String sort,String search)
        {
            Books = books.ToList();
            CurrentPage = current;
            PageSize = size;
            Total = total;
            Sort = sort;
            Search = search;
            SortList = new List<String>
            {
                "Tittle","Author","Price","Date","Not Sorted"
            };
            if(total%size==0)
            {
                PagesCount = total / size;
            }
            else
            {
                PagesCount = (total / size) + 1;
            }
            if (PagesCount == 0) PagesCount = 1;
        }
        public List<String> SortList { set; get; }
        public List<Book> Books { set; get; }
        public int CurrentPage { set; get; }
        public int PageSize { set; get; }
        public int Total { set; get; }
        public int PagesCount { set; get; }
        public String Sort { set; get; }
        public String Search { set; get; }
    }
}