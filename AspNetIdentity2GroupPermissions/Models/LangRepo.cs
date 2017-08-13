using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class LangRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<LangViewModel> GetAll()
        {
            IList<LangViewModel> result = new List<LangViewModel>();

            result = entities.languages.Select(Item => new LangViewModel
            {
                ID = Item.ID,
                LName = Item.Lang_Name
            }).ToList();


            return result;
        }

        public IEnumerable<LangViewModel> Read()
        {
            return GetAll();
        }

        public void Create(LangViewModel Item)
        {

            var entity = new language();

            entity.Lang_Name = Item.LName;
            entities.languages.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(LangViewModel Item)
        {

            var entity = new language();

            entity.ID = Item.ID;
            entity.Lang_Name = Item.LName;

            entities.languages.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(LangViewModel Item)
        {


            var entity = new language();

            entity.ID = Item.ID;

            entities.languages.Attach(entity);

            entities.languages.Remove(entity);

            entities.SaveChanges();

        }

        public LangViewModel One(Func<LangViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}