using Sitecore;
using Sitecore.Collections;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using SitecoreCaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreCaseStudy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Renderings/Home/HomePage.cshtml");
        }

        public ActionResult TopNavBar()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            LanguageCollection langColl = LanguageManager.GetLanguages(Context.Database);
            // Move to DI
            foreach (Language language in langColl)
            {
                string url = new News().GetURL(Context.Item, language);
                list.Add(language.Name, url);
            }
            return View("~/Views/Renderings/Shared/TopNavBar.cshtml", list);
        }

        public ActionResult LoginBox()
        {
            return View("~/Views/Renderings/Home/LoginBox.cshtml");
        }

        public ActionResult NewsContentHome()
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
            var item = database.SelectItems("fast:/sitecore/content/homecasestudy/newslist/*").OrderByDescending(x => x.Created).FirstOrDefault();
            var news = new News { Item = item };
            return View("~/Views/Renderings/Home/NewsContentHome.cshtml", news);
        }

        public ActionResult TransactionBlockHome()
        {
            return View("~/Views/Renderings/Home/TransactionBlockHome.cshtml");
        }

        public ActionResult RegisterBox()
        {
            return View("~/Views/Renderings/Home/RegisterBox.cshtml");
        }
    }
}