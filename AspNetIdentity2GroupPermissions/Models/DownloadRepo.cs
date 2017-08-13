using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class DownloadRepo :IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<DownloadViewModel> GetAll()
        {
            IList<DownloadViewModel> result = new List<DownloadViewModel>();

            result = entities.final_temp.Select(Item => new DownloadViewModel
            {
                ID = Item.ID,
                File = Item.Name,
                Category = Item.category.Name,
                Comm=Item.category.doc_type.committee.Name,
                Type= Item.category.doc_type.Name,
            }).ToList();


            return result;
        }
        public IEnumerable<DownloadViewModel> Read()
        {
            return GetAll();
        }
        public void Dispose()
        {
            entities.Dispose();
        }

    }
}