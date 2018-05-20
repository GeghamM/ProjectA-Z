using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace BooksCatalogeMVC.CustomHtlmHelpers
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString DisplayImage(this HtmlHelper helper, String path,String classes = null)
        {
            return helper.DisplayImage(path, new object(), classes);
        }
        public static MvcHtmlString DisplayImage(this HtmlHelper helper, String path, object HtmlAttributes, String classes = null)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(HtmlAttributes) as IDictionary<string, object>;
            TagBuilder tb = new TagBuilder("img");
            tb.MergeAttributes(attributes);
            tb.Attributes.Add("src", path);
            tb.Attributes.Add("alt", "Cover");
            if (classes != null){
                tb.Attributes.Add("class", classes);
            }
            else {
                tb.Attributes.Add("class", "image-grid cover");
            }
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }

        //Pagination button
        public static MvcHtmlString PageNext(this HtmlHelper Html, int current, int size, int total, string sort, string Search)
        {
            if (total - ((size * current) + 1) <= 0)
            {
                return Html.ActionLink("Next", "Index",
                new { page = current + 1, SortDown = sort, SearchText = Search },
                new { @class = "btn btn-primary", disabled = "disabled" });
            }
            else
            {
                return Html.ActionLink("Next", "Index",
                new { page = current + 1, SortDown = sort, SearchText = Search },
                new { @class = "btn btn-primary" });
            }
        }

        //Pagination button
        public static MvcHtmlString PagePrev(this HtmlHelper Html, int current, int size, int total, string sort, string Search)
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

        //I can't find language with DropDownList and make my own one. :) :)
        public static MvcHtmlString MyDropDownList(this HtmlHelper Html, string name, List<String> slist, string selected)
        {
            return Html.MyDropDownList(name, slist, selected, new object());
        }
        public static MvcHtmlString MyDropDownList(this HtmlHelper Html, string name, List<String> slist, string selected, object HtmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(HtmlAttributes) as IDictionary<string, object>;
            var select = new TagBuilder("select");
            select.MergeAttributes(attributes);
            select.Attributes.Add("name", name);
            select.Attributes.Add("onchange", "this.form.submit()");
            select.GenerateId(name);
            int count = slist.Count();
            List<TagBuilder> option = new List<TagBuilder>();
            for (int i = 0; i < count; i++)
            {
                option.Add(new TagBuilder("option"));
                option[i].InnerHtml = slist[i];
                if (selected == slist[i])
                {
                    option[i].MergeAttribute("selected", "");
                }
            }
            string inner = "";
            for (int i = 0; i < count; i++)
            {
                inner += option[i];
            }
            select.InnerHtml = inner;
            return new MvcHtmlString(select.ToString(TagRenderMode.Normal));
        }
    }
}