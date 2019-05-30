using DSkin.DirectUI;
using DSkin.Forms;
using FreeSqlTools.Models;

namespace FreeSqlTools.Pages
{
    public class Template : DSkin.Forms.MiniBlinkPage
    {



        


        public Template()
        {
           var res= Curd.SetFsql.Select<Templates>().ToList<TemplatesOut>();
            Data.AddRange(res);
            
        }
        MiniBlinkCollection<TemplatesOut> data;
        public MiniBlinkCollection<TemplatesOut> Data
        {
            get
            {
                if (data == null)
                {
                    data = new MiniBlinkCollection<TemplatesOut>(this);
                }
                return data;
            }
        }


        [JSFunction]
        public void AddTemplates(string name)
        {
            Curd.SetFsql.Insert<Templates>(new Templates {
                 Title = name
            }).ExecuteInserted();
            Data.Clear();
            var res = Curd.SetFsql.Select<Templates>().ToList<TemplatesOut>();
            Data.AddRange(res);
            Data.SaveChanges();
        }
    }
}
