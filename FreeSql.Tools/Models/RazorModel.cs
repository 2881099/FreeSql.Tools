using FreeSql.DatabaseModel;
using FreeSqlTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class RazorModel {
	public RazorModel(IFreeSql fsql, TaskBuild task, DbTableInfo table) {
		this.fsql = fsql;
		this.task = task;
		this.table = table;
	}

	public IFreeSql fsql { get; set; }
	public TaskBuild task { get; set; }
	public List<DbColumnInfo> Columns => this.table.Columns;
	public DbTableInfo table { get; set; }
	public string NameSpace => task.NamespaceName;
	public string FullTableName => $"{table.Schema}.{table.Name}".TrimStart('.');

	public string GetCsType(DbColumnInfo col) => fsql.DbFirst.GetCsType(col);
	public string GetCsName(string name) {
		name = Regex.Replace(name.TrimStart('@', '.'), @"[^\w]", "_");
		name = char.IsLetter(name, 0) ? name : string.Concat("_", name);
		if (task.OptionsEntity01) name = UFString(name);
		if (task.OptionsEntity02) name = UFString(name.ToLower());
		if (task.OptionsEntity03) name = name.ToLower();
		if (task.OptionsEntity04) name = string.Join("", name.Split('_').Select(a => UFString(a)));
		return name;
	}
	public string UFString(string text) {
		text = Regex.Replace(text, @"[^\w]", "_");
		if (text.Length <= 1) return text.ToUpper();
		else return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1);
	}
	public string LFString(string text) {
		text = Regex.Replace(text, @"[^\w]", "_");
		if (text.Length <= 1) return text.ToLower();
		else return text.Substring(0, 1).ToLower() + text.Substring(1, text.Length - 1);
	}

	#region 特性
	public string GetTableAttribute() {
		var sb = new List<string>();

		if (GetCsName(this.FullTableName) != this.FullTableName)
			sb.Add("Name = \"" + this.FullTableName + "\"");

		if (sb.Any() == false) return null;
		return "[Table(" + string.Join(", ", sb) + ")]";
	}
	public string GetColumnAttribute(DbColumnInfo col) {
		var sb = new List<string>();

		if (GetCsName(col.Name) != col.Name)
			sb.Add("Name = \"" + col.Name + "\"");

		var dbinfo = fsql.CodeFirst.GetDbInfo(col.CsType);
		if (dbinfo != null && dbinfo.Value.dbtypeFull.Replace("NOT NULL", "").Trim() != col.DbTypeTextFull)
			sb.Add("DbType = \"" + col.DbTypeTextFull + "\"");
		if (col.IsPrimary && string.Compare(col.Name, "id", true) != 0 && col.IsIdentity == false)
			sb.Add("IsPrimary = true");
		if (col.IsIdentity)
			sb.Add("IsPrimary = true");
		if (dbinfo != null && dbinfo.Value.isnullable != col.IsNullable) {
			if (col.IsNullable && fsql.DbFirst.GetCsType(col).Contains("?") == false && col.CsType.IsValueType)
				sb.Add("IsNullable = true");
			if (col.IsNullable == false && fsql.DbFirst.GetCsType(col).Contains("?") == true)
				sb.Add("IsNullable = false");
		}
		if (sb.Any() == false) return null;
		return "[Column(" + string.Join(", ", sb) + ")]";
	}
	#endregion
}

