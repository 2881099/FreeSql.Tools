using DSkin.DirectUI;
using DSkin.Forms;
using FreeSqlTools.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task delTemplate(string id)
        {
            if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
            {
                if(Curd.TaskBuild.Select.Any(a => a.TemplatesId == gid))
                {
                    InvokeJS("Helper.ui.message.error('当前模板有任务构建,删除失败.');");
                    return;
                }
                Curd.Templates.Delete(gid);
                var _temp = Data.FirstOrDefault(a => a.Id == gid);
                Data.Remove(_temp);
                Data.SaveChanges();
                InvokeJS("departments.bootstrapTable('load', page.Data);$('[data-toggle=\"tooltip\"]').tooltip();");
                InvokeJS("Helper.ui.message.success('删除数据库信息成功');");
                await Task.FromResult(0);
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
