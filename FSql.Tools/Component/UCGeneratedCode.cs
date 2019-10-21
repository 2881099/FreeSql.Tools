using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using FreeSql.DatabaseModel;
using FreeSqlTools.Common;
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
        public UCGeneratedCode(Node node)
        {
            InitializeComponent();

            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            textEditorControl1.Font = new Font("Consolas", 15);
            textEditorControl1.Encoding = System.Text.Encoding.Default;

            textEditorControl2.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            textEditorControl2.Font = new Font("Consolas", 15);
            textEditorControl2.Encoding = System.Text.Encoding.Default;
            InitTemplates();
            _node = node;
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
                textEditorControl1.Text = File.ReadAllText(lst.FirstOrDefault().FullName);
            }
        }
        List<DbTableInfo> dbTableInfos = new List<DbTableInfo>();
        DbTableInfo dbTableInfo = null;
        void InitTableInfo()
        {
            dbTableInfos = G.GetTablesByDatabase(_node.Parent.DataKey, _node.Parent.Text);
            dbTableInfo = dbTableInfos.FirstOrDefault(a => a.Name == _node.Text);
            dataGridViewX1.DataSource = dbTableInfo?.Columns;
        }

        private void comboBoxEx1_DropDownClosed(object sender, EventArgs e)
        {
            var fileInfo = lst.Where(a => a.Name == comboBoxEx1.Text).FirstOrDefault();
            if (fileInfo != null)
            {
                textEditorControl1.Text = File.ReadAllText(fileInfo.FullName);
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

                textEditorControl2.Text = await codeGenerate.Setup(taskBuild, textEditorControl1.Text, dbTableInfos, dbTableInfo);
            }
        }
    }
}
