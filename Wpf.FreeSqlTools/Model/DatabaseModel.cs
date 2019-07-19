using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wpf.FreeSqlTools.Model
{

    [Table(Name = "sys.database")]
    public class DatabaseModel: ViewBaseModel
    {
        private int _id;   
        public int Id
        {
            get => _id;
            set=>OnPropertyChanged(ref _id, value, RaisePropertyChanged());            
        }

        private string _name;
        /// <summary>
        ///  
        /// </summary>
        public string Name
        {
            get =>  _name;
            set => OnPropertyChanged(ref _name, value, RaisePropertyChanged());
        }

        private DataType _dataType;
        /// <summary>
        ///  
        /// </summary>
        public DataType DataType
        {
            get => _dataType;
            set {

                if (_dataType == DataType.MySql) this.Port = 3306;
                else if (_dataType == DataType.SqlServer) this.Port = 1433;
                else if (_dataType == DataType.PostgreSQL) this.Port = 5432;
                else if (_dataType == DataType.Sqlite) this.Port = 0;
                else if (_dataType == DataType.Oracle) this.Port = 1521;
                OnPropertyChanged(ref _dataType, value, RaisePropertyChanged());
            }
        }


        private string _serveAddress;
        /// <summary>
        ///  
        /// </summary>
        public string ServeAddress
        {
            get => _serveAddress;
            set => OnPropertyChanged(ref _serveAddress, value, RaisePropertyChanged());
        }


        private int _port;
        /// <summary>
        ///  
        /// </summary>
        public int Port
        {
            get => _port;
            set => OnPropertyChanged(ref _port, value, RaisePropertyChanged());
        }



    }
}
