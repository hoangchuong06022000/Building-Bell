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
        public ActionResult TransactionList()
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

        public ActionResult TransactionDetails()
        {
            var item = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.Item;
            var transaction = new Transaction { Item = item };
            return View("~/Views/Renderings/Transaction/TransactionDetails.cshtml", transaction);
        }
    }
}