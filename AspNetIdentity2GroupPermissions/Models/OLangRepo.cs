using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class OLangRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<OlangViewModel> GetAll()
        {
            IList<OlangViewModel> result = new List<OlangViewModel>();

            result = entities.olanguages.Select(Item => new OlangViewModel
            {
                ID = Item.ID,
                LName = Item.Name,
                Lang_ID=Item.languages_ID
            }).ToList();


            return result;
        }

        public IEnumerable<OlangViewModel> Read()
        {
            return GetAll();
        }

        public void Create(OlangViewModel Item)
        {

            var entity = new olanguage();

            entity.Name = Item.LName;
            entity.languages_ID = Item.Lang_ID;
            entities.olanguages.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(OlangViewModel Item)
        {

            var entity = new olanguage();

            entity.ID = Item.ID;
            entity.Name = Item.LName;
            entity.languages_ID = Item.Lang_ID;
            entities.olanguages.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(OlangViewModel Item)
        {


            var entity = new olanguage();

            entity.ID = Item.ID;

            entities.olanguages.Attach(entity);

            entities.olanguages.Remove(entity);

            entities.SaveChanges();

        }

        public OlangViewModel One(Func<OlangViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}