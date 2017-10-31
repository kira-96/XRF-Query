namespace X射线荧光分析查询软件.Model
{
    using System;
    using System.Data.Linq;
    using System.Data.SQLite;
    using System.IO;

    public class DataService : IDataService
    {
        /// <summary>
        /// 数据库路径
        /// </summary>
        private readonly string _dbPath = Environment.CurrentDirectory + "\\DB\\db.SQLITE3";

        public void GetData(Action<Table<Element>, Exception> callback)
        {
            if (!File.Exists(_dbPath))
            {
                callback(null, new Exception("数据库丢失！"));
                return;
            }
            try
            {
                // Use this to connect to the actual data service
                SQLiteConnection conn = new SQLiteConnection("Data Source = " + _dbPath);  // 连接到数据库
                DataContext context = new DataContext(conn);  // 创建DataContext
                Table<Element> elements = context.GetTable<Element>();  // 获取表中数据
                // var item = new DataItem("Welcome to MVVM Light");
                callback(elements, null);
            }
            catch (Exception e)
            {
                callback(null, e);
            }
        }
    }
}