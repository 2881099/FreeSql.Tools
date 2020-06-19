using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FreeSqlTools.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeSqlTools
{
    public partial class FrmCreateDataConnection : DevComponents.DotNetBar.OfficeForm
    {
        public FrmCreateDataConnection()
        {
            this.EnableGlass = false;
            InitializeComponent();
        }
        UserControlBase userControl = null;
        Control[] controls;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (buttonX1.Text == "保存")
            {
                userControl?.SaveDataConnection();
                MessageBoxEx.Show("保存成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                groupPanel1.Text = "配置数据库连接";
                controls = groupPanel1.Controls.Cast<Control>().Where(a => (a is CheckBoxX checkBox)).ToArray();
                groupPanel1.Controls.Clear();
                var checkbox = controls.Where(a => (a is CheckBoxX checkBox) && checkBox.Checked).FirstOrDefault();
                if (checkbox == null) return;
                switch (checkbox.Text)
                {
                    case "MySql": userControl = new UcMysql(); break;
                    case "SqlServer": userControl = new UcSqlServer(); break;
                    case "PostgreSQL": userControl = new UcPostgreSQL(); break;
                    case "Oracle": userControl = new UcOracle(); break;
                    case "Sqlite": userControl = new UcSqlite(); break;
                    case "自定义": userControl = new UCConnection(); break;
                }
                userControl.BackColor = Color.Transparent;
                userControl.Dock = DockStyle.Fill;
                groupPanel1.Controls.Add(userControl);
                buttonX2.Visible = true;
                buttonX3.Visible = true;
                buttonX1.Text = "保存";
            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
     
            groupPanel1.Controls.Clear();
            groupPanel1.Controls.AddRange(controls);
            buttonX2.Visible = false;
            buttonX3.Visible = false;
            buttonX1.Text = "下一步";
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            userControl?.TestDataConnection();
        }
    }
}
