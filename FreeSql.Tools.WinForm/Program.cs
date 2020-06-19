using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            ApplicationEventHandlerClass AppEvents = new ApplicationEventHandlerClass();
            Application.ThreadException += new ThreadExceptionEventHandler(AppEvents.OnThreadException);
            Application.Run(new Form1());
        }


    }
    // 全局异常处理
    public class ApplicationEventHandlerClass
    {
        public void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            TaskDialog.Show("错误提示", eTaskDialogIcon.Stop, "全局异常", e.Exception.Message, eTaskDialogButton.Ok);
        }
    }
}
