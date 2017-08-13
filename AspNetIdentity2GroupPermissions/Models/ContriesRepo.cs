using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class ContriesRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<CountriesViewModel> GetAll()
        {
            IList<CountriesViewModel> result = new List<CountriesViewModel>();

            result = entities.countries.Select(Item => new CountriesViewModel
            {
                ID = Item.ID,
                Article=Item.Article,
                Article1=Item.Article1,
                Name = Item.Name,
                SName=Item.Short_Name,
                ISO=Item.ISO,
                Lang_ID = Item.languages_ID
            }).ToList();


            return result;
        }

        public IEnumerable<CountriesViewModel> Read()
        {
            return GetAll();
        }

        public void Create(CountriesViewModel Item)
        {

            var entity = new country();

            entity.Name = Item.Name;
            entity.Short_Name = Item.SName;
            entity.Article = Item.Article;
            entity.Article1 = Item.Article1;
            entity.ISO = Item.ISO;
            entity.languages_ID = Item.Lang_ID;

            entities.countries.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }

        public void Update(CountriesViewModel Item)
        {
            try
            {

                var entity = new country();

                entity.ID = Item.ID;
                entity.Name = Item.Name;
                entity.Short_Name = Item.SName;
                entity.Article = Item.Article;
                entity.Article1 = Item.Article1;
                entity.ISO = Item.ISO;
                entity.languages_ID = Item.Lang_ID;

                entities.countries.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
           
            }
        }

        public void Destroy(CountriesViewModel Item)
        {


            var entity = new country();

            entity.ID = Item.ID;

            entities.countries.Attach(entity);

            entities.countries.Remove(entity);

            entities.SaveChanges();

        }

        public CountriesViewModel One(Func<CountriesViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}