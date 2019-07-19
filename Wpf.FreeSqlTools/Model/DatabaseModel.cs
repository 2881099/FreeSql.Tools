using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wpf.FreeSqlTools.Model
{

    [Table(Name = "sys_database")]
    public class DatabaseModel : ViewBaseModel
    {
        public DatabaseModel()
        {
            this.IsEnabled = true;
        }
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => OnPropertyChanged(ref _id, value, RaisePropertyChanged());
        }

        private string _name;
        /// <summary>
        ///  
        /// </summary>
        public string Name
        {
            get => _name;
            set => OnPropertyChanged(ref _name, value, RaisePropertyChanged());
        }


        private string _username;
        /// <summary>
        ///  
        /// </summary>
        public string UserName
        {
            get => _username;
            set => OnPropertyChanged(ref _username, value, RaisePropertyChanged());
        }

        private string _password;
        /// <summary>
        ///  
        /// </summary>
        public string Password
        {
            get => _password;
            set => OnPropertyChanged(ref _password, value, RaisePropertyChanged());
        }

        private DataType _dataType;
        /// <summary>
        ///  
        /// </summary>
        public DataType DataType
        {
            get => _dataType;
            set
            {
                SetDataType(value);
                OnPropertyChanged(ref _dataType, value, RaisePropertyChanged());
            }
        }


        void SetDataType(DataType dataType)
        {

            if (dataType == DataType.MySql)
            {
                this.Port = 3306;
                this.InitDatabaseName = "";
            }
            else if (dataType == DataType.SqlServer)
            {
                this.Port = 1433;
                this.InitDatabaseName = "master";
            }
            else if (dataType == DataType.PostgreSQL)
            {
                this.Port = 5432;
                this.InitDatabaseName = "postgres";
            }
            else if (dataType == DataType.Sqlite)
            {
                this.InitDatabaseName = "";
                this.Port = 0;
            }
            else if (dataType == DataType.Oracle)
            {
                this.InitDatabaseName = "";
                this.Port = 1521;
            }
        }

        private string _initDatabaseName;
        /// <summary>
        ///  
        /// </summary>
        public string InitDatabaseName
        {
            get => _initDatabaseName;
            set => OnPropertyChanged(ref _initDatabaseName, value, RaisePropertyChanged());
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


        private bool _isEnabled;
        /// <summary>
        ///  
        /// </summary>
        [Column(IsIgnore = true)]
        public bool IsEnabled
        {
            get => _isEnabled;
            set => OnPropertyChanged(ref _isEnabled, value, RaisePropertyChanged());
        }


        private string _connection;
        /// <summary>
        ///  
        /// </summary>
        public string Connection
        {
            get => _connection;
            set => OnPropertyChanged(ref _connection, value, RaisePropertyChanged());
        }



        private bool _isCustom;
        /// <summary>
        ///  
        /// </summary>
        public bool IsCustom
        {
            get => _isCustom;
            set
            {
                IsEnabled = !value;
                OnPropertyChanged(ref _isCustom, value, RaisePropertyChanged());
            }
        }



        public bool GetConnection(out string msg)
        {

            msg = string.Empty;
            try
            {
                string connection = this.Connection;
                if (!this.IsCustom)
                {
                    #region 配置数据库连接串
                    switch (this.DataType)
                    {
                        case DataType.MySql:
                            connection = $"Data Source={this.ServeAddress};Port={this.Port};User ID={this.UserName};Password={this.Password};Initial Catalog={(string.IsNullOrEmpty(this.InitDatabaseName) ? "mysql" : this.InitDatabaseName)};Charset=utf8;SslMode=none;Max pool size=5";
                            break;
                        case DataType.SqlServer:
                            connection = $"Data Source={this.ServeAddress},{this.Port};Initial Catalog={this.InitDatabaseName};User ID={this.UserName};Password={this.Password};Pooling=true;Max Pool Size=5";
                            break;
                        case DataType.PostgreSQL:
                            connection = $"Host={this.ServeAddress};Port={this.Port};Username={this.UserName};Password={this.Password};Database={(string.IsNullOrEmpty(this.InitDatabaseName) ? "postgres" : this.InitDatabaseName)};Pooling=true;Maximum Pool Size=5";
                            break;
                        case DataType.Oracle:
                            connection = $"user id={this.UserName};password={this.Password};data source=//{this.ServeAddress}:{this.Port}/{this.InitDatabaseName};Pooling=true;Max Pool Size=5";
                            break;
                    }
                    #endregion
                }
                using (IFreeSql fsql = new FreeSqlBuilder().UseConnectionString(this.DataType,
                    connection).Build())
                {
                    fsql.DbFirst.GetDatabases();
                    msg = "连接成功";
                    return true;
                }
            }catch(Exception e)
            {
                msg = e.Message;
                return false;
            }

        }


    }
}
