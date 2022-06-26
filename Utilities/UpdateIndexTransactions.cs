using Sitecore.Data.Items;
using Sitecore.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;

namespace SitecoreCaseStudy.Utilities
{
    public class UpdateIndexTransactions
    {
        public void Execute(Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            try
            {
                string indexName = "sitecore_transactions_index";
                Update(indexName, items);
                Sitecore.Diagnostics.Log.Info(string.Format("Scheduler Info: Command ID: {0}, Schedule ID: {1}, Number of transactions: {2}", commandItem.ID, scheduleItem.ID, items.Count()), this);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Scheduler Exception: " + ex.InnerException.Message, this);
            }
        }

        private void Update(string indexName, ICollection<Item> items)
        {
            var index = ContentSearchManager.GetIndex(indexName);
            foreach (var item in items)
            {
                var uniqueId = new SitecoreItemUniqueId(item.Uri);
                index.Update(uniqueId);
                Refresh(indexName, item);
            }
        }

        private void Refresh(string indexName, Item item)
        {
            var index = ContentSearchManager.GetIndex(indexName);
            var indexableItem = (SitecoreIndexableItem)item;
            index.Refresh(indexableItem);
        }
    }
}