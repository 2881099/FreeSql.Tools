using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Validator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeSqlTools.Component
{
    public class UserControlBase : UserControl
    {



        public virtual void SaveDataConnection() { }
        public virtual void TestDataConnection() { }

        protected List<TextBoxX> textBoxs { get; private set; }
        protected List<TextBoxX> InitLostFocus(Control control, Highlighter highlighter)
        {
            textBoxs = control.Controls.Cast<Control>().Where(a => (a is TextBoxX textBox))
            .OrderBy(a => a.Name).Select(a => a as TextBoxX).ToList();
            textBoxs.ForEach(a =>
            {
                a.LostFocus += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(((TextBoxX)s).Text))
                    {
                        highlighter.SetHighlightColor(a, eHighlightColor.Green);
                    }
                };

            });
            return textBoxs;
        }

        protected bool Validator(Highlighter highlighter)
        {
            foreach (var m in textBoxs)
            {
                if (string.IsNullOrEmpty(m.Text) && m.Tag != null)
                {
                    highlighter.SetHighlightColor(m, eHighlightColor.Red);
                    ToastNotification.ToastBackColor = Color.Red;
                    ToastNotification.ToastForeColor= Color.White;
                    ToastNotification.Show(this, m.Tag.ToString() + " 不能为空", null, 3000, eToastGlowColor.Red, eToastPosition.TopCenter);
                    return false;
                }
            }
            return true;

        }

    }
}
