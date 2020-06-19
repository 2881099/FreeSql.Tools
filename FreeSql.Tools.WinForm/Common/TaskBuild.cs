using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Common
{
    public class TaskBuild
    {       

        public string GeneratePath { get; set; }
        public string FileName { get; set; }
        public string DbName { get; set; }
        public IFreeSql Fsql { get; set; }
        public string RemoveStr { get; set; }
        public string NamespaceName { get; set; }
        /// <summary>
        /// 首字母大写
        /// </summary>
        public bool OptionsEntity01 { get; set; } = false;
        /// <summary>
        /// 首字母大写,其他小写
        /// </summary>
        public bool OptionsEntity02 { get; set; } = false;
        /// <summary>
        /// 全部小写
        /// </summary>
        public bool OptionsEntity03 { get; set; } = false;
        /// <summary>
        /// 下划线转驼峰
        /// </summary>
        public bool OptionsEntity04 { get; set; } = false;
        public ICollection<string> Templates { get; set; }      

    }
}
