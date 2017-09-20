namespace X射线荧光分析查询软件.Design
{
    using System;
    using System.Data.Linq;
    using X射线荧光分析查询软件.Model;

    public class DesignDataService : IDataService
    {

        public void GetData(Action<Table<Element>, Exception> callback)
        {
            Element _H_ = new Element()
            {
                序数 = 1,
                名称 = "氢",
                拼音 = "qīng",
                符号 = "H",
                英文名称 = "Hydrogen",
                电子排布 = "1s1",
                密度 = 8.98E-5,
                K吸收限能量 = 0.0136,
                μ1 = 0,
                μ2 = 0,
                Kα1能量 = 0,
                Kα2能量 = 0,
                Kα2比例 = 0,
                Kβ1能量 = 0,
                Kβ1比例 = 0,
                Kβ2能量 = 0,
                Kβ2比例 = 0,
                LI = 0,
                LII = 0,
                LIII = 0,
                Lα1能量 = 0,
                Lα2能量 = 0,
                Lβ1能量 = 0,
                Lβ2能量 = 0,
                Lγ1能量 = 0,
                ωK = 0,
                ωL = 0,
                Tag = ""
            };

            // var item = new DataItem("Welcome to MVVM Light [design]");
            callback(null, null);
        }
    }
}