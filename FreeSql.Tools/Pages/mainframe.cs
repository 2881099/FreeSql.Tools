using DSkin.DirectUI;
using DSkin.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Pages
{
    public class data
    {
        public string date { get; set; }
        public string name { get; set; }
        public string address { get; set; }
    }
    public class mainframe : DSkin.Forms.MiniBlinkPage
    {

        MiniBlinkCollection<data> data;
        public MiniBlinkCollection<data> Data
        {
            get
            {
                if (data == null)
                {
                    data = new MiniBlinkCollection<data>(this);
                }
                return data;
            }
        }
        [JSFunction]
        public string Test()
        {
            return "123";
        }
        protected internal mainframe()
        {
            Data.Add(new data { date = "sdaf", name = "dgsda", address = "hfdgdsfa" });
            Data.Add(new data { date = "sdaf测试", name = "dgsda", address = "hfdgdsfa" });
        }

        /// <summary>
        /// 组件的事件绑定，参数数量要一样，复杂类型用JSValue代替
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [JSFunction]
        public void handleSelect(string a, JsValue b, JsValue c)
        {

        }
        [JSFunction]
        public void openBrowser(string url)
        {
            Process.Start(url);
        }
    }
}
