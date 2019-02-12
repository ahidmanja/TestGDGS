using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;


namespace IdentitySample.Models
{
    public class DataRepo
    {
        private gdgs1Entities entities = new gdgs1Entities();

        public IList<DataViewModel> GetAll()
        {
            IList<DataViewModel> result = new List<DataViewModel>();

            result = entities.data.Select(Item => new DataViewModel
            {
                ID = Item.Id,
                Tlang = Item.tlang,
                Olang = Item.olang,
                Sdate = Item.sdate,
                Anum = Item.anum,
                Atitle = Item.atitle,
                Count = Item.count,
                Prep = Item.prep,
                Stitle = Item.stitle,
                Gdoc = Item.gdoc,
                Bar = Item.bar,
                Symh = Item.symh,
                Dist = Item.dist,
                Date = Item.date,
                FName = Item.file,
                Ldate = Item.ldate,
                Dname = Item.dname,
                Loca = Item.loca,
                Snum = Item.snum,
                Mnum = Item.mnum,
                Org = Item.org,
                Entity = Item.entity,
                DocType = Item.doctype,
                Category = Item.category,
                Lname1 = Item.lname1,
                Lname2 = Item.lname2,
                Subcat = Item.subcat

            }).ToList();


            return result;
        }
     


        public IEnumerable<DataViewModel> Read()
        {
       


            return GetAll();
        }
      

        public void Create(DataViewModel Item)
        {

            var entity = new datum();

            entity.tlang = Item.Tlang;
               entity.olang = Item.Olang;
            entity.sdate = Item.Sdate;
            entity.anum = Item.Anum;
            entity.atitle = Item.Atitle;
            entity.count = Item.Count;
            entity.prep = Item.Prep;
            entity.stitle = Item.Stitle;
            entity.gdoc = Item.Gdoc;
            entity.bar = Item.Bar;
            entity.symh = Item.Symh;
            entity.dist = Item.Dist;
            entity.date = Item.Date;
            entity.file = Item.FName;
            entity.ldate = Item.Ldate;
            entity.dname = Item.Dname;
            entity.loca = Item.Loca;
            entity.snum = Item.Snum;
              entity.mnum = Item.Mnum;
            entity.org = Item.Org;
            entity.entity = Item.Entity;
            entity.doctype = Item.DocType;
            entity.category = Item.Category;
            entity.lname1 = Item.Lname1;
            entity.lname2 = Item.Lname2;
            entity.subcat = Item.Subcat;

            try
            {
                entities.data.Add(entity);
                entities.SaveChanges();
                Item.ID = entity.Id;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            //entities.data.Add(entity);
            //entities.SaveChanges();

       

        }

        public void Update(DataViewModel Item)
        {

            var entity = new datum();

            entity.Id = Item.ID;
            entity.tlang = Item.Tlang;
            entity.olang = Item.Olang;
            entity.sdate = Item.Sdate;
            entity.anum = Item.Anum;
            entity.atitle = Item.Atitle;
            entity.count = Item.Count;
            entity.prep = Item.Prep;
            entity.stitle = Item.Stitle;
            entity.gdoc = Item.Gdoc;
            entity.bar = Item.Bar;
            entity.symh = Item.Symh;
            entity.dist = Item.Dist;
            entity.date = Item.Date;
            entity.file = Item.FName;
            entity.ldate = Item.Ldate;
            entity.dname = Item.Dname;
            entity.loca = Item.Loca;
            entity.snum = Item.Snum;
            entity.mnum = Item.Mnum;
            entity.org = Item.Org;
            entity.entity = Item.Entity;
            entity.doctype = Item.DocType;
            entity.category = Item.Category;
            entity.lname1 = Item.Lname1;
            entity.lname2 = Item.Lname2;
            entity.subcat = Item.Subcat;
            entities.data.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();

        }

        public void Destroy(DataViewModel Item)
        {


            var entity = new datum();

            entity.Id = Item.ID;

            entities.data.Attach(entity);

            entities.data.Remove(entity);

            entities.SaveChanges();

        }

        public DataViewModel One(Func<DataViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}