using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Models
{
    public class TaskBuildInfo
    {
        [Column(IsPrimary = true)]
        public Guid Id { get; set; }
        public Guid TaskBuildId { get; set; }
        public Guid DataBaseConfigId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        

        public DataBaseConfig DataBaseConfig { get; set; }

        public TaskBuild TaskBuild { get; set; }
    }

    public class TaskBuild
    {       


        [Column(IsPrimary = true)]
        public Guid Id { get; set; }
        public string TaskName { get; set; }   
        public Guid TemplatesId { get; set; }
        public string GeneratePath { get; set; }
        public string FileName { get; set; }
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
        public DateTime AddTime { get; set; } = DateTime.Now;

        public ICollection<TaskBuildInfo> TaskBuildInfos { get; set; }
        public  Templates Templates { get; set; }    
       

    }
}
