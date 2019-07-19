using FreeSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.FreeSqlTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IFreeSql fsql;
        protected override void OnStartup(StartupEventArgs e)
        {

            var path = Path.Combine(Environment.CurrentDirectory, "freesql.db");
            fsql =  new FreeSqlBuilder()
                    .UseConnectionString(DataType.Sqlite,$"Data Source={path};Pooling=true;Max Pool Size=10")
                    .UseLazyLoading(true)
                    .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                    .UseMonitorCommand(
                      //监听SQL命令对象，在执行前
                      cmd => Trace.WriteLine(cmd.CommandText),
                     //监听SQL命令对象，在执行后
                     (cmd, traceLog) => Trace.WriteLine(traceLog))
                   .Build();
            base.OnStartup(e);
        }
    }
}
