using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooksCatalogeMVC.Controllers
{
    public class ErrorPageHandlerController : Controller
    {
        // GET: ErrorPageHandler
        public ActionResult Oops(int id)
        {
            Response.StatusCode = id;

            return View("Error");
        }
    }
}