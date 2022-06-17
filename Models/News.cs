using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
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
        public Item Item { get; set; }

        public Language Language { get; set; }

        public MvcHtmlString Image
        {
            get { return GetFieldValue("Image"); }
        }
        
        public MvcHtmlString Title
        {
            get { return GetFieldValue("Title"); }
        }

        public string Date
        {
            get { return Item.Created.ToString("dd'/'MM'/'yyyy"); }
        }
       
        public MvcHtmlString Summary
        {
            get { return GetFieldValue("Summary"); }
        }

        public MvcHtmlString Body
        {
            get { return GetFieldValue("Body"); }
        }

        public string NewsDetail
        {
            get { return GetURL(Item, Language); }
        }
        // Move to DI
        public string GetURL(Item item, Language language)
        {
            string url = LinkManager.GetItemUrl(item,
              new UrlOptions
              {
                  LanguageEmbedding = LanguageEmbedding.Always,
                  LanguageLocation = LanguageLocation.FilePath,
                  Language = language
              });
            return url;
        }

        private MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(Sitecore.Web.UI.WebControls.FieldRenderer.Render(Item, fieldName));
        }
    }
}