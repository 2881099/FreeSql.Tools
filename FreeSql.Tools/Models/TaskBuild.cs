using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Models
{
    public class TaskBuild
    {

        [FreeSql.DataAnnotations.Column(IsPrimary = true)]
        public Guid Id { get; set; }

        public string TaskName { get; set; }

        public Guid DataBaseConfigId { get; set; }

        public string TableInfos { get; set; }

        public Guid TemplatesId { get; set; }

        public string GeneratePath { get; set; }

        public string FileName { get; set; }

        public virtual Templates Templates { get; set; }
        public virtual DataBaseConfig  DataBaseConfig { get; set; }
    }
}
