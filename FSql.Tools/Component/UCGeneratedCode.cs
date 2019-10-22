using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using FreeSql.DatabaseModel;
using FreeSqlTools.Common;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.TextEditor.Document;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FreeSqlTools.Component
{
    public partial class UCGeneratedCode : UserControl
    {

        Node _node;
        private readonly TextEditor editorTemplates = new TextEditor();
        private readonly TextEditor editorCode = new TextEditor();
        public UCGeneratedCode(Node node)
        {
            InitializeComponent();                   
            InitTemplates();
            _node = node;
            var typeConverter = new HighlightingDefinitionTypeConverter();
            //展示行号
            editorTemplates.ShowLineNumbers = true;
            editorCode.ShowLineNumbers = true;
            //字体
            editorTemplates.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editorTemplates.FontSize = 22;
            editorCode.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editorCode.FontSize = 22;
            //C#语法高亮          
            var csSyntaxHighlighter = (IHighlightingDefinition)typeConverter.ConvertFrom("C#");
            editorTemplates.SyntaxHighlighting = csSyntaxHighlighter;
            editorCode.SyntaxHighlighting = csSyntaxHighlighter;
            //将editor作为elemetnHost的组件
            elementHost1.Child = editorTemplates;          
            elementHost2.Child = editorCode;
            InitTableInfo();
        }

        List<FileInfo> lst = new List<FileInfo>();
        void InitTemplates()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates");
            string[] dir = Directory.GetDirectories(path);
            DirectoryInfo fdir = new DirectoryInfo(path);
            FileInfo[] file = fdir.GetFiles("*.tpl");
            if (file.Length != 0 || dir.Length != 0)
            {
                foreach (FileInfo f in file)
                {
                    lst.Add(f);
                }
            }
            if (lst.Count >= 1)
            {
                comboBoxEx1.DataSource = lst.Select(a => a.Name).ToArray();
                comboBoxEx1.SelectedIndex = 0;
                editorTemplates.Load(lst.FirstOrDefault().FullName);
            }
        }
        List<DbTableInfo> dbTableInfos = new List<DbTableInfo>();
        DbTableInfo dbTableInfo = null;
        void InitTableInfo()
        {
            dbTableInfos = _node.Parent.Nodes.Cast<Node>()
                .Select(a => a.Tag as DbTableInfo).ToList();
            //G.GetTablesByDatabase(_node.Parent.DataKey, _node.Parent.Text);
            dbTableInfo = dbTableInfos.FirstOrDefault(a => a.Name == _node.Text);
            dataGridViewX1.DataSource = dbTableInfo?.Columns;
        }

        private void comboBoxEx1_DropDownClosed(object sender, EventArgs e)
        {
            var fileInfo = lst.Where(a => a.Name == comboBoxEx1.Text).FirstOrDefault();
            if (fileInfo != null)
            {
                editorTemplates.Load(fileInfo.FullName);
            }
        }


        private async void superTabControl1_SelectedTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxX1.Text))
            {
                e.Cancel = true;
                MessageBoxEx.Show("命名空间不能为空");
                return;
            }

            if (e.NewValue.Text == "生成代码")
            {
                var codeGenerate = new CodeGenerate();
                var taskBuild = new TaskBuild()
                {
                    Fsql = G.GetFreeSql(_node.DataKey),
                    DbName = _node.Parent.Text,
                    NamespaceName = textBoxX1.Text,
                    RemoveStr = textBoxX2.Text,
                    OptionsEntity01 = checkBoxX1.Checked,
                    OptionsEntity02 = checkBoxX2.Checked,
                    OptionsEntity03 = checkBoxX3.Checked,
                    OptionsEntity04 = checkBoxX4.Checked
                };

                editorCode.Text = await codeGenerate.Setup(taskBuild, editorTemplates.Text, dbTableInfos, dbTableInfo);
            }
        }
    }
}
