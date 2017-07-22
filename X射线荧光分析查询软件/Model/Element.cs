namespace X射线荧光分析查询软件.Model
{
    using System;
    using System.Data.Linq.Mapping;

    /// <summary>
    /// 与数据库表格匹配的类
    /// 用于将数据库中的数据映射为对象
    /// 可更改Table Name 和 Column Name
    /// 但必须与数据库的表名和列相匹配
    /// 否则将不能读到数据，并且程序将崩溃
    /// </summary>

    [Table(Name = "特征X射线")]
    public class Element
    {
        [Column(Name = "ID")]
        public int 序数 { get; set; }
        [Column(Name = "元素名称")]
        public string 名称 { get; set; }
        [Column(Name = "拼音")]
        public string 拼音 { get; set; }
        [Column(Name = "元素符号")]
        public string 符号 { get; set; }
        [Column(Name = "英文名称")]
        public string 英文名称 { get; set; }
        [Column(Name = "电子排布")]
        public string 电子排布 { get; set; }
        [Column(Name = "密度")]
        public double 密度 { get; set; }
        [Column(Name = "K吸收限能量")]
        public double K吸收限能量 { get; set; }
        [Column(Name = "质量吸收系数μ1")]
        public double μ1 { get; set; }
        [Column(Name = "质量吸收系数μ2")]
        public double μ2 { get; set; }
        
        public double μ1比μ2 => M1比M2();

        [Column(Name = "Kα1能量")]
        public double Kα1能量 { get; set; }
        [Column(Name = "Kα2能量")]
        public double Kα2能量 { get; set; }
        [Column(Name = "Kα2比例")]
        public double Kα2比例 { get; set; }

        public double Kα平均值 => Kα有效平均值();

        [Column(Name = "Kβ1能量")]
        public double Kβ1能量 { get; set; }
        [Column(Name = "Kβ1比例")]
        public double Kβ1比例 { get; set; }
        [Column(Name = "Kβ2能量")]
        public double Kβ2能量 { get; set; }
        [Column(Name = "Kβ2比例")]
        public double Kβ2比例 { get; set; }
        [Column(Name = "ωK")]
        public double ωK { get; set; }
        [Column(Name = "LI")]
        public double LI { get; set; }
        [Column(Name = "LII")]
        public double LII { get; set; }
        [Column(Name = "LIII")]
        public double LIII { get; set; }
        [Column(Name = "Lα1能量")]
        public double Lα1能量 { get; set; }
        [Column(Name = "Lα2能量")]
        public double Lα2能量 { get; set; }
        [Column(Name = "Lβ1能量")]
        public double Lβ1能量 { get; set; }
        [Column(Name = "Lβ2能量")]
        public double Lβ2能量 { get; set; }
        [Column(Name = "Lγ1能量")]
        public double Lγ1能量 { get; set; }
        [Column(Name = "ωL")]
        public double ωL { get; set; }

        /// <summary>
        /// <see cref="Tag"/>
        /// </summary>
        private string _Tag = string.Empty;

        /// <summary>
        /// 显示能量信息
        /// </summary>
        public string Tag
        {
            get => _Tag;
            set { _Tag = value; }
        }


        private double Kα有效平均值()
        {
            return Math.Round((Kα1能量 * 100 + Kα2能量 * Kα2比例) / (100 + Kα2比例), 4);
        }

        private double M1比M2()
        {
            return Math.Round(μ1 / μ2, 4);
        }

        /// <summary>
        /// 重载ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return 序数 + " " + 名称 + " " + 符号 + " ";
        }
    }
}
