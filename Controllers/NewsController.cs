using SitecoreCaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreCaseStudy.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult GetNewsList()
        {
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var slideIds = Sitecore.Data.ID.ParseArray(item["News List"]);
            IList<News> listNews = new List<News>();
            foreach (var id in slideIds)
            {
                var news = item.Database.GetItem(id);
                if (news != null)
                {
                    listNews.Add(new News { Item = news });
                }
            }
            return View("~/Views/Renderings/News/NewsList.cshtml", listNews);
        }

        public ActionResult GetNewsDetails()
        {
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var news = new News { Item = item };
            return View("~/Views/Renderings/News/NewsDetails.cshtml", news);
        }
    }
}