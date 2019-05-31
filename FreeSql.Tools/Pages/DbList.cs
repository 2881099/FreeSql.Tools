using DSkin.DirectUI;
using DSkin.Forms;
using FreeSqlTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsValue = DSkin.DirectUI.MiniblinkPInvoke.JsValue;

namespace FreeSqlTools.Pages
{
    public class DbList : DSkin.Forms.MiniBlinkPage
    {
        MiniBlinkCollection<DataBaseConfig> data;
        public MiniBlinkCollection<DataBaseConfig> Data
        {
            get
            {
                if (data == null)
                {
                    data = new MiniBlinkCollection<DataBaseConfig>(this);
                }
                return data;
            }
        }

        public DbList()
        {
            var _list = Curd.DataBase.Select.ToList();
            Data.AddRange(_list);
        }



        [JSFunction]
        public async Task delDataBase(string id)
        {
            if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
            {
                Curd.TaskBuildInfo.Delete(a => a.DataBaseConfigId == gid);
                Curd.DataBase.Delete(gid);
                var _temp = Data.FirstOrDefault(a => a.Id == gid);
                Data.Remove(_temp);
                Data.SaveChanges();
                InvokeJS("departments.bootstrapTable('load', page.Data);$('[data-toggle=\"tooltip\"]').tooltip();");
                InvokeJS("Helper.ui.message.success('删除数据库信息成功');");
                await Task.FromResult(0);
            }
        }




        [JSFunction]
        public void UpdateRow(JsValue jsValue)
        {
            Data.Clear();

            var entity = new DataBaseConfig
            {
                DataBaseName = jsValue.Get("dataBaseName").ToString(),
                Name = jsValue.Get("name").ToString(),
                ServerIP = jsValue.Get("serverIP").ToString(),
                UserName = jsValue.Get("userName").ToString(),
                UserPass = jsValue.Get("userPass").ToString(),
                Port = jsValue.Get("port").ToInt(),
                DataType = (FreeSql.DataType)jsValue.Get("dataType").ToInt()
            };

            #region 配置数据库连接串
            switch (entity.DataType)
            {
                case FreeSql.DataType.MySql:
                    entity.ConnectionStrings = $"Data Source={entity.ServerIP};Port={entity.Port};User ID={entity.UserName};Password={entity.UserPass};Initial Catalog={(string.IsNullOrEmpty(entity.DataBaseName) ? "mysql" : entity.DataBaseName)};Charset=utf8;SslMode=none;Max pool size=5";
                    break;
                case FreeSql.DataType.SqlServer:
                    entity.ConnectionStrings = $"Data Source={entity.ServerIP},{entity.Port};Initial Catalog={entity.DataBaseName};User ID={entity.UserName};Password={entity.UserPass};Pooling=true;Max Pool Size=5";
                    break;
                case FreeSql.DataType.PostgreSQL:
                    entity.ConnectionStrings = $"Host={entity.ServerIP};Port={entity.Port};Username={entity.UserName};Password={entity.UserPass};Database={(string.IsNullOrEmpty(entity.DataBaseName) ? "postgres" : entity.DataBaseName)};Pooling=true;Maximum Pool Size=5";
                    break;
                case FreeSql.DataType.Oracle:
                    entity.ConnectionStrings = $"user id={entity.UserName};password={entity.UserPass};data source=//{entity.ServerIP}:{entity.Port}/{entity.DataBaseName};Pooling=true;Max Pool Size=5";
                    break;
            }
            #endregion

            var id = jsValue.Get("id").ToString();
            if (Guid.TryParse(id, out Guid vid) && vid != Guid.Empty)
            {
                entity.Id = vid;
            }

            Curd.DataBase.InsertOrUpdate(entity);
            var _list = Curd.DataBase.Select.ToList();
            Data.AddRange(_list);
            Data.SaveChanges();





            //InvokeJS("page.$confirm('添加完成, 是否继续?', '提示', {confirmButtonText: '确定',cancelButtonText: '取消',type: 'warning'})");
        }
    }
}
