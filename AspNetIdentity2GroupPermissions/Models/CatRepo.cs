using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class CatRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<CatViewModel> GetAll()
        {
            IList<CatViewModel> result = new List<CatViewModel>();

            result = entities.categories.Select(Item => new CatViewModel
            {
                ID = Item.ID,
                Name = Item.Name,
                doctype = Item.doc_type.Name,
                comm = Item.doc_type.committee.Name,
                temptype = Item.doc_type.committee.template_type.Name,

                com_ID = Item.doc_type.committee_ID,
                temptype_ID = Item.doc_type.committee.template_type_ID,
                DocT_ID = Item.type_ID

            }).ToList();


            return result;
        }

        public IEnumerable<CatViewModel> Read()
        {
            return GetAll();
        }

        public void Create(CatViewModel Item)
        {

            var entity = new category();

            entity.Name = Item.Name;
            entity.type_ID = Item.DocT_ID;

            entities.categories.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(CatViewModel Item)
        {

            var entity = new category();

            entity.ID = Item.ID;
            entity.Name = Item.Name;
            entity.type_ID = Item.DocT_ID;

            entities.categories.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(CatViewModel Item)
        {


            var entity = new category();

            entity.ID = Item.ID;

            entities.categories.Attach(entity);

            entities.categories.Remove(entity);

            entities.SaveChanges();

        }

        public CatViewModel One(Func<CatViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}