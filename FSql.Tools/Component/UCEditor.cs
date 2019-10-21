using DevComponents.DotNetBar;
using ICSharpCode.TextEditor.Document;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FreeSqlTools.Component
{
    public partial class UCEditor : UserControl
    {
        string _path = string.Empty;
        public UCEditor(string path)
        {
            InitializeComponent();
            textEditorControl1.Text = File.ReadAllText(path);
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            textEditorControl1.Font = new Font("Consolas", 15);
            textEditorControl1.Encoding = System.Text.Encoding.Default;
            _path = path;
        }

        private void command_save_Executed(object sender, EventArgs e)
        {
            File.WriteAllText(_path, textEditorControl1.Text);
            ToastNotification.ToastBackColor = Color.Green;
            ToastNotification.ToastForeColor = Color.White;
            ToastNotification.ToastFont = new Font("微软雅黑", 22);
            ToastNotification.Show(this,"编辑成功", null, 3000, eToastGlowColor.Green, eToastPosition.TopCenter);
        }


    }
}
