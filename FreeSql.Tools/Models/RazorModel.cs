using FreeSql.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Models
{
    public class RazorModel
    {
        public IFreeSql fsql { get; set; }
        public List<DbColumnInfo> Columns { get; set; }
        public DbTableInfo table { get; set; }
        public Func<Models.TaskBuild, string> GetEntityName { get; set; }
        public string Namespace { get; set; }


        public string FullTableName => $"{table.Schema}.{table.Name}".TrimStart('.');



        public Func<DbColumnInfo, GetDbInfoModel> GetDbInfo => col => {
            var info = fsql.CodeFirst.GetDbInfo(col.CsType);
            if (info == null) return null;
            return new GetDbInfoModel
            {
                type = info.Value.type,
                dbtype = info.Value.dbtype,
                dbtypeFull = info.Value.dbtypeFull.Replace(" NOT NULL", ""),
                isnullable = info.Value.isnullable,
                defaultValue = info.Value.defaultValue
            };
        };
    }

    public class GetDbInfoModel
    {
        public int type { get; set; }
        public string dbtype { get; set; }
        public string dbtypeFull { get; set; }
        public bool? isnullable { get; set; }
        public object defaultValue { get; set; }
    }
}
