using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FreeSqlTools.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeSqlTools
{
    public partial class Form1 : RibbonForm
    {
        static FrmLoading frmLoading;
        public Form1()
        {
            this.EnableGlass = false;
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            var process = Process.GetCurrentProcess();
            process.Kill();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() => LoadDataTreeList());
            // DesktopAlert.Show("发现新版本", "\uf005", eSymbolSet.Awesome, Color.Empty, eDesktopAlertColor.DarkRed, eAlertPosition.BottomRight, 5, 0, (x) => { });
            this.WindowState = FormWindowState.Maximized;
        }

        private void advTree1_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (e.Node.Level == 0 || e.Node.Level > 2) return;
            LoadDataInfo(e.Node);
        }
        private void Command_createDataConnection_Executed(object sender, EventArgs e)
        {
            var frm = new FrmCreateDataConnection();
            var frmResult = frm.ShowDialog();
            if (frmResult == DialogResult.OK)
                LoadDataTreeList();
        }
        private void buttonItem16_Click(object sender, EventArgs e)
        {
            Task.Run(() => LoadDataTreeList());
        }

        void LoadDataTreeList()
        {
            DataBaseInfo baseInfo = new DataBaseInfo();
            advTree1.Nodes[0].Nodes.Clear();
            List<Node> nodes = new List<Node>();
            foreach (var m in baseInfo.GetDataBaseInfos())
            {
                //var connectionString = m.IsString ? m.ConnectionString
                //    : G.GetConnectionString(m.DataType, m.UserId, m.Pwd, m.Host, m.DbName,
                //       m.Port, m.ValidatorType);
                var node = new Node($"{m.Name}({m.DataType.ToString()})")
                {
                    Image = Properties.Resources.monitor,
                    Name = m.Id.ToString(),
                    Tag = m,
                    //TagString = connectionString,
                    DataKey = $"{m.DataType.ToString()}_{m.Id.ToString("N")}"
                };
                node.ContextMenu = buttonItem21;
                G.AddFreeSql(node.DataKey,m);
                nodes.Add(node);
            }
            advTree1.Nodes[0].Nodes.AddRange(nodes.ToArray());
        }

        private void buttonItem17_Click(object sender, EventArgs e)
        {
            EditTemplates(true);
        }

        private void superTabControl1_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (e.Tab.Text == "首页")
            {
                e.Cancel = true;
                ToastNotification.ToastBackColor = Color.Red;
                ToastNotification.ToastForeColor = Color.White;
                ToastNotification.ToastFont = new Font("微软雅黑", 15);
                ToastNotification.Show(superTabControl1, "默认页不允许关闭", null, 3000, eToastGlowColor.Red, eToastPosition.TopCenter);
            }
            if (pairs.ContainsKey(e.Tab.Text)) pairs.Remove(e.Tab.Text);
            if (pairs.Count == 0) buttonItem19.Enabled = false;

        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {
            EditTemplates(false);
        }

        Dictionary<string, UCEditor> pairs
            = new Dictionary<string, UCEditor>();
        void EditTemplates(bool fag)
        {

            string path = string.Empty;
            var form = new FrmRazorTemplates(fag);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (pairs.ContainsKey(form.TemplatesName))
                {
                    var item = superTabControl1.Tabs.Cast<SuperTabItem>()
                        .Where(a => a.Text == form.TemplatesName).FirstOrDefault();
                    superTabControl1.SelectedTab = item;
                    return;
                }
                var superItem = superTabControl1.CreateTab(form.TemplatesName);
                var ucEditor = new UCEditor(form.TemplatesPath);
                pairs.Add(form.TemplatesName, ucEditor);
                superTabControl1.SelectedTab = superItem;
                ucEditor.Dock = DockStyle.Fill;
                superTabControl1.SelectedPanel.Controls.Add(ucEditor);
            }
        }

        async void LoadDataInfo(Node node)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(a =>
            {
                frmLoading = new FrmLoading();
                frmLoading.ShowDialog();
            }));
            var res = await Task.Run(() =>
             {

                 if (node.Level == 1)
                 {
                     if (node.Nodes.Count >= 1) return 0;
                     node.Nodes.Clear();
                     var list = G.GetDatabases(node.DataKey, node.TagString);
                     var nodes = list.Select(a => new Node(a)
                     {
                         Image = Properties.Resources._base,
                         DataKey = node.DataKey,
                         ContextMenu = buttonItem22
                     }).ToArray();
                     node.Nodes.AddRange(nodes);
                 }
                 else if (node.Level == 2)
                 {
                     node.Nodes.Clear();
                     Task.Delay(1000);
                     var list = G.GetTablesByDatabase(node.DataKey, node.Text);
                     var nodes = list.Select(a => new Node(a.Name)
                     {
                         Image = Properties.Resources.application,
                        // CheckBoxVisible = true,
                        // CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.CheckBox,
                        // CheckState = CheckState.Unchecked,
                         Tag = a,
                         DataKey = node.DataKey,
                         ContextMenu = buttonItem23
                     }).ToArray();
                     node.Nodes.AddRange(nodes);
                 }
                 return 0;
             });
            node.Expanded = true;
            this.Invoke((Action)delegate () { Thread.CurrentThread.Join(500); frmLoading.Close(); });
        }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem26_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            node.Nodes.Clear();
            LoadDataInfo(node);
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem33_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            node.Nodes.Clear();
            LoadDataInfo(node);
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem27_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            node.Nodes.Clear();

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem25_Click(object sender, EventArgs e)
        {
            var frm = new FrmCreateDataConnection();
            var frmResult = frm.ShowDialog();
            if (frmResult == DialogResult.OK)
                LoadDataTreeList();
        }

        /// <summary>
        /// 刷新库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem31_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            node.Nodes.Clear();
            LoadDataInfo(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem29_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            if (node != null && node.Level == 1)
            {
                if (MessageBoxEx.Show("是否删除当前选中服务器", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    node.Nodes.Clear();
                    DataBaseInfo baseInfo = new DataBaseInfo();
                    baseInfo.Delete(Guid.Parse(node.Name));
                    advTree1.Nodes[0].Nodes.Remove(node);
                }
            }
            else
            {
                MessageBoxEx.Show("删除节点无效", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 批量生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void command_batch_Executed(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            var frm = new FrmBatch(node);
            frm.ShowDialog();
        }

        private void buttonItem34_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/2881099/FreeSql/issues/new");
        }
        private void buttonItem35_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/2881099/FreeSql");
        }
        private void buttonItem36_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/2881099/FreeSql/wiki");
        }


        private void superTabControl1_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            buttonItem19.Enabled = false;
            var superTab = superTabControl1.SelectedTab;
            if (pairs.TryGetValue(superTab.Text, out UCEditor uCEditor))
            {
                buttonItem19.Command = uCEditor.command_save;
                buttonItem19.Enabled = true;
            }
        }
        private void buttonItem32_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            var superItem = superTabControl1.CreateTab($"({node.Parent.Text })-{node.Text}");
            var ucEditor = new UCGeneratedCode(node);
            superTabControl1.SelectedTab = superItem;
            ucEditor.Dock = DockStyle.Fill;
            superTabControl1.SelectedPanel.Controls.Add(ucEditor);
        }

        private void buttonItem28_Click(object sender, EventArgs e)
        {
            var node = advTree1.SelectedNode;
            var superItem = superTabControl1.CreateTab($"({node.Parent.Text })-{node.Text} 查询");
            var ucEditor = new UCDataGrid(node);
            superTabControl1.SelectedTab = superItem;
            ucEditor.Dock = DockStyle.Fill;
            superTabControl1.SelectedPanel.Controls.Add(ucEditor);
        }
    }
}
