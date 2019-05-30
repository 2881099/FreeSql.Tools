using DSkin.DirectUI;
using FreeSqlTools.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Pages
{
    public class generate : DSkin.Forms.MiniBlinkPage
    {


        string path = Environment.CurrentDirectory + "\\generate";

        [JSFunction]
        public void GenerateCode()
        {
            var strjson = File.ReadAllText(Environment.CurrentDirectory + "\\demo.json");
       
            var res = Curd.SetFsql.Select<Templates>().ToOne();
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(strjson);
            var resHtml = Engine.Razor.RunCompile(res.Code,Guid.NewGuid().ToString("N"), null, model);


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText($"{path}\\teb.cs", resHtml);
            Process.Start(path);
        }


    }
}
