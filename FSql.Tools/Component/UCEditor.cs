using DevComponents.DotNetBar;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.TextEditor.Document;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FreeSqlTools.Component
{
    public partial class UCEditor : UserControl
    {
        TextEditor editor = new TextEditor();
        string _path = string.Empty;
        public UCEditor(string path)
        {
            InitializeComponent();
            //textEditorControl1.Text = File.ReadAllText(path);
            //textEditorControl1.ShowEOLMarkers = false;
            //textEditorControl1.ShowHRuler = false;
            //textEditorControl1.ShowInvalidLines = false;
            //textEditorControl1.ShowMatchingBracket = true;
            //textEditorControl1.ShowSpaces = false;
            //textEditorControl1.ShowTabs = false;
            //textEditorControl1.ShowVRuler = false;
            //textEditorControl1.AllowCaretBeyondEOL = false;
            //textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            //textEditorControl1.Font = new Font("Consolas", 15);
            //textEditorControl1.Encoding = System.Text.Encoding.Default;
            var typeConverter = new HighlightingDefinitionTypeConverter();
            //展示行号
            editor.ShowLineNumbers = true;
            editor.Padding = new System.Windows.Thickness(20);
            //字体
            editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editor.FontSize = 22;
            //C#语法高亮          
            var csSyntaxHighlighter = (IHighlightingDefinition)typeConverter.ConvertFrom("C#");
            editor.SyntaxHighlighting = csSyntaxHighlighter;
            //将editor作为elemetnHost的组件
            elementHost1.Child = editor;
            editor.Load(path);
            _path = path;
        }

        private void command_save_Executed(object sender, EventArgs e)
        {
            File.WriteAllText(_path, editor.Text);
            ToastNotification.ToastBackColor = Color.Green;
            ToastNotification.ToastForeColor = Color.White;
            ToastNotification.ToastFont = new Font("微软雅黑", 22);
            ToastNotification.Show(this,"编辑成功", null, 3000, eToastGlowColor.Green, eToastPosition.TopCenter);
        }


    }
}
