using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using SitecoreCaseStudy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreCaseStudy.Models
{
    public class Transaction
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

        public MvcHtmlString Location
        {
            get { return itemExtentions.GetFieldValue(Item, "Location"); }
        }

        public MvcHtmlString Summary
        {
            get { return itemExtentions.GetFieldValue(Item, "Summary"); }
        }

        public MvcHtmlString NumberOfFloors
        {
            get { return itemExtentions.GetFieldValue(Item, "NumberOfFloors"); }
        }

        public MvcHtmlString NumberOfBedRooms
        {
            get { return itemExtentions.GetFieldValue(Item, "NumberOfBedRooms"); }
        }

        public MvcHtmlString NumberOfBathRooms
        {
            get { return itemExtentions.GetFieldValue(Item, "NumberOfBathRooms"); }
        }

        public MvcHtmlString PropertyCondition
        {
            get { return itemExtentions.GetFieldValue(Item, "PropertyCondition"); }
        }

        public MvcHtmlString Area
        {
            get { return itemExtentions.GetFieldValue(Item, "Area"); }
        }

        public MvcHtmlString Cost
        {
            get { return itemExtentions.GetFieldValue(Item, "Cost"); }
        }

        public string FormatedCost
        {
            get { return string.Format("{0:#,##0}", Double.Parse(Cost.ToString())); }
        }

        public string TransactionDetail
        {
            get { return itemExtentions.GetURL(Item, Language); }
        }

        public string TransactionCategoryName
        {
            get { return itemExtentions.GetSelectedItemFromDroplistField(Item, "TransactionCategory").Fields["TransactionCategoryName"].Value; }
        }

        public string TransactionTypeName 
        {
            get { return itemExtentions.GetSelectedItemFromDroplistField(Item, "TransactionType").Fields["TransactionTypeName"].Value; }
        }
    }
}