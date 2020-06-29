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
using FreeSql;
using DevComponents.DotNetBar.Controls;

namespace FreeSqlTools.Component
{
    public partial class UCConnection : UserControlBase
    {
        public UCConnection()
        {
            InitializeComponent();
        }


        public override void SaveDataConnection()
        {
            new DataBaseInfo
            {
                Id = Guid.NewGuid(),
                Name = textBoxX1.Text,
                IsString = true,
                ConnectionString = textBoxX2.Text,
                DataType = GetDataType(),
            }.Add();
        }

        DataType GetDataType()
        {
            DataType dataType = DataType.MySql;
            var checkbox = this.Controls.Cast<Control>().Where(a => (a is CheckBoxX checkBox) && checkBox.Checked).FirstOrDefault();
            switch (checkbox.Text)
            {
                case "MySql": dataType = DataType.MySql; break;
                case "SqlServer": dataType = DataType.SqlServer; break;
                case "PostgreSQL": dataType = DataType.PostgreSQL; break;
                case "Oracle": dataType = DataType.Oracle; break;
                case "Sqlite": dataType = DataType.Sqlite; break;
            }

            return dataType;
        }
        public override void TestDataConnection()
        {
            var connString = textBoxX2.Text;
            try
            {

                var dataType = GetDataType();
                using (var fsql = new FreeSql.FreeSqlBuilder()
                   .UseConnectionString(dataType,
                   connString).Build())
                    fsql.DbFirst.GetDatabases();
                MessageBoxEx.Show("数据库连接成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                TaskDialog.Show("错误提示", eTaskDialogIcon.BlueStop, "数据库连接失败", $" 原因：{e.Message}\n 连接串：{connString}\n", eTaskDialogButton.Ok);
            }
        }
    }
}
