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
        MiniBlinkCollection<DbConfig> data;
        public MiniBlinkCollection<DbConfig> Data
        {
            get
            {
                if (data == null)
                {
                    data = new MiniBlinkCollection<DbConfig>(this);
                }
                return data;
            }
        }

        public DbList()
        {
            var _list = Curd.SetFsql.Select<DbConfig>().ToList();
            Data.AddRange(_list);
        }







        [JSFunction]
        public void UpdateRow(JsValue jsValue)
        {
            Data.Clear();

            var entity = new DbConfig
            {
                DataBaseName = jsValue.Get("dataBaseName").ToString(),
                Name = jsValue.Get("name").ToString(),
                ServerIP = jsValue.Get("serverIP").ToString(),
                UserName = jsValue.Get("userName").ToString(),
                UserPass = jsValue.Get("userPass").ToString(),
                Port = jsValue.Get("prot").ToInt(),
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
                    entity.ConnectionStrings = $"Host={entity.ServerIP};Port={entity.Port};Username={entity.UserName};Password={entity.UserPass};Database={entity.DataBaseName};Pooling=true;Maximum Pool Size=5";
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
                Curd.SetFsql.Update<DbConfig>(id).SetSource(entity).ExecuteUpdated();
            }
            else
            {
                Curd.SetFsql.Insert(entity).ExecuteInserted();
            }
            var _list = Curd.SetFsql.Select<DbConfig>().ToList();
            Data.AddRange(_list);
            Data.SaveChanges();





            //InvokeJS("page.$confirm('添加完成, 是否继续?', '提示', {confirmButtonText: '确定',cancelButtonText: '取消',type: 'warning'})");
        }
    }
}
