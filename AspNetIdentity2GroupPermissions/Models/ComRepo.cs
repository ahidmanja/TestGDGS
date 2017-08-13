using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class ComRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<CommitteeViewModel> GetAll()
        {
            IList<CommitteeViewModel> result = new List<CommitteeViewModel>();

            result = entities.committees.Select(Item => new CommitteeViewModel
            {
                ID = Item.ID,
                Name = Item.Name,
                TType_ID = Item.template_type_ID
            }).ToList();


            return result;
        }

        public IEnumerable<CommitteeViewModel> Read()
        {
            return GetAll();
        }

        public void Create(CommitteeViewModel Item)
        {

            var entity = new committee();

            entity.Name = Item.Name;
            entity.template_type_ID = Item.TType_ID;

            entities.committees.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(CommitteeViewModel Item)
        {

            var entity = new committee();

            entity.ID = Item.ID;
            entity.Name = Item.Name;
            entity.template_type_ID = Item.TType_ID;

            entities.committees.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(CommitteeViewModel Item)
        {


            var entity = new committee();

            entity.ID = Item.ID;

            entities.committees.Attach(entity);

            entities.committees.Remove(entity);

            entities.SaveChanges();

        }

        public CommitteeViewModel One(Func<CommitteeViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}