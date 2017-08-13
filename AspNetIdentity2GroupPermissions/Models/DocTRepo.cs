using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace IdentitySample.Models
{
    public class DocTRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<DocTViewModel> GetAll()
        {
            IList<DocTViewModel> result = new List<DocTViewModel>();

            result = entities.doc_type.Select(Item => new DocTViewModel
            {
                ID = Item.ID,
                Name = Item.Name,
                Comm_ID = Item.committee_ID
            }).ToList();


            return result;
        }

        public IEnumerable<DocTViewModel> Read()
        {
            return GetAll();
        }

        public void Create(DocTViewModel Item)
        {

            var entity = new doc_type();

            entity.Name = Item.Name;
            entity.committee_ID = Item.Comm_ID;

            entities.doc_type.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(DocTViewModel Item)
        {

            var entity = new doc_type();

            entity.ID = Item.ID;
            entity.Name = Item.Name;
            entity.committee_ID = Item.Comm_ID;

            entities.doc_type.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(DocTViewModel Item)
        {


            var entity = new doc_type();

            entity.ID = Item.ID;

            entities.doc_type.Attach(entity);

            entities.doc_type.Remove(entity);

            entities.SaveChanges();

        }

        public DocTViewModel One(Func<DocTViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}