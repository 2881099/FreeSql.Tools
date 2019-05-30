using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools.Models
{
    public class DataBaseConfig
    {

        [FreeSql.DataAnnotations.Column(IsPrimary = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
    
        public string DataBaseName { get; set; }
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public FreeSql.DataType DataType { get; set; }
        public string Note { get; set; }

        public string ConnectionStrings { get; set; }

    }
}
