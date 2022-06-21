using Sitecore;
using Sitecore.Collections;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using SitecoreCaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SitecoreCaseStudy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Renderings/Home/HomePage.cshtml");
        }

        public ActionResult InitTopNavBar()
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

        public ActionResult Login()
        {
            if (TempData.ContainsKey("loginFail"))
            {
                ViewBag.LoginFailMessage = TempData["loginFail"].ToString();
            }
            if(Session["User"] != null)
            {
                User currentUser = Session["User"] as User;
                return View("~/Views/Renderings/Home/LoginBox.cshtml", currentUser);
            }
            return View("~/Views/Renderings/Home/LoginBox.cshtml");
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            bool isLogin = AuthenticationManager.Login(@"buildingbell\" + userName, password, false);

            if (isLogin)
            {
                User user = Sitecore.Context.User;
                Session["User"] = user;
                return Redirect("HomeCaseStudy");
            }
            TempData["loginFail"] = "Login in error mesage!";
            return Redirect("HomeCaseStudy");
        }

        public ActionResult Logout()
        {
            AuthenticationManager.Logout();
            Session["User"] = null;
            return Redirect("HomeCaseStudy");
        }

        public ActionResult GetNewsContentHome()
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
            var item = database.SelectItems("fast:/sitecore/content/homecasestudy/newslist/*").OrderByDescending(x => x.Created).FirstOrDefault();
            var news = new News { Item = item };
            return View("~/Views/Renderings/Home/NewsContentHome.cshtml", news);
        }

        public ActionResult InitTransactionBlockHome()
        {
            return View("~/Views/Renderings/Home/TransactionBlockHome.cshtml");
        }

        [HttpPost]
        public ActionResult GetTransactionSearchResult(string stringSearch)
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
            Sitecore.Data.Items.Item[] items = database.SelectItems("fast:/sitecore/content/homecasestudy/transactionlist/*[@Location = '%" + stringSearch + "%']");
            IList<Transaction> listTransaction = new List<Transaction>();
            if (items.Count() == 0)
            {
                items = database.SelectItems("fast:/sitecore/content/homecasestudy/transactionlist/*");
            }
            foreach (var transaction in items)
            {
                if (transaction != null)
                {
                    listTransaction.Add(new Transaction { Item = transaction });
                }
            }
            TempData["listTransaction"] = listTransaction;
            return Redirect("TransactionSearchResult");
        }

        public ActionResult Register()
        {
            return View("~/Views/Renderings/Home/RegisterBox.cshtml");
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password)
        {
            string domain = "buildingbell";
            string role = "Access Only News Folder";
            string userName = string.Concat(firstName, lastName);
            userName = string.Format(@"{0}\{1}", domain, userName);
            string newPassword = password;
            try
            {
                if (!Sitecore.Security.Accounts.User.Exists(userName))
                {
                    Membership.CreateUser(userName, newPassword, email);

                    Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.FromName(userName, true);
                    Sitecore.Security.UserProfile userProfile = user.Profile;
                    userProfile.FullName = firstName + " " + lastName;
                    userProfile.Save();

                    UserRoles.FromUser(Sitecore.Security.Accounts.User.FromName(userName, true)).Add(Role.FromName(domain + @"\" + role));

                    return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('Register Successfull!'); location.href='/HomeCaseStudy';</script>" };
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(string.Format("Error in Client.Project.Security.UserMaintenance (AddUser): Message: {0}; Source:{1}", ex.Message, ex.Source), this);
            }
            return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('User Existed!'); location.href='/Register';</script>" };
        }

        public ActionResult RetrievePassword()
        {
            return View("~/Views/Renderings/Home/ForgotPassword.cshtml");
        }

        [HttpPost]
        public ActionResult RetrievePassword(string email)
        {
            return View("~/Views/Renderings/Home/ForgotPassword.cshtml");
        }
    }
}