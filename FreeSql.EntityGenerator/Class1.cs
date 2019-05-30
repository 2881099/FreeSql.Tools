namespace CompiledRazorTemplates.Dynamic {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FreeSql.DatabaseModel;
    
    
    [RazorEngine.Compilation.HasDynamicModelAttribute()]
    public class RazorEngine_da52a577fd114dd6a93cc1a519309d3e : RazorEngine.Templating.TemplateBase<dynamic> {
        

        
        public RazorEngine_da52a577fd114dd6a93cc1a519309d3e() {
        }
        
        public override void Execute() {
                               

	var model = Model as RazorModel;

	IFreeSql fsql = model.fsql;
	List<DbTableInfo> tables = model.tables;
	DbTableInfo table = model.table;
	Func<string, string> GetEntityName = model.GetEntityName;
	Func<string, string> GetPropertyName = model.GetPropertyName;

	Func<DbColumnInfo, string> GetCsType = cola3 => {
		return fsql.DbFirst.GetCsType(cola3);
	};

	var tableName = string.IsNullOrEmpty(table.Schema) ? table.Schema + "." : "";
	tableName += table.Name;

	Func<string> GetTableAttribute = () => {
		var sb = new List<string>();

		if (GetEntityName(tableName) != tableName)
			sb.Add("Name = \"" + tableName + "\"");

		if (sb.Any() == false) return null;
		return ", Table(" + string.Join(", ", sb) + ")";
	};
	Func<DbColumnInfo, string> GetColumnAttribute = col => {
		var sb = new List<string>();

		if (GetPropertyName(col.Name) != col.Name)
			sb.Add("Name = \"" + col.Name + "\"");

		if (fsql.CodeFirst.GetDbInfo(col.CsType)?.dbtypeFull != col.DbTypeTextFull)
			sb.Add("DbType = \"" + col.DbTypeTextFull + "\"");

		if (col.IsPrimary && string.Compare(col.Name, "id", true) != 0 && col.IsIdentity == false)
			sb.Add("IsPrimary = true");

		if (col.IsIdentity)
			sb.Add("IsPrimary = true");

		if (fsql.CodeFirst.GetDbInfo(col.CsType)?.isnullable != col.IsNullable)
			sb.Add("IsNullable = " + (col.IsNullable ? "true" : "false"));

		if (sb.Any() == false) return null;
		return ", Column(" + string.Join(", ", sb) + ")";
	};


   
switch (fsql.Ado.DataType) {
	case FreeSql.DataType.PostgreSQL:

WriteLiteral("using System;\r\n");

WriteLiteral("using System.Collections;\r\n");

WriteLiteral("using System.Collections.Generic;\r\n");

WriteLiteral("using System.Linq;\r\n");

WriteLiteral("using System.Reflection;\r\n");

WriteLiteral("using System.Threading.Tasks;\r\n");

WriteLiteral("using Newtonsoft.Json;\r\n");

WriteLiteral("using FreeSql.DataAnnotations;\r\n");

WriteLiteral("using System.Net;\r\n");

WriteLiteral("using Newtonsoft.Json.Linq;\r\n");

WriteLiteral("using System.Net.NetworkInformation;\r\n");

WriteLiteral("using NpgsqlTypes;\r\n");

WriteLiteral("using Npgsql.LegacyPostgis;\r\n");

		break;
	case FreeSql.DataType.SqlServer:
	case FreeSql.DataType.MySql:
	default:

WriteLiteral("using System;\r\n");

WriteLiteral("using System.Collections;\r\n");

WriteLiteral("using System.Collections.Generic;\r\n");

WriteLiteral("using System.Linq;\r\n");

WriteLiteral("using System.Reflection;\r\n");

WriteLiteral("using System.Threading.Tasks;\r\n");

WriteLiteral("using Newtonsoft.Json;\r\n");

WriteLiteral("using FreeSql.DataAnnotations;\r\n");

		break;
}

WriteLiteral("\r\n\r\nnamespace test.Model {\r\n\r\n");

 if (string.IsNullOrEmpty(table.Comment) == false) {

WriteLiteral("\t");

WriteLiteral("/// <summary>\r\n");

WriteLiteral("\t");

WriteLiteral("/// ");

     Write(table.Comment.Replace("\r\n", "\n").Replace("\n", "\r\n		/// "));

WriteLiteral("\r\n");

WriteLiteral("\t");

WriteLiteral("/// </summary>\r\n");

}

WriteLiteral("\t[JsonObject(MemberSerialization.OptIn)");

                                     Write(GetTableAttribute());

WriteLiteral("]\r\n\tpublic class ");

            Write(GetEntityName(tableName));

WriteLiteral(" {\r\n\r\n");

	
     foreach (var col in table.Columns) {

		if (string.IsNullOrEmpty(col.Coment) == false) {

WriteLiteral("\t\t");

WriteLiteral("/// <summary>\r\n");

WriteLiteral("\t\t");

WriteLiteral("/// ");

         Write(col.Coment.Replace("\r\n", "\n").Replace("\n", "\r\n		/// "));

WriteLiteral("\r\n");

WriteLiteral("\t\t");

WriteLiteral("/// </summary>\r\n");

		}

WriteLiteral("\t\t");

WriteLiteral("[JsonProperty ");

                   Write(GetColumnAttribute(col));

WriteLiteral("]\r\n");

WriteLiteral("\t\t");

WriteLiteral("public ");

            Write(GetCsType(col));

WriteLiteral(" ");

                            Write(GetPropertyName(col.Name));

WriteLiteral(" { get; set; }\r\n");

WriteLiteral("\r\n");

	}

WriteLiteral("\t}\r\n}");

        }
    }
}