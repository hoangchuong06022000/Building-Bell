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
                var config = AppendLanguage();

                tempUrlSet.Add(BuildUrl(homeItem));

                var childrens = homeItem.Axes.GetDescendants();
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

        private static int AppendLanguage()
        {
            return string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")) ? 0 : System.Convert.ToInt32((Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")));
        }

        private Url BuildUrl(Item item)
        {
            return new Url
            {
                Location = "",
                ItemName = item.Name,
                ItemID = item.ID.ToString(),
                ItemPath = item.Paths.FullPath,
                LastMod = item.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
            };
        }
    }
}