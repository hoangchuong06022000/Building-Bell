using Sitecore.Data.Items;
using SitecoreCaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SitecoreCaseStudy.Utilities
{
    public class SitemapHandler : IHttpHandler
    {
        private ItemExtentions itemExtentions = new ItemExtentions();
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if ((Sitecore.Context.Site == null) || Sitecore.Context.Database == null)
            {

            }
            try
            {
                var homeItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
                var xmlSerialize = new XmlSerializer(typeof(UrlSet));
                var urlSet = new UrlSet();

                var tempUrlSet = new List<Url>();

                tempUrlSet.Add(BuildUrl(homeItem));

                var childrens = homeItem.Axes.GetDescendants().Where(x => x.TemplateName != "Transaction Category"
                || x.TemplateName != "Transaction Category List" || x.TemplateName != "Transaction Type"
                || x.TemplateName != "Transaction Type List");

                var finalCollection = childrens.ToList();

                tempUrlSet.AddRange(finalCollection.Select(childItem => BuildUrl(childItem)));

                urlSet.UrlColection = tempUrlSet;

                var response = HttpContext.Current.Response;
                response.AddHeader("Content-Type", "text/xml");
                xmlSerialize.Serialize(response.OutputStream, urlSet);
                HttpContext.Current.Response.End();
            }
            catch (Exception)
            {

            }
        }

        private Url BuildUrl(Item item)
        {
            return new Url
            {
                Location = GetFullLink(itemExtentions.GetURL(item, language: Sitecore.Globalization.Language.Current)),
                ItemName = item.Name,
                ItemID = item.ID.ToString(),
                ItemPath = item.Paths.FullPath,
                LastMod = item.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
            };
        }

        private string GetFullLink(string url)
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + url;
        }
    }
}