using DSkin.DirectUI;
using FreeSqlTools.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeSqlTools.Pages
{


    public class Ztree
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public bool open { get; set; } = false;
        public bool isParent { get; set; } = true;

        public List<Ztree> children { get; set; } = new List<Ztree>();

    }



    public class TaskBuild : DSkin.Forms.MiniBlinkPage
    {

        [JSFunction]
        public string GetDataBaseAll()
        {
            var data = Curd.DataBase.Select
                .Select(a => new Ztree
                {
                    id = a.Id,
                    name = a.Name,
                    open = false
                })
                .ToList();
            return JsonConvert.SerializeObject(new { code = 0, data });
        }


        [JSFunction]
        public string GetDataBase(string id, string tableName, int level)
        {
            
            DataBaseConfig model = null;
            try
            {
                var data = new object();
                if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
                {
                    model = Curd.DataBase.Select.WhereDynamic(gid).ToOne();
                    using (IFreeSql fsql = new FreeSql.FreeSqlBuilder().UseConnectionString(model.DataType,
                       model.ConnectionStrings).Build())
                    {
                        if (level == 0)
                        {
                            var res = fsql.DbFirst.GetDatabases();
                            if (!string.IsNullOrEmpty(model.DataBaseName))
                            {
                                res = res.Where(a => a.ToUpper() == model.DataBaseName.ToUpper()).ToList();
                            }
                            data =
                                res.Select(a => new
                                {
                                    id = gid,
                                    name = a,
                                    children = fsql.DbFirst.GetTablesByDatabase(a).Select(b => new
                                    {
                                        id = gid,
                                        name = b.Name
                                    }).ToList()
                                }).ToList();
                        }
                        else
                        {
                            var res = fsql.DbFirst.GetTablesByDatabase(tableName);
                            data = res.Select(a => new { name = a.Name }).ToList();
                        }
                    }
                }
                return JsonConvert.SerializeObject(new { code = 0, data });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { code = 1, msg = e.Message + "<br/> 数据库连接串：" + model?.ConnectionStrings });
            }


        }
    }
}
