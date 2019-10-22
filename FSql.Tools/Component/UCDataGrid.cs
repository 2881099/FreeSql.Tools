using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.AdvTree;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit;
using System.Diagnostics;

namespace FreeSqlTools.Component
{
    public partial class UCDataGrid : UserControl
    {
        private readonly Node _node;
        private readonly Lazy<IFreeSql> fsql;
        private DataBaseInfo dataBaseInfo;
        TextEditor editor = new TextEditor();
        public UCDataGrid(Node node)
        {
            InitializeComponent();
            var typeConverter = new HighlightingDefinitionTypeConverter();
            //展示行号
            editor.ShowLineNumbers = true;
            //editor.Padding = new System.Windows.Thickness(20);
            //字体
            editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editor.FontSize = 22;
            //C#语法高亮          
            var csSyntaxHighlighter = (IHighlightingDefinition)typeConverter.ConvertFrom("TSQL");
            editor.SyntaxHighlighting = csSyntaxHighlighter;
            //将editor作为elemetnHost的组件
            elementHost1.Child = editor;
            _node = node;
            dataBaseInfo = (DataBaseInfo)(node.Level == 3 ? node.Parent.Parent.Tag :
                 node.Parent.Tag);
            if (dataBaseInfo.DataType == FreeSql.DataType.SqlServer)
            {
                dataBaseInfo.DbName = node.Parent.Text;
            }

            fsql = G.GetNewFreeSql(dataBaseInfo);
            Load += UCDataGrid_Load;
        }

        private void UCDataGrid_Load(object sender, EventArgs e)
        {
            string sqlString = "";
            switch (dataBaseInfo.DataType)
            {
                case FreeSql.DataType.SqlServer:
                    sqlString = $"SELECT top 100 * FROM {_node.Text}"; break;
                case FreeSql.DataType.Oracle:
                    sqlString = $"SELECT * FROM {_node.Text} WHERE ROWNUM <=100"; break;
                case FreeSql.DataType.MySql:
                case FreeSql.DataType.PostgreSQL:
                    sqlString = $"SELECT * FROM {_node.Text} LIMIT 100"; break;
            }
            editor.Text = sqlString;
            BindDataGridView(sqlString);
        }


        void BindDataGridView(string sqlString)
        {
            this.BeginInvoke(QueryBindDataGridView, sqlString);
        }

        Action<string> QueryBindDataGridView => (sqlString) =>
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            dataGridViewX1.DataSource = fsql.Value.Ado.ExecuteDataTable(CommandType.Text, sqlString);
            stopwatch.Stop();
            labelItem1.Text = $"查询耗时：{stopwatch.ElapsedMilliseconds} 毫秒";
        };

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(QueryBindDataGridView, editor.Text);
        }
    }
}
