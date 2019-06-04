using DSkin.DirectUI;
using DSkin.Forms;
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
        MiniBlinkCollection<Models.TaskBuild> data;

        public TaskBuild()
        {
            var list = Curd.TaskBuild.Select.ToList();
            Data.AddRange(list);
        }

        public MiniBlinkCollection<Models.TaskBuild> Data
        {
            get
            {
                if (data == null)
                {
                    data = new MiniBlinkCollection<Models.TaskBuild>(this);
                }
                return data;
            }
        }

        static Dictionary<string, bool> stateKeyValues = new Dictionary<string, bool>();
        [JSFunction]
        public async Task CodeGenerate(string id)
        {
            if (stateKeyValues.ContainsKey(id))
            {
                InvokeJS("Helper.ui.message.error('当前任务未结束,请稍后再试.');");
            }
            else
            {
                if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
                {
                    stateKeyValues.Add(id, true);
                    var model = Curd.TaskBuild.Select.WhereDynamic(gid)
                       .LeftJoin(a => a.Templates.Id == a.TemplatesId)
                       .IncludeMany(a => a.TaskBuildInfos, b => b.Include(p => p.DataBaseConfig))
                       .ToOne();
                    var res = await new CodeGenerate().Setup(model);
                    InvokeJS($"Helper.ui.message.info('[{model.TaskName}]{res}');");
                    stateKeyValues.Remove(id);
                }
                else
                {
                    InvokeJS("Helper.ui.alert.error('生成失败','参数不是有效的.');");
                }

            }
        }


        [JSFunction]
        public async Task CodeGenerates(string jsonStr)
        {

            var ids =  Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(jsonStr);       
            foreach(var id in ids)
            {              
                if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
                {                 
                    var model = Curd.TaskBuild.Select.WhereDynamic(gid)
                       .LeftJoin(a => a.Templates.Id == a.TemplatesId)
                       .IncludeMany(a => a.TaskBuildInfos, b => b.Include(p => p.DataBaseConfig))
                       .ToOne();
                    InvokeJS($"$('.helper-loading-text').text('正在生成[{model.TaskName}]请稍后....')");
                    await Task.Delay(500);
                    var res = await new CodeGenerate().Setup(model);                                       
                    InvokeJS($"$('.helper-loading-text').text('[{model.TaskName}]{res}')");
                    if(res.Contains("发生异常")) await Task.Delay(3000);

                }
                else
                {
                    InvokeJS("Helper.ui.alert.error('生成失败','参数不是有效的.');");
                }
            }
            await Task.FromResult(0);
            InvokeJS("Helper.ui.removeDialog();");
           
            
        }

        [JSFunction]
        public string GetDataBaseAll()
        {
            var data = Curd.DataBase.Select
                .ToList(a => new Ztree
                {
                    id = a.Id,
                    name = a.Name,
                    open = false
                });
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
                            data = res.Select(a => new
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

        [JSFunction]
        public string GetTemplate()
        {
            var data = Curd.Templates.Select
               .ToList(a => new { a.Id, a.Title });

            return JsonConvert.SerializeObject(new { code = 0, data });
        }




        [JSFunction]
        public async Task delTaslBuild(string id)
        {
            if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
            {
                Curd.TaskBuildInfo.Delete(a => a.TaskBuildId == gid);
                Curd.TaskBuild.Delete(gid);
                var _temp = Data.FirstOrDefault(a => a.Id == gid);
                Data.Remove(_temp);
                Data.SaveChanges();
                InvokeJS("tableTaskBuild.bootstrapTable('load', page.Data);$('[data-toggle=\"tooltip\"]').tooltip();");
                InvokeJS("Helper.ui.message.success('删除任务成功');");
                await Task.FromResult(0);
            }
        }

        [JSFunction]
        public string saveTaskBuild(string res)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<Models.TaskBuild>(res);
                Curd.fsql.SetDbContextOptions(o => o.EnableAddOrUpdateNavigateList = false);
                var entity = Curd.TaskBuild.Insert(model);
                model.TaskBuildInfos.ToList().ForEach(a => a.TaskBuildId = entity.Id);
                Curd.TaskBuildInfo.Insert(model.TaskBuildInfos);
                var list = Curd.TaskBuild.Select.ToList();
                Data.Clear();
                Data.AddRange(list);
                Data.SaveChanges();
                return JsonConvert.SerializeObject(new { code = 0, msg = "构建任务成功" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 打开对话框
        /// </summary>
        /// <returns></returns>
        [JSFunction]
        public string folderBrowserDialog()
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            return string.Empty;
        }
    }
}
