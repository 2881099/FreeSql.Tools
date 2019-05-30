using DSkin.DirectUI;
using DSkin.Forms;
using FreeSqlTools.Models;

namespace FreeSqlTools.Pages
{
    public class Template : DSkin.Forms.MiniBlinkPage
    {



        


        public Template()
        {
           var res= Curd.Templates.Select.ToList<TemplatesOut>();
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
            Curd.Templates.Insert(new Templates {
                 Title = name
            });
            Data.Clear();
            var res = Curd.Templates.Select.ToList<TemplatesOut>();
            Data.AddRange(res);
            Data.SaveChanges();
        }
    }
}
