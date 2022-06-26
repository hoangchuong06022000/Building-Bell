using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using SitecoreCaseStudy.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreCaseStudy.Models
{
    public class News
    {
        private ItemExtentions itemExtentions = new ItemExtentions();
        public Item Item { get; set; }

        public Language Language { get; set; }

        public MvcHtmlString Image
        {
            get { return itemExtentions.GetFieldValue(Item, "Image"); }
        }

        public string ImageUrl
        {
            get { return itemExtentions.GetImageSrc(Item); }
        }

        public MvcHtmlString Title
        {
            get { return itemExtentions.GetFieldValue(Item, "Title"); }
        }

        public string Date
        {
            get { return Item.Created.ToString("dd'/'MM'/'yyyy"); }
        }
       
        public MvcHtmlString Summary
        {
            get { return itemExtentions.GetFieldValue(Item, "Summary"); }
        }

        public MvcHtmlString Body
        {
            get { return itemExtentions.GetFieldValue(Item, "Body"); }
        }

        public string NewsDetail
        {
            get { return itemExtentions.GetURL(Item, Language); }
        }
    }
}