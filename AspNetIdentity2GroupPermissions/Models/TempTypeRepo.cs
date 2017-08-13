using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class TempTypeRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<TempTypeViewModel> GetAll()
        {
            IList<TempTypeViewModel> result = new List<TempTypeViewModel>();

            result = entities.template_type.Select(Item => new TempTypeViewModel
            {
                ID = Item.ID,
                TName = Item.Name
            }).ToList();


            return result;
        }

        public IEnumerable<TempTypeViewModel> Read()
        {
            return GetAll();
        }

        public void Create(TempTypeViewModel Item)
        {

            var entity = new template_type();

            entity.Name = Item.TName;
            entities.template_type.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(TempTypeViewModel Item)
        {

            var entity = new template_type();

            entity.ID = Item.ID;
            entity.Name = Item.TName;

            entities.template_type.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(TempTypeViewModel Item)
        {


            var entity = new template_type();

            entity.ID = Item.ID;

            entities.template_type.Attach(entity);

            entities.template_type.Remove(entity);

            entities.SaveChanges();

        }

        public TempTypeViewModel One(Func<TempTypeViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}