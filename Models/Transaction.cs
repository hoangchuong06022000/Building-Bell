using Sitecore.Data.Fields;
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
    public class Transaction
    {
        public Item Item { get; set; }

        public Language Language { get; set; }

        public MvcHtmlString Image
        {
            get { return GetFieldValue("Image"); }
        }
        // Move to DI
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

        public string ImageUrl
        {
            get { return GetImageSrc(Item); }
        }

        public MvcHtmlString Location
        {
            get { return GetFieldValue("Location"); }
        }

        public MvcHtmlString Summary
        {
            get { return GetFieldValue("Summary"); }
        }

        public MvcHtmlString NumberOfFloors
        {
            get { return GetFieldValue("NumberOfFloors"); }
        }

        public MvcHtmlString NumberOfBedRooms
        {
            get { return GetFieldValue("NumberOfBedRooms"); }
        }

        public MvcHtmlString NumberOfBathRooms
        {
            get { return GetFieldValue("NumberOfBathRooms"); }
        }

        public MvcHtmlString PropertyCondition
        {
            get { return GetFieldValue("PropertyCondition"); }
        }

        public MvcHtmlString Area
        {
            get { return GetFieldValue("Area"); }
        }

        public MvcHtmlString Cost
        {
            get { return GetFieldValue("Cost"); }
        }

        public string FormatedCost
        {
            get { return string.Format("{0:#,##0}", Double.Parse(Cost.ToString())); }
        }

        public string TransactionDetail
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

        public Item GetSelectedItemFromDroplistField(Item item, string fieldName)
        {
            Field field = item.Fields[fieldName];
            if (field == null || string.IsNullOrEmpty(field.Value))
            {
                return null;
            }

            var fieldSource = field.Source ?? string.Empty;
            var selectedItemPath = fieldSource.TrimEnd('/') + "/" + field.Value;
            return item.Database.GetItem(selectedItemPath);
        }

        public string TransactionCategoryName
        {
            get { return GetSelectedItemFromDroplistField(Item, "TransactionCategory").Fields["TransactionCategoryName"].Value; }
        }

        public string TransactionTypeName 
        {
            get { return GetSelectedItemFromDroplistField(Item, "TransactionType").Fields["TransactionTypeName"].Value; }
        }

        private MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(Sitecore.Web.UI.WebControls.FieldRenderer.Render(Item, fieldName));
        }
    }
}