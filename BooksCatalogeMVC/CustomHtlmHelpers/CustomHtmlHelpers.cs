using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace BooksCatalogeMVC.CustomHtlmHelpers
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlString DisplayImage(this HtmlHelper helper, String path)
        {
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src",path);
            tb.Attributes.Add("alt", "Cover");
            tb.Attributes.Add("class", "image-grid cover");
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString PageNext(this HtmlHelper Html, int current, int size, int total,string sort,string Search)
        {
            if (total - ((size * current)+1) <= 0)
            {
                return Html.ActionLink("Next", "Index",
                new { page = current + 1, SortDown=sort, SearchText=Search },
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Next", "Index",
                new { page = current + 1, SortDown = sort, SearchText = Search },
                new { @class = "btn btn-primary" });
            }
        }

        public static MvcHtmlString PagePrev(this HtmlHelper Html, int current, int size, int total,string sort,string Search)
        {
            if (current < 2)
            {
                return Html.ActionLink("Previous", "Index",
                new { page = current - 1, SortDown = sort, SearchText = Search },
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Previous", "Index",
                new { page = current - 1, SortDown = sort, SearchText = Search },
                new { @class = "btn btn-primary" });
            }
        }
    }
}