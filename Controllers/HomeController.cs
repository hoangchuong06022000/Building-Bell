using Sitecore;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.Security.Domains;
using SitecoreCaseStudy.Models;
using SitecoreCaseStudy.Utilities;
using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Sitecore.Data.Fields;

namespace SitecoreCaseStudy.Controllers
{
    public class HomeController : Controller
    {
        private ItemExtentions itemExtentions = new ItemExtentions();
        private MailSender sendMail = new MailSender();
        public ActionResult InitTopNavBar()
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
            var homeItem = database.SelectSingleItem("fast:/sitecore/content/homecasestudy");
            var currentLanguage = Sitecore.Context.Language;
            // Move to DI
            IList<Navigation> listNavigation = new List<Navigation>();
            listNavigation.Add(itemExtentions.BuildNavigation(homeItem, currentLanguage));
            if (homeItem.HasChildren)
            {
                foreach (Item item in homeItem.Children)
                {
                    CheckboxField hideInNavigation = item.Fields["Hide In Navigation"];
                    if (!hideInNavigation.Checked)
                    {
                        listNavigation.Add(itemExtentions.BuildNavigation(item, currentLanguage));
                    }
                }
            }       
            Dictionary<string, string> listLanguage = new Dictionary<string, string>();
            LanguageCollection langColl = LanguageManager.GetLanguages(Context.Database);
            // Move to DI
            foreach (Language language in langColl)
            {
                string url = itemExtentions.GetURL(Context.Item, language);
                listLanguage.Add(language.Name, url);
            }
            dynamic myModel = new ExpandoObject();
            myModel.Navigations = listNavigation;
            myModel.Languages = listLanguage;
            ViewBag.CurrentLanguage = currentLanguage;
            return View("~/Views/Renderings/Shared/TopNavBar.cshtml", myModel);
        }

        public ActionResult Login()
        {
            if (TempData.ContainsKey("loginFailMessage"))
            {
                ViewBag.LoginFailMessage = TempData["loginFailMessage"].ToString();
            }
            if (TempData.ContainsKey("passwordChanged"))
            {
                ViewBag.PasswordChanged = TempData["passwordChanged"].ToString();
            }
            if (Session["User"] != null)
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
            TempData["loginFailMessage"] = "Username or Password incorrect!!";
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
            var quantityOfNews = database.GetItem("/sitecore/content/homecasestudy/newslist").Fields["QuantityOfNews"].Value;
            var items = database.SelectItems("fast:/sitecore/content/homecasestudy/newslist/*").OrderByDescending(x => x.Created).Take(int.Parse(quantityOfNews));
            IList<News> newsList = new List<News>();
            foreach (var news in items)
            {
                if (news != null)
                {
                    newsList.Add(new News { Item = news });
                }
            }
            return View("~/Views/Renderings/Home/NewsContentHome.cshtml", newsList);
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
            if (TempData.ContainsKey("rePasswordIncorrect"))
            {
                ViewBag.RePasswordIncorrect = TempData["rePasswordIncorrect"].ToString();
            }
            if (TempData.ContainsKey("existedEmail"))
            {
                ViewBag.ExistedEmail = TempData["existedEmail"].ToString();
            }
            return View("~/Views/Renderings/Home/RegisterBox.cshtml");
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password, string rePassword)
        {
            string domain = "buildingbell";
            string role = "Contributor";
            string userName = string.Concat(firstName, lastName);
            userName = string.Format(@"{0}\{1}", domain, userName);
            if(rePassword != password)
            {
                TempData["rePasswordIncorrect"] = "Re-Password Incorrect!!";
                return Redirect("Register");
            }
            var listUserByDomain = Domain.GetDomain(domain).GetUsers();
            IList<string> listEmail = new List<string>();
            foreach (var user in listUserByDomain)
            {
                var emailUser = user.Profile.Email;
                if (emailUser != "")
                {
                    listEmail.Add(emailUser);
                }
            }
            if (listEmail.Contains(email))
            {
                TempData["existedEmail"] = "Email is existed!!";
                return Redirect("Register");
            }
            try
            {
                if (!Sitecore.Security.Accounts.User.Exists(userName))
                {
                    Membership.CreateUser(userName, password, email);

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
            if (TempData.ContainsKey("invalidEmail"))
            {
                ViewBag.InvalidEmail = TempData["invalidEmail"].ToString();
            }
            return View("~/Views/Renderings/Home/ForgotPasswordPage.cshtml");
        }

        [HttpPost]
        public ActionResult RetrievePassword(string email)
        {
            var userName = Membership.GetUserNameByEmail(email);
            if (userName == null)
            {
                TempData["invalidEmail"] = "Invalid email address!!";
                return Redirect("ForgotPassword");
            }
            var user = Membership.GetUser(userName);         
            sendMail.SendOTP("Your OTP:", email);
            TempData["user"] = user;
            return Redirect("SetNewPassword");
        }

        public ActionResult SetNewPassword()
        {
            if (TempData.ContainsKey("otpIncorrect"))
            {
                ViewBag.OTPIncorrect = TempData["otpIncorrect"].ToString();
            }
            return View("~/Views/Renderings/Home/SetNewPasswordPage.cshtml");
        }

        [HttpPost]
        public ActionResult SetNewPassword(string newPassword, string otp)
        {
            var user = TempData["user"] as MembershipUser;          
            user.ChangePassword(user.ResetPassword(), newPassword);
            string otpCheck = sendMail.getOTP();
            if (otpCheck != otp)
            {
                TempData["otpIncorrect"] = "OTP Incorrect!!";
                return Redirect("SetNewPassword");
            }
            TempData["passwordChanged"] = "Your password is changed!!";
            return Redirect("HomeCaseStudy");
        }
    }
}