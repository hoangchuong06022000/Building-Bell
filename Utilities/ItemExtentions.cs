using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using SitecoreCaseStudy.Models;

namespace SitecoreCaseStudy.Utilities
{
    public class ItemExtentions
    {
        public Item GetSelectedItemFromDroplistField(Item item, string fieldName, Language language)
        {
            Field field = item.Fields[fieldName];
            if (field == null || string.IsNullOrEmpty(field.Value))
            {
                return null;
            }

            var fieldSource = field.Source ?? string.Empty;
            var selectedItemPath = fieldSource.TrimEnd('/') + "/" + field.Value;
            return item.Database.GetItem(selectedItemPath, language);
        }

        public string GetImageSrc(Item item)
        {
            Sitecore.Data.Fields.ImageField imageField = item.Fields["Image"];
            if (imageField != null && imageField.MediaItem != null)
            {
                Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(image));
            }
            return "";
        }

        public Navigation BuildNavigation(Item item, Language language)
        {
            return new Navigation
            {
                NavigationTitle = item.Fields["Title"].Value,
                NavigationLink = GetURL(item, language),
                ActiveClass = PageContext.Current.Item.ID == item.ID ? "Active" : string.Empty
            };
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

        public MvcHtmlString GetFieldValue(Item item, string fieldName)
        {
            return new MvcHtmlString(Sitecore.Web.UI.WebControls.FieldRenderer.Render(item, fieldName));
        }
    }
}