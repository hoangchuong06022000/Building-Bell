using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreCaseStudy.Models
{
    public class TransactionType
    {
        public Item Item { get; set; }

        public Language Language { get; set; }

        public MvcHtmlString TransactionTypeName
        {
            get { return GetFieldValue("TransactionCategoryName"); }
        }

        public string TransactionTypeDetail
        {
            get { return GetURL(Item, Language); }
        }

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