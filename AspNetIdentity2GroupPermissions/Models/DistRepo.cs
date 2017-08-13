using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class DistRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<DistrbutionViewModel> GetAll()
        {
            IList<DistrbutionViewModel> result = new List<DistrbutionViewModel>();

            result = entities.distrbutions.Select(Item => new DistrbutionViewModel
            {
                ID = Item.ID,
                DName = Item.Name,
                Lang_ID=Item.languages_ID
            }).ToList();


            return result;
        }

        public IEnumerable<DistrbutionViewModel> Read()
        {
            return GetAll();
        }

        public void Create(DistrbutionViewModel Item)
        {

            var entity = new distrbution();

            entity.Name = Item.DName;
            entity.languages_ID = Item.Lang_ID;

            entities.distrbutions.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(DistrbutionViewModel Item)
        {

            var entity = new distrbution();

            entity.ID = Item.ID;
            entity.Name = Item.DName;
            entity.languages_ID = Item.Lang_ID;

            entities.distrbutions.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(DistrbutionViewModel Item)
        {


            var entity = new distrbution();

            entity.ID = Item.ID;

            entities.distrbutions.Attach(entity);

            entities.distrbutions.Remove(entity);

            entities.SaveChanges();

        }

        public DistrbutionViewModel One(Func<DistrbutionViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}