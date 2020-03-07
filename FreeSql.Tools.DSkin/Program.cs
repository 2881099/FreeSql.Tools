using DSkin.DirectUI;
using FreeSqlTools.Pages;
using System;
using System.Windows.Forms;

namespace FreeSqlTools
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MiniblinkPInvoke.ResourceAssemblys.Add("FreeSqlTools", System.Reflection.Assembly.GetExecutingAssembly());
            MiniblinkPInvoke.PageNameSpace = "FreeSqlTools.Pages";
            Curd.InitFreeSql();
            Application.Run(new FrWebMain());
        }
    }
}
