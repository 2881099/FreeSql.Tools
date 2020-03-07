using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FreeSqlTools
{
    public partial class FrmRazorTemplates : DevComponents.DotNetBar.OfficeForm
    {
        public string TemplatesPath { get; private set; }
        public string TemplatesName { get; private set; }
        public FrmRazorTemplates(bool fag)
        {
            this.EnableGlass = false;
            InitializeComponent();


            if (fag == false)
            {
                comboBoxEx1.Location = textBoxX1.Location;
                textBoxX1.Visible = false;
                buttonX1.Text = "选择模板";
                loadTemplates();
                buttonX1.Click += ButtonX1_Click;
            }
            else
            {
                comboBoxEx1.Visible = false;
                textBoxX1.Visible = true;
                buttonX1.Text = "创建模板";
                buttonX1.Click += buttonX1_Click;
            }
        }

        private void ButtonX1_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.Items.Count <= 0)
            {
                ToastNotification.ToastBackColor = Color.Red;
                ToastNotification.ToastForeColor = Color.White;
                ToastNotification.ToastFont = new Font("微软雅黑", 15);
                ToastNotification.Show(this, "没有可用模板，请新建。", null, 3000, eToastGlowColor.Red, eToastPosition.TopCenter);
                return;
            }
            var entity = lst.Where(a => a.Name == comboBoxEx1.Text).FirstOrDefault();
            if (entity != null)
            {
                TemplatesPath = entity.FullName;
                TemplatesName = entity.Name.Replace(".tpl", "").Trim();
                this.Close();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        List<FileInfo> lst = new List<FileInfo>();
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
                }
            }
            if (lst.Count >= 1)
            {
                comboBoxEx1.DataSource = lst.Select(a => a.Name).ToArray();
                comboBoxEx1.SelectedIndex = 0;
            }
        }


        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxX1.Text))
            {
                ToastNotification.ToastBackColor = Color.Red;
                ToastNotification.ToastForeColor = Color.White;
                ToastNotification.ToastFont = new Font("微软雅黑", 15);
                ToastNotification.Show(this, "模版名称不允许为空", null, 3000, eToastGlowColor.Red, eToastPosition.TopCenter);
                return;
            }
            string path = Path.Combine(Environment.CurrentDirectory, "Templates");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            TemplatesName = textBoxX1.Text;
            TemplatesPath = Path.Combine(path, $"{textBoxX1.Text}.tpl");
            if (File.Exists(TemplatesPath))
            {
                ToastNotification.ToastBackColor = Color.Red;
                ToastNotification.ToastForeColor = Color.White;
                ToastNotification.ToastFont = new Font("微软雅黑", 15);
                ToastNotification.Show(this, "模版名称己存在", null, 3000, eToastGlowColor.Red, eToastPosition.TopCenter);
                return;
            }
            using (var sr = File.Create(TemplatesPath))
            {
                sr.Close();
                sr.Dispose();
            }
            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
