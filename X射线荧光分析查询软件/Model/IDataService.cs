namespace X射线荧光分析查询软件.Model
{
    using System;
    using System.Data.Linq;

    public interface IDataService
    {
        void GetData(Action<Table<Element>, Exception> callback);
    }
}
