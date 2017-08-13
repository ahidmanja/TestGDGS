using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class OrgRepo: IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<OrgViewModel> GetAll()
        {
            IList<OrgViewModel> result = new List<OrgViewModel>();

            result = entities.organaizations.Select(org => new OrgViewModel
            {
                ID = org.ID,
                OName = org.Name
            }).ToList();


            return result;
        }

        public IEnumerable<OrgViewModel> Read()
        {
            return GetAll();
        }

        public void Create(OrgViewModel org)
        {

                var entity = new organaization();

                entity.Name = org.OName;
                entities.organaizations.Add(entity);
                entities.SaveChanges();

                org.ID = entity.ID;
            
        }

        public void Update(OrgViewModel org)
        {
          
                var entity = new organaization();

                entity.ID = org.ID;
                entity.Name = org.OName;

                entities.organaizations.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
          
        }

        public void Destroy(OrgViewModel org)
        {
         
           
                var entity = new organaization();

                entity.ID = org.ID;

                entities.organaizations.Attach(entity);

                entities.organaizations.Remove(entity);

                entities.SaveChanges();
           
        }

        public OrgViewModel One(Func<OrgViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}