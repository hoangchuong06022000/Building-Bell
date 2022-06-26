using SitecoreCaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SitecoreCaseStudy.Controllers
{
    [Route("api/news/getallnews")]
    public class NewsApiController : ApiController
    {
        // GET: NewsApi
        public IHttpActionResult GetAllNews()
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
            var items = database.SelectItems("fast:/sitecore/content/homecasestudy/newslist/*");
            var listNews = new List<NewsAPI>();
            foreach (var item in items)
            {
                if (item != null)
                {
                    var news = new News { Item = item };
                    var newsViewAPI = new NewsAPI
                    {
                        Image = news.Image.ToString(),
                        ImageUrl = news.ImageUrl.ToString(),
                        Title = news.Title.ToString(),
                        Date = news.Date.ToString(),
                        Summary = news.Summary.ToString(),
                        Body = news.Body.ToString()
                    };
                    listNews.Add(newsViewAPI);
                }
            }

            if (listNews.Count == 0)
            {
                return NotFound();
            }

            Sitecore.Diagnostics.Log.Info(string.Format("Request API to Get All News at : {0};", DateTime.Now), this);

            return Ok(listNews);
        }
    }
}