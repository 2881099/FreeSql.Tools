using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using FreeSql.DatabaseModel;
using FreeSqlTools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeSqlTools
{
    public partial class FrmBatch : DevComponents.DotNetBar.Office2007Form
    {
        Node _node;
        FrmLoading frmLoading = null;
        public FrmBatch(Node node)
        {
            this.EnableGlass = false;
            InitializeComponent();
            _node = node;                  
            Load += FrmBatch_Load;   
        }
        private void FrmBatch_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(x => {
                frmLoading = new FrmLoading();
                frmLoading.ShowDialog();
            });
            labelX3.Text = _node.Parent.Text;
            labelX4.Text = _node.Text;
            LoadTableList();
            loadTemplates();
            Properties.Settings.Default.Reload();
            this.Invoke((Action)delegate { frmLoading.Close(); });
        }

        List<FileInfo> lst = new List<FileInfo>();
        List<DbTableInfo> dbTableInfos = new List<DbTableInfo>();
        void loadTemplates()
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
                    listBoxAdv3.Items.Add(f.Name);
                }
            }
        }
        void LoadTableList()
        {
            dbTableInfos = G.GetTablesByDatabase(_node.Parent.DataKey, _node.Text);
            listBoxAdv1.DataSource = dbTableInfos.Select(a => a.Name).ToArray();
        }

        private void command_all_Executed(object sender, EventArgs e)
        {
            listBoxAdv2.Items.Clear();
            foreach (var m in (string[])listBoxAdv1.DataSource)
                listBoxAdv2.Items.Add(m);
        }

        private void command_unall_Executed(object sender, EventArgs e)
        {
            listBoxAdv2.Items.Clear();
        }

        private void command_select_Executed(object sender, EventArgs e)
        {
            var item = listBoxAdv1.SelectedItem;
            if (item != null)
            {
                if (!listBoxAdv2.Items.Cast<string>().Any(a => a == item.ToString()))
                {
                    listBoxAdv2.Items.Add(item);
                }
            }
        }

        private void command_unselect_Executed(object sender, EventArgs e)
        {
            var item = listBoxAdv2.SelectedItem;
            if (item != null)
            {
                listBoxAdv2.Items.Remove(item);
            }
        }

        private async void command_export_Executed(object sender, EventArgs e)
        {

            Properties.Settings.Default.Save();
            if (listBoxAdv2.Items.Count == 0)
            {
                MessageBoxEx.Show("请选择表");
                return;
            }
            if (string.IsNullOrEmpty(textBoxX1.Text))
            {
                MessageBoxEx.Show("命名空间不能为空");
                return;
            }
            if (string.IsNullOrEmpty(textBoxX4.Text))
            {
                MessageBoxEx.Show("请选择导出路径");
                return;
            }
            if (listBoxAdv3.CheckedItems.Count == 0)
            {
                MessageBoxEx.Show("请选择生成模板");
                return;
            }
            var templates = listBoxAdv3.CheckedItems.Cast<ListBoxItem>().Select(a => a.Text).ToArray();     
            var taskBuild = new TaskBuild()
            {
                Fsql = G.GetFreeSql(_node.DataKey),
                DbName = _node.Text,
                FileName = textBoxX3.Text,
                GeneratePath = textBoxX4.Text,
                NamespaceName = textBoxX1.Text,
                RemoveStr = textBoxX2.Text,
                OptionsEntity01 = checkBoxX1.Checked,
                OptionsEntity02 = checkBoxX2.Checked,
                OptionsEntity03 = checkBoxX3.Checked,
                OptionsEntity04 = checkBoxX4.Checked,
                Templates = templates
            };
            var tables = listBoxAdv2.Items.Cast<string>().ToArray();
            var tableInfos = dbTableInfos.Where(a => tables.Contains(a.Name)).ToList();
            FrmLoading frmLoading=null;
            ThreadPool.QueueUserWorkItem(new WaitCallback(a =>
            {
                this.Invoke((Action)delegate ()
                {
                    frmLoading = new FrmLoading("正在生成中，请稍后.....");
                    frmLoading.ShowDialog();
                });
            }));
            await new CodeGenerate().Setup(taskBuild, tableInfos);
            this.Invoke((Action)delegate () { frmLoading?.Close(); });

        }
        private void command_openFileDialog_Executed(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxX4.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
