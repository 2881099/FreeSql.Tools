using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace FreeSqlTools.Component
{
    public partial class UcSqlite : UserControlBase
    {
        public UcSqlite()
        {
            InitializeComponent();
            InitLostFocus(this, this.highlighter1);
        }

        public override void SaveDataConnection()
        {
            if (!Validator(this.highlighter1)) return;
            new DataBaseInfo
            {
                Id = Guid.NewGuid(),
                Host = textBoxX2.Text,
                Name = textBoxX1.Text,
                DataType = FreeSql.DataType.Sqlite,
                UserId = textBoxX4.Text,
                Pwd = textBoxX5.Text
            }.Add();
        }

        public override void TestDataConnection()
        {
            if (!Validator(this.highlighter1)) return;
            var connString = G.GetConnectionString(FreeSql.DataType.Sqlite, textBoxX4.Text,
         textBoxX5.Text, textBoxX2.Text, string.Empty, string.Empty);
            try
            {
                using (var fsql = new FreeSql.FreeSqlBuilder()
                   .UseConnectionString(FreeSql.DataType.Sqlite,
                   connString).Build())
                    fsql.DbFirst.GetDatabases();
                MessageBoxEx.Show("数据库连接成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                TaskDialog.Show("错误提示", eTaskDialogIcon.Stop, "数据库连接失败", $" 原因：{e.Message}\n 连接串：{connString}\n", eTaskDialogButton.Ok);
            }
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxX2.Text = openFileDialog1.FileName;
            }
        }
    }
}
