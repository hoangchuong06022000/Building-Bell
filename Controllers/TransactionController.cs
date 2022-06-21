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
        // GET: Transaction
        public ActionResult GetTransactionList()
        {
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
            
            return View("~/Views/Renderings/Transaction/TransactionList.cshtml", listTransaction);
        }

        public ActionResult GetTransactionSearchResult()
        {
            IList<Transaction> listTransaction = (IList<Transaction>)TempData["listTransaction"];
            return View("~/Views/Renderings/Transaction/TransactionList.cshtml", listTransaction);
        }

        [HttpPost]
        public ActionResult GetTransactionSearchResult(string stringSearch)
        {
            Sitecore.Data.Database database = Sitecore.Data.Database.GetDatabase("web");
            Sitecore.Data.Items.Item[] items = database
                .SelectItems("fast:/sitecore/content/homecasestudy/transactionlist/*[@Location = '%" + stringSearch + "%'"
                + " or @Summary = '%" + stringSearch + "%'"
                + " or @NumberOfFloors = '%" + stringSearch + "%'"
                + " or @NumberOfBedRooms = '%" + stringSearch + "%'"
                + " or @NumberOfBathRooms = '%" + stringSearch + "%'"
                + " or @Area = '%" + stringSearch + "%'"
                + " or @Cost = '%" + stringSearch + "%'"
                + " or @TransactionCategoryName = '%" + stringSearch + "%'"
                + " or @TransactionTypeName = '%" + stringSearch + "%'"
                + " or @PropertyCondition = '%" + stringSearch + "%']");
            IList<Transaction> listTransaction = new List<Transaction>();
            if(items.Count() == 0)
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

        public ActionResult GetTransactionDetails()
        {
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var transaction = new Transaction { Item = item };
            return View("~/Views/Renderings/Transaction/TransactionDetails.cshtml", transaction);
        }
    }
}