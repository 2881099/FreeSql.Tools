using DSkin.DirectUI;
using FreeSqlTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreeSqlTools.Pages
{
    public class editCode : DSkin.Forms.MiniBlinkPage
    {

        Guid id;
        Templates templates;
        protected override void OnLoad()
        {
            base.OnLoad();
            var a= this.Url.IndexOf("=")+1;
            Guid.TryParse(this.Url.Substring(a, 36), out id);
            templates = Curd.Templates.Select.WhereDynamic(id).ToOne();
         
        }

        protected editCode()
        {
           
        }

        

        [JSFunction]
        public string GetCode() => templates.Code ?? @" Console.WriteLine(""请开始你的表演?"");";
        [JSFunction]
        public string GetTitle() => templates.Title;



        [JSFunction]
        public void UpdateTemplates(string text)
        {
       
          var res= Curd.Templates.UpdateDiy.WhereDynamic(id).Set(a => a.Code, text)
                .Set(a => a.EditTime, DateTime.Now).ExecuteAffrows();


        }

    }
}
