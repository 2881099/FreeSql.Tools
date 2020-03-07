using DSkin.DirectUI;
using DSkin.Forms;
using FreeSqlTools.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreeSqlTools.Pages
{
    public class DbList : DSkin.Forms.MiniBlinkPage
    {
        private MiniBlinkCollection<DataBaseConfig> data;
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
        public async Task TestDbconnection(string id)
        {
            await Task.Run(() =>
            {

                if (Guid.TryParse(id, out Guid gid) && gid != Guid.Empty)
                {
                    var entity = Curd.DataBase.Select.WhereDynamic(gid).ToOne();
                    try
                    {
                        using (var fsql = new FreeSql.FreeSqlBuilder()
                        .UseConnectionString(entity.DataType, entity.ConnectionStrings).Build())
                        {
                            Invoke(() =>
                            {
                                InvokeJS("Helper.ui.removeDialog();");
                                InvokeJS($"Helper.ui.dialogSuccess('提示','数据库连接成功');");
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Invoke(() =>
                        {
                            InvokeJS("Helper.ui.removeDialog();");
                            InvokeJS($"Helper.ui.dialogError('连接失败','{e.Message}');");
                        });
                    }
                }
            });
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
        public void UpdateRow(string jsonStr)
        {
            Data.Clear();

            var entity = JsonConvert.DeserializeObject<DataBaseConfig>(jsonStr);


            if (string.IsNullOrEmpty(entity.ConnectionStrings))
            {
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
            }
            else if (string.IsNullOrEmpty(entity.DataBaseName))
            {
                switch(entity.DataType)
                {
                    case FreeSql.DataType.Oracle:
                        entity.DataBaseName = Regex.Match(entity.ConnectionStrings, @"user id=([^;]+)", RegexOptions.IgnoreCase).Groups[1].Value;
                        break;
                    case FreeSql.DataType.MySql:
                        using (var conn = new MySql.Data.MySqlClient.MySqlConnection(entity.ConnectionStrings))
                        {
                            conn.Open();
                            entity.DataBaseName = conn.Database;
                            conn.Close();
                        }
                        break;
                    case FreeSql.DataType.SqlServer:
                        using (var conn = new System.Data.SqlClient.SqlConnection(entity.ConnectionStrings))
                        {
                            conn.Open();
                            entity.DataBaseName = conn.Database;
                            conn.Close();
                        }
                        break;
                    case FreeSql.DataType.PostgreSQL:
                        using (var conn = new Npgsql.NpgsqlConnection(entity.ConnectionStrings))
                        {
                            conn.Open();
                            entity.DataBaseName = conn.Database;
                            conn.Close();
                        }
                        break;
                }
            }

            Curd.DataBase.InsertOrUpdate(entity);
            var _list = Curd.DataBase.Select.ToList();
            Data.AddRange(_list);
            Data.SaveChanges();





            //InvokeJS("page.$confirm('添加完成, 是否继续?', '提示', {confirmButtonText: '确定',cancelButtonText: '取消',type: 'warning'})");
        }
    }
}
