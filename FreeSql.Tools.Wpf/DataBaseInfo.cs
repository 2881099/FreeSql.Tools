using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FreeSqlTools
{

    [Serializable]
    public class DataBaseInfo
    {
        string path = Path.Combine(Environment.CurrentDirectory, "database.bin");
        List<DataBaseInfo> dataBases = new List<DataBaseInfo>();
        public DataBaseInfo()
        {
            dataBases = Load();
        }
        public string Name { get; set; }
        public List<DataBaseInfo> GetDataBaseInfos() => dataBases;
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Pwd { get; set; }
        public string Host { get; set; }
        public bool IsString { get; set; } = false;
        public string ConnectionString { get; set; }
        public FreeSql.DataType DataType { get; set; } = FreeSql.DataType.MySql;

        public int ValidatorType { get; set; } = 0;

        public string Port { get; set; }
        public string DbName { get; set; }
        public void Add()
        {
            dataBases.Add(this);
            this.Save();
        }
        public void Delete(Guid id)
        {
            var model = dataBases.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                dataBases.Remove(model);
                this.Save();
            }
        }
        public void Update()
        {
            var model = dataBases.Where(a => a.Id == this.Id).FirstOrDefault();
            if (model != null)
            {
                this.Id = model.Id;
                this.UserId = model.UserId;
                this.Pwd = model.Pwd;
                this.Host = model.Host;
                this.Port = model.Port;
                this.DbName = model.DbName;
                this.Save();
            }
        }
        void Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                File.WriteAllBytes(path, ms.GetBuffer());
            }
        }
        public List<DataBaseInfo> Load()
        {
            if (!File.Exists(path))
            {
                return new List<DataBaseInfo>();
            }

            using (Stream destream = new FileStream(path, FileMode.Open,
                         FileAccess.Read, FileShare.Read))
            {
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    var entity = (DataBaseInfo)formatter.Deserialize(destream);
                    return entity.dataBases;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new List<DataBaseInfo>();
                }
            }
        }
    }
}