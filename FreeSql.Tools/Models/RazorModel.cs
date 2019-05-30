using FreeSql.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Models
{
  public  class RazorModel
    {
        public IFreeSql fsql { get; set; }
        public List<DbTableInfo> Tables { get; set; }
        public DbTableInfo Table { get; set; }
        public string TableName { get; set; }
        public Func<string, string> GetEntityName { get; set; }
        public Func<string, string> GetPropertyName { get; set; }
        public RazorModel(IFreeSql freeSql)
        {
            this.fsql = freeSql;
            this.Tables = freeSql.DbFirst.GetTablesByDatabase();            

        }
    }
}
