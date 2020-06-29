using DevComponents.DotNetBar;
using System;
using System.Windows.Forms;

namespace FreeSqlTools.Component
{
    public partial class UcSqlServer : UserControlBase
    {
        public UcSqlServer()
        {
            InitializeComponent();
            InitLostFocus(this, this.highlighter1);
            comboBoxEx1.SelectedIndex = 0;
        }
        public override void SaveDataConnection()
        {
            if (!Validator(this.highlighter1)) return;
            new DataBaseInfo
            {
                Id = Guid.NewGuid(),
                Host = textBoxX2.Text,
                Name = textBoxX1.Text,
                ValidatorType= validator,
                DbName = textBoxX6.Text,
                Port = textBoxX3.Text,
                DataType = FreeSql.DataType.SqlServer,
                UserId = textBoxX4.Text,
                Pwd = textBoxX5.Text
            }.Add();
        }

        public override void TestDataConnection()
        {
            if (!Validator(this.highlighter1)) return;
            var connString = G.GetConnectionString(FreeSql.DataType.SqlServer, textBoxX4.Text,
            textBoxX5.Text, textBoxX2.Text, textBoxX6.Text, textBoxX3.Text, validator);
            try
            {
                using (var fsql = new FreeSql.FreeSqlBuilder()
                   .UseConnectionString(FreeSql.DataType.SqlServer,
                   connString).Build())
                    fsql.DbFirst.GetDatabases();
                MessageBoxEx.Show("数据库连接成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                TaskDialog.Show("错误提示", eTaskDialogIcon.BlueStop, "数据库连接失败", $" 原因：{e.Message}\n 连接串：{connString}\n", eTaskDialogButton.Ok);
            }

        }

        int validator = 0;
        private void comboBoxEx1_SelectedValueChanged(object sender, EventArgs e)
        {
            validator = comboBoxEx1.SelectedIndex;
            if (validator == 1)
            {
                labelX4.Visible = false;
                labelX5.Visible = false;
                textBoxX5.Visible = false;
                textBoxX4.Visible = false;
                textBoxX4.Tag = null;
                textBoxX5.Tag = null;
            }
            else
            {
                labelX4.Visible = true;
                labelX5.Visible = true;
                textBoxX5.Visible = true;
                textBoxX4.Visible = true;
                textBoxX4.Tag = "用户名";
                textBoxX5.Tag = "密码";
            }
        }
    }
}
