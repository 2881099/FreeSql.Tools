@using FreeSql.DatabaseModel;@{
var gen = Model as RazorModel;

Func<string, string> GetAttributeString = attr => {
	if (string.IsNullOrEmpty(attr)) return null;
	return string.Concat(", ", attr.Trim('[', ']'));
};
Func<DbColumnInfo, string> GetDefaultValue = col => {
    if (col.CsType == typeof(string)) return " = string.Empty;";
    return "";
};
}@{
switch (gen.fsql.Ado.DataType) {
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
namespace @gen.NameSpace {

@if (string.IsNullOrEmpty(gen.table.Comment) == false) {
	@:/// <summary>
	@:/// @gen.table.Comment.Replace("\r\n", "\n").Replace("\n", "\r\n		/// ")
	@:/// </summary>
}
	[JsonObject(MemberSerialization.OptIn)@GetAttributeString(gen.GetTableAttribute())]
	public partial class @gen.GetCsName(gen.FullTableName) {

	@foreach (var col in gen.columns) {

		if (string.IsNullOrEmpty(col.Coment) == false) {
		@:/// <summary>
		@:/// @col.Coment.Replace("\r\n", "\n").Replace("\n", "\r\n		/// ")
		@:/// </summary>
		}
		@:@("[JsonProperty" + GetAttributeString(gen.GetColumnAttribute(col)) + "]")
		@:public @gen.GetCsType(col) @gen.GetCsName(col.Name) { get; set; }@GetDefaultValue(col)
@:
	}
	}
@gen.GetMySqlEnumSetDefine()
}