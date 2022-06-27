using PagedList;
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
        public ActionResult GetNewsList(int? page)
        {
            if (page == null) page = 1;
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var slideIds = Sitecore.Data.ID.ParseArray(item["News List"]);
            var listNews = new List<News>();
            foreach (var id in slideIds)
            {
                var news = item.Database.GetItem(id);
                if (news != null)
                {
                    listNews.Add(new News { Item = news });
                }
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("~/Views/Renderings/News/NewsList.cshtml", listNews.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetNewsDetails()
        {
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            if (item == null)
            {
                Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
                item = database.SelectItems("fast:/sitecore/content/homecasestudy/newslist/*").FirstOrDefault();
            }
            var news = new News { Item = item };
            return View("~/Views/Renderings/News/NewsDetails.cshtml", news);
        }
    }
}