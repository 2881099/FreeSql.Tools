using FreeSql;
using FreeSqlTools.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Pages
{
    public static class Curd
    {
        public static  IFreeSql fsql { get; private set; }   

        public static void InitFreeSql()
        {
            fsql = new FreeSql.FreeSqlBuilder()
                    .UseConnectionString(FreeSql.DataType.Sqlite,
                        @"Data Source=|DataDirectory|\freesql.db;Pooling=true;Max Pool Size=10")
                        .UseLazyLoading(true)
                         .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                      .UseMonitorCommand(
                         //监听SQL命令对象，在执行前
                      cmd => Trace.WriteLine(cmd.CommandText),
                     //监听SQL命令对象，在执行后
                     (cmd, traceLog) => Trace.WriteLine(traceLog))
                    .Build();

        }

        public static GuidRepository<Templates> Templates => fsql.GetGuidRepository<Templates>();
        public static GuidRepository<DataBaseConfig> DataBase  => fsql.GetGuidRepository<DataBaseConfig>();
        public static GuidRepository<Models.TaskBuild> TaskBuild => fsql.GetGuidRepository<Models.TaskBuild>();
        public static GuidRepository<Models.TaskBuildInfo> TaskBuildInfo => fsql.GetGuidRepository<Models.TaskBuildInfo>();

    }
}
