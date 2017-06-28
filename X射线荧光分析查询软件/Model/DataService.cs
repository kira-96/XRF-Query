namespace X射线荧光分析查询软件.Model
{
    using System;
    using System.Data.Linq;
    using System.Data.SQLite;

    public class DataService : IDataService
    {
        /// <summary>
        /// 数据库路径
        /// </summary>
        private string _DB_PATH_ = "Data Source = " + Environment.CurrentDirectory + "\\DB\\db.SQLITE3";

        public void GetData(Action<Table<Element>, Exception> callback)
        {
            // Use this to connect to the actual data service
            SQLiteConnection conn = new SQLiteConnection(_DB_PATH_);  // 连接到数据库
            DataContext context = new DataContext(conn);  // 创建DataContext
            Table<Element> elements = context.GetTable<Element>();  // 获取表中数据
            // var item = new DataItem("Welcome to MVVM Light");
            callback(elements, null);
        }
    }
}