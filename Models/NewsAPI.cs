using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;


namespace SitecoreCaseStudy.Models
{
    public class NewsAPI
    {
        public string Image { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Summary { get; set; }

        public string Body { get; set; }
    }
}