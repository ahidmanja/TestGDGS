using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace IdentitySample.Models
{
    public class FinalTempRepo : IDisposable
    {

        private gdgs1Entities entities = new gdgs1Entities();

        public IList<FinalTempViewModel> GetAll()
        {
            IList<FinalTempViewModel> result = new List<FinalTempViewModel>();

            result = entities.final_temp.Select(Item => new FinalTempViewModel
            {
                ID = Item.ID,
                Name = Item.Name,
                Symbole=Item.Symbole,
                Title = Item.Title,
                Count= Item.Count,
                Reg=Item.Reg,

                //Display
                doctype=Item.category.doc_type.Name,
                comm = Item.category.doc_type.committee.Name,
                temptype = Item.category.doc_type.committee.template_type.Name,
                cat=Item.category.Name,

                //edit
                com_ID = Item.category.doc_type.committee_ID,
                Doctype_ID=Item.category.doc_type.ID,
                temptype_ID=Item.category.doc_type.committee.template_type_ID,
                Cat_ID = Item.categories_ID
            }).ToList();


            return result;
        }

        public IEnumerable<FinalTempViewModel> Read()
        {
            return GetAll();
        }

        public void Create(FinalTempViewModel Item)
        {

            var entity = new final_temp();

            entity.Name = Item.Name;
            entity.Symbole = Item.Symbole;
            entity.Title = Item.Title;
            entity.Count = Item.Symbole.Split('/').Count();
            entity.Reg = RegGenerate(Item.Symbole);
            entity.categories_ID = Item.Cat_ID;

            entities.final_temp.Add(entity);
            entities.SaveChanges();

            Item.ID = entity.ID;

        }
        public string RegGenerate(string sym)
        {
            string result = "";
            string[] str = sym.Split('/');
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == "ISO")
                {
                    str[i] = "[A-Z]{3}";
                }
                if (str[i] == "#")
                {
                    str[i] = "[0-9]+-{0,1}[0-9]*";
                }
                if (str[i] == "@")
                {
                    str[i] = "[0-9]{1,9}";
                }
                if (str[i].Contains("$"))
                {
                    str[i] = str[i].Trim('$');
                    str[i] = str[i] + "[0-9]{1,9}";
                    //str[i].Replace("$", "[1-9]{1,9}");
                }
                if (str[i].Contains("#"))
                {
                    str[i] = str[i].Trim('#');
                    str[i] = str[i] + "[0-9]+-{0,1}[0-9]*";
                    //str[i].Replace("$", "[1-9]{1,9}");
                }
                if (str[i] == "*" || str[i] == "&")
                {
                    str[i] = "[0-9]{4}";
                }
                if (str[i].Contains("#"))
                {
                    str[i].Replace("#", "[0 - 9] + -{ 0,1}[0-9]*");
                }
                else
                {
                    str[i] = "(" + str[i] + ")";
                }

            }
            result = string.Join("\\/",str);
            result = "^" + result + "$";

            return result;
        }

        public void Update(FinalTempViewModel Item)
        {
            var entity = new final_temp();
            entity.ID = Item.ID;
            entity.Name = Item.Name;
            entity.Symbole = Item.Symbole;
            entity.Count = Item.Symbole.Split('/').Count();
            entity.Reg = RegGenerate(Item.Symbole);
            entity.categories_ID = Item.Cat_ID;
            entity.Title = Item.Title;
            entities.final_temp.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
          

            //var entity = new final_temp();

            //entity.ID = Item.ID;
            //entity.Name = Item.Name;
            //entity.Symbole = Item.Symbole;
            //entity.Count = Item.Symbole.Split('/').Count();
            //entity.Reg = RegGenerate(Item.Symbole);
            //entity.categories_ID = Item.Cat_ID;
            //entity.Title = Item.Title;
            //entities.final_temp.Attach(entity);
            //entities.Entry(entity).State = EntityState.Modified;
           entities.SaveChanges();

        }

        public void Destroy(FinalTempViewModel Item)
        {


            var entity = new final_temp();

            entity.ID = Item.ID;

            entities.final_temp.Attach(entity);

            entities.final_temp.Remove(entity);

            entities.SaveChanges();

        }

        public FinalTempViewModel One(Func<FinalTempViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}