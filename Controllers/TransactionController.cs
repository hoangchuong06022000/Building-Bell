using PagedList;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using SitecoreCaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreCaseStudy.Controllers
{
    public class TransactionController : Controller
    {
        private static IList<Transaction> listTransaction;
        // GET: Transaction
        public ActionResult GetTransactionList(int? page)
        {
            if (page == null) page = 1;
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var slideIds = Sitecore.Data.ID.ParseArray(item["Transaction List"]);
            IList<Transaction> listTransaction = new List<Transaction>();
            foreach (var id in slideIds)
            {
                var transaction = item.Database.GetItem(id);
                if (transaction != null)
                {
                    listTransaction.Add(new Transaction { Item = transaction });
                }
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View("~/Views/Renderings/Transaction/TransactionList.cshtml", listTransaction.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetTransactionSearchResult(int? page)
        {
            if (TempData.ContainsKey("notFound"))
            {
                ViewBag.NotFound = TempData["notFound"].ToString();
                return View("~/Views/Renderings/Transaction/TransactionList.cshtml");
            }
            if (page == null) page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            if (TempData.ContainsKey("listTransaction"))
            {
                listTransaction = (List<Transaction>)TempData["listTransaction"];
            }
            return View("~/Views/Renderings/Transaction/TransactionList.cshtml", listTransaction.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult GetTransactionSearchResult(string stringSearch)
        {
            using (var context = ContentSearchManager.GetIndex("sitecore_web_index").CreateSearchContext())
            {
                var isNumber = Int32.TryParse(stringSearch, out int number);
                var currentLanguage = Sitecore.Context.Language.ToString();

                var items = context.GetQueryable<SearchResultItem>()
                    .Where(p => p.Path.StartsWith("/sitecore/Content/homecasestudy/transactionlist"))
                    .Where(p => p.TemplateName == "Transaction")
                    .Where(p => p["location_t"].Contains(stringSearch) || p["propertycondition_t"].Contains(stringSearch)
                    || p["transactiontype_s"].Contains(stringSearch) || p["transactioncategory_s"].Contains(stringSearch)
                    || (isNumber && (p["cost_tf"] == stringSearch)) || (isNumber && (p["area_tf"] == stringSearch))
                    || (isNumber && (p["numberofbedrooms_tf"] == stringSearch)) || (isNumber && (p["numberofbathrooms_tf"] == stringSearch))
                    || (isNumber && (p["numberoffloors_tf"] == stringSearch)))
                    .Where(p => p.Language == currentLanguage).ToList();
                IList<Transaction> listTransaction = new List<Transaction>();
                if (items.Count() == 0)
                {
                    TempData["notFound"] = "'" + stringSearch + "' not found!!";
                }
                foreach (var transaction in items)
                {
                    if (transaction != null)
                    {
                        listTransaction.Add(new Transaction { Item = transaction.GetItem() });
                    }
                }
                TempData["listTransaction"] = listTransaction;
                return Redirect("TransactionSearchResult");
            }          
        }

        public ActionResult GetTransactionDetails()
        {
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var transaction = new Transaction { Item = item };
            return View("~/Views/Renderings/Transaction/TransactionDetails.cshtml", transaction);
        }
    }
}