namespace X射线荧光分析查询软件.Design
{
    using System;
    using System.Data.Linq;
    using System.Data.SQLite;
    using X射线荧光分析查询软件.Model;

    public class DesignDataService : IDataService
    {
        /// <summary>
        /// 数据库路径
        /// </summary>
        private string _DB_PATH_ = "Data Source = " + Environment.CurrentDirectory + "\\DB\\db.SQLITE3";

        public void GetData(Action<Table<Element>, Exception> callback)
        {
            // Use this to create design time data
            SQLiteConnection conn = new SQLiteConnection(_DB_PATH_);
            DataContext context = new DataContext(conn);
            Table<Element> elements = context.GetTable<Element>();

            // var item = new DataItem("Welcome to MVVM Light [design]");
            callback(elements, null);
        }
    }
}