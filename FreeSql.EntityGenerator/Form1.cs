using FreeSql.DatabaseModel;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreeSql.EntityGenerator {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void linkLabelGithubWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://github.com/2881099/FreeSql/wiki");
		}
		private void linkLabelGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://github.com/2881099/FreeSql");
		}
		private void linkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://freesql.net/");
		}

		IFreeSql fsql;

		private void buttonConnect_Click(object sender, EventArgs e) {
			this.comboBoxConnectionString.Enabled = false;
			this.buttonConnect.Enabled = false;
			if (buttonConnect.Text == "Connect") {
				fsql?.Dispose();
				var connectionString = this.comboBoxConnectionString.Text.Trim();
				DataType dbtype;
				if (connectionString.StartsWith("[MySql] ")) {
					dbtype = DataType.MySql;
					connectionString = connectionString.Substring(8);
				} else if (connectionString.StartsWith("[PgSql] ")) {
					dbtype = DataType.PostgreSQL;
					connectionString = connectionString.Substring(8);
				} else if (connectionString.StartsWith("[SqlServer] ")) {
					dbtype = DataType.SqlServer;
					connectionString = connectionString.Substring(12);
				} else if (connectionString.StartsWith("[Oracle] ")) {
					dbtype = DataType.Oracle;
					connectionString = connectionString.Substring(9);
				} else {
					this.comboBoxConnectionString.Enabled = true;
					this.buttonConnect.Enabled = true;
					MessageBox.Show("Connection String 错误，格式为：[MySql] 连接字符串内容", "FreeSql", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				fsql = new FreeSqlBuilder()
					.UseConnectionString(dbtype, connectionString)
					.Build();

				var dbs = fsql.DbFirst.GetDatabases();
				this.checkedListBoxDatabases.Items.AddRange(dbs.ToArray());

				this.groupBoxDatabase.Enabled =
				this.groupBoxNameOptions.Enabled =
				this.groupBoxTemplateStyle.Enabled =
				this.groupBoxOutput.Enabled = true;

				this.buttonConnect.Text = "Disconnect";
				this.buttonConnect.Enabled = true;
			} else {
				fsql?.Dispose();

				this.groupBoxDatabase.Enabled =
				this.groupBoxNameOptions.Enabled =
				this.groupBoxTemplateStyle.Enabled =
				this.groupBoxOutput.Enabled = false;

				this.comboBoxConnectionString.Enabled = true;
				this.buttonConnect.Text = "Connect";
				this.buttonConnect.Enabled = true;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			fsql?.Dispose();
		}

		protected string UFString(string text) {
			text = Regex.Replace(text, @"[^\w]", "_");
			if (text.Length <= 1) return text.ToUpper();
			else return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1);
		}
		protected string LFString(string text) {
			if (text.Length <= 1) return text.ToLower();
			else return text.Substring(0, 1).ToLower() + text.Substring(1, text.Length - 1);
		}
		protected string GetCsEntityName(string dbname) {
			var name = Regex.Replace(dbname.TrimStart('@', '.'), @"[^\w]", "_");
			name = char.IsLetter(name, 0) ? name : string.Concat("_", name);
			if (this.checkBoxNameOptionsEntity01.Checked) name = UFString(name);
			if (this.checkBoxNameOptionsEntity02.Checked) name = UFString(name.ToLower());
			if (this.checkBoxNameOptionsEntity03.Checked) name = name.ToLower();
			if (this.checkBoxNameOptionsEntity04.Checked) name = string.Join("", name.Split('_').Select(a => UFString(a)));
			return name;
		}
		protected string GetCsPropertyName(string dbname) {
			var name = Regex.Replace(dbname.TrimStart('@', '.'), @"[^\w]", "_");
			name = char.IsLetter(name, 0) ? name : string.Concat("_", name);
			if (this.checkBoxNameOptionsProperty01.Checked) name = UFString(name);
			if (this.checkBoxNameOptionsProperty02.Checked) name = UFString(name.ToLower());
			if (this.checkBoxNameOptionsProperty03.Checked) name = name.ToLower();
			if (this.checkBoxNameOptionsProperty04.Checked) name = string.Join("", name.Split('_').Select(a => UFString(a)));
			return name;
		}



		private void checkBoxNameOptionsEntity01_CheckedChanged(object sender, EventArgs e) {
			if (_tables == null) return;
			foreach (var table in _tables) table.CsName = GetCsEntityName(table.FullName);
			BindGridView();
		}

		private void checkedListBoxDatabases_SelectedValueChanged(object sender, EventArgs e) {
			List<object> dbs = new List<object>();
			foreach (var item in this.checkedListBoxDatabases.CheckedItems)
				dbs.Add(item);
			if (dbs.Any() == false) {
				_tables = null;
				BindGridView();
				return;
			}

			_tables = fsql?.DbFirst.GetTablesByDatabase(dbs.Select(a => a?.ToString()).ToArray())?.Select(a => new BuildTableInfo { Schema = a, CsName = GetCsEntityName($"{a.Schema}.{a.Name}"), IsOutput = false }).ToList();
			BindGridView();
		}
		private void checkedListBoxDatabases_ItemCheck(object sender, ItemCheckEventArgs e) {
		}
		private void checkedListBoxDatabases_SelectedIndexChanged(object sender, EventArgs e) {
		}

		class BuildTableInfo {
			public DbTableInfo Schema;
			public string CsName { get; set; }

			public bool IsOutput { get; set; }
			public string FullName => $"{Schema.Schema}.{Schema.Name}".TrimStart('.');
			public string Comment => Schema.Comment;
		}

		List<BuildTableInfo> _tables;
		private void BindGridView() {
			var dgvColName = new DataGridViewLinkColumn();
			dgvColName.Name = "dgvColName";
			dgvColName.DefaultCellStyle.SelectionBackColor = Color.White;
			dgvColName.DataPropertyName = "FullName";
			dgvColName.HeaderText = "DbName";
			dgvColName.DisplayIndex = 1;
			dgvColName.Width = 206;

			var dgvColCsName = new DataGridViewTextBoxColumn();
			dgvColCsName.Name = "dgvColCsName";
			//dgvColCsName.DefaultCellStyle.SelectionBackColor = Color.White;
			dgvColCsName.DataPropertyName = "CsName";
			dgvColCsName.HeaderText = "CsName";
			dgvColCsName.DisplayIndex = 2;
			dgvColCsName.Width = 206;

			var dgvColComment = new DataGridViewTextBoxColumn();
			dgvColComment.Name = "dgvColComment";
			//dgvColComment.DefaultCellStyle.SelectionBackColor = Color.White;
			dgvColComment.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#999999");
			dgvColComment.DataPropertyName = "Comment";
			dgvColComment.HeaderText = "Comment";
			dgvColComment.DisplayIndex = 3;
			dgvColComment.Width = 206;

			var dgvColIsOutput = new DataGridViewCheckBoxColumn();
			dgvColIsOutput.Name = "dgvColIsOutput";
			dgvColIsOutput.DefaultCellStyle.SelectionBackColor = Color.White;
			dgvColIsOutput.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgvColIsOutput.DataPropertyName = "IsOutput";
			dgvColIsOutput.HeaderText = "Ins Sel";
			dgvColIsOutput.DisplayIndex = 4;
			dgvColIsOutput.Width = 60;

			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = null;

			this.dataGridView1.Columns.Clear();
			this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]{
				dgvColName,
				dgvColCsName,
				dgvColComment,
				dgvColIsOutput
			});

			this.dataGridView1.DataSource = _tables;
		}

		private void txtProject_TextChanged(object sender, EventArgs e) {
			this.buttonBuild.Enabled = this._tables != null && this._tables.Count > 0 && this.comboBoxOutputDirectory.Text.Trim() != string.Empty;
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
		}

		private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
			if (e.Button == MouseButtons.Left && e.ColumnIndex == 3 && this._tables != null) {
				foreach (var table in _tables) table.IsOutput = !table.IsOutput;
				this.BindGridView();
			}
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex >= 0) {
				DataGridViewColumn column = ((DataGridView)sender).Columns[e.ColumnIndex];
				DataGridViewRow row = ((DataGridView)sender).Rows[e.RowIndex];
				if (column.Name == "dgvColIsOutput") {
					txtProject_TextChanged(sender, e);
				}
			}
		}
		private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			e.Cancel = true;
		}
		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
			if (e.ColumnIndex == 0 && this._tables != null && e.RowIndex < this._tables.Count) {
				switch (this._tables[e.RowIndex].Schema.Type) {
					case DbTableType.StoreProcedure:
						e.CellStyle.BackColor = ColorTranslator.FromHtml("#CDEDFC");
						break;
					case DbTableType.VIEW:
						e.CellStyle.BackColor = ColorTranslator.FromHtml("#EDCDFC");
						break;
				}
			}
		}
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
			DataGridView dgv = sender as DataGridView;
			if (dgv != null) {
				bool isFrmMain = dgv.FindForm() is Form1;
				if (e.RowIndex >= 0) {
					DataGridViewColumn column = dgv.Columns[e.ColumnIndex];
					DataGridViewRow row = dgv.Rows[e.RowIndex];
					if (isFrmMain && column.Name == "dgvColName" || column.Name == "dgvColView") {
						string pdgvColName = string.Concat(row.Cells["dgvColName"].Value);
						string dgvColValue = string.Concat(column.Name == "dgvColView" ? row.Cells["dgvColView"].Value : null);
						string viewTable = isFrmMain ? pdgvColName : row.Cells["dgvColView"].Tag.ToString();
						string name = isFrmMain ? pdgvColName : dgv.Tag.ToString();

						var table = _tables.Find(table1 => viewTable == table1.FullName);
						if (table == null) return;

						FrmView frmView = new FrmView();
						frmView.Text = isFrmMain ? (name + " - view") :
							(name + "." + pdgvColName + " - " + table.FullName + " - relation view");

						frmView.dataGridView1.Tag = viewTable;
						foreach (var c1 in table.Schema.Columns) {
							string viewText = null;
							object image = c1.IsPrimary ? this.imageList1.Images["PrimaryKey.ico"] : null;

							table.Schema.Foreigns.FindAll(fk => {
								var c2 = fk.Columns.Find(c3 => c3.Name == c1.Name);
								if (c2 != null) {
									viewTable = $"{fk.ReferencedTable.Schema}.{fk.ReferencedTable.Name}";
									viewText = "View";
									if (image == null) image = imageList1.Images["Key.ico"];
								}
								return c2 != null;
							});

							frmView.dataGridView1.Rows.Add(new object[] { image, c1.Name, GetCsPropertyName(c1.Name), c1.DbTypeText, c1.IsNullable, viewText, c1.Coment });
							if (viewText != null) frmView.dataGridView1.Rows[frmView.dataGridView1.Rows.Count - 1].Cells["dgvColView"].Tag = viewTable;
						}

						frmView.dataGridView1.CellContentClick += dataGridView1_CellContentClick;
						frmView.ShowDialog();
						frmView.Dispose();
					}
				}
			}
		}

		private void buttonBuild_Click(object sender, EventArgs e) {

			var config = new TemplateServiceConfiguration();
			config.EncodedStringFactory = new RawStringFactory();
			var service = RazorEngineService.Create(config);

			Engine.Razor = service;

			foreach (var table in _tables) {
				var content = Engine.Razor.RunCompile(@"@using FreeSql.DatabaseModel;@{

	var model = Model as RazorModel;

	IFreeSql fsql = model.fsql;
	List<DbTableInfo> tables = model.tables;
	DbTableInfo table = model.table;
	Func<string, string> GetEntityName = model.GetEntityName;
	Func<string, string> GetPropertyName = model.GetPropertyName;

	Func<DbColumnInfo, string> GetCsType = cola3 => {
		return fsql.DbFirst.GetCsType(cola3);
	};

	var tableName = string.IsNullOrEmpty(table.Schema) ? table.Schema + ""."" : """";
	tableName += table.Name;

	Func<string> GetTableAttribute = () => {
		var sb = new List<string>();

		if (GetEntityName(tableName) != tableName)
			sb.Add(""Name = \"""" + tableName + ""\"""");

		if (sb.Any() == false) return null;
		return "", Table("" + string.Join("", "", sb) + "")"";
	};
	Func<DbColumnInfo, string> GetColumnAttribute = col => {
		var sb = new List<string>();

		if (GetPropertyName(col.Name) != col.Name)
			sb.Add(""Name = \"""" + col.Name + ""\"""");

		var dbinfo = model.GetDbInfo(col);
		if (dbinfo != null && dbinfo.dbtypeFull != col.DbTypeTextFull)
			sb.Add(""DbType = \"""" + col.DbTypeTextFull + ""\"""");

		if (col.IsPrimary && string.Compare(col.Name, ""id"", true) != 0 && col.IsIdentity == false)
			sb.Add(""IsPrimary = true"");

		if (col.IsIdentity)
			sb.Add(""IsPrimary = true"");

		if (dbinfo != null && dbinfo.isnullable != col.IsNullable) {
			if (col.IsNullable && GetCsType(col).Contains(""?"") == false && col.CsType.IsValueType)
				sb.Add(""IsNullable = true"");
			if (col.IsNullable == false && GetCsType(col).Contains(""?"") == true)
				sb.Add(""IsNullable = false"");
		}

		if (sb.Any() == false) return null;
		return "", Column("" + string.Join("", "", sb) + "")"";
	};

}@{
switch (fsql.Ado.DataType) {
	case FreeSql.DataType.PostgreSQL:
@:using System;
@:using System.Collections;
@:using System.Collections.Generic;
@:using System.Linq;
@:using System.Reflection;
@:using System.Threading.Tasks;
@:using Newtonsoft.Json;
@:using FreeSql.DataAnnotations;
@:using System.Net;
@:using Newtonsoft.Json.Linq;
@:using System.Net.NetworkInformation;
@:using NpgsqlTypes;
@:using Npgsql.LegacyPostgis;
		break;
	case FreeSql.DataType.SqlServer:
	case FreeSql.DataType.MySql:
	default:
@:using System;
@:using System.Collections;
@:using System.Collections.Generic;
@:using System.Linq;
@:using System.Reflection;
@:using System.Threading.Tasks;
@:using Newtonsoft.Json;
@:using FreeSql.DataAnnotations;
		break;
}
}

namespace test.Model {

@if (string.IsNullOrEmpty(table.Comment) == false) {
	@:/// <summary>
	@:/// @table.Comment.Replace(""\r\n"", ""\n"").Replace(""\n"", ""\r\n		/// "")
	@:/// </summary>
}
	[JsonObject(MemberSerialization.OptIn)@GetTableAttribute()]
	public class @GetEntityName(tableName) {

	@foreach (var col in table.Columns) {

		if (string.IsNullOrEmpty(col.Coment) == false) {
		@:/// <summary>
		@:/// @col.Coment.Replace(""\r\n"", ""\n"").Replace(""\n"", ""\r\n		/// "")
		@:/// </summary>
		}
		@:@(""[JsonProperty"" + GetColumnAttribute(col) + ""]"")
		@:public @GetCsType(col) @GetPropertyName(col.Name) { get; set; }
@:
	}
	}
}", Guid.NewGuid().ToString("N"), null, new RazorModel {
					fsql = fsql,
					tables = _tables.Select(a => a.Schema).ToList(),
					GetEntityName = GetCsEntityName,
					GetPropertyName = GetCsPropertyName,
					table = table.Schema
				});
			}
		}
	}
}

public class RazorModel {
	public IFreeSql fsql { get; set; }
	public List<DbTableInfo> tables { get; set; }
	public DbTableInfo table { get; set; }
	public Func<string, string> GetEntityName { get; set; }
	public Func<string, string> GetPropertyName { get; set; }

	public Func<DbColumnInfo, GetDbInfoModel> GetDbInfo => col => {
		var info = fsql.CodeFirst.GetDbInfo(col.CsType);
		if (info == null) return null;
		return new GetDbInfoModel {
			type = info.Value.type,
			dbtype = info.Value.dbtype,
			dbtypeFull = info.Value.dbtypeFull.Replace(" NOT NULL", ""),
			isnullable = info.Value.isnullable,
			defaultValue = info.Value.defaultValue
		};
	};
}

public class GetDbInfoModel {
	public int type { get; set; }
	public string dbtype { get; set; }
	public string dbtypeFull { get; set; }
	public bool? isnullable { get; set; }
	public object defaultValue { get; set; }
}