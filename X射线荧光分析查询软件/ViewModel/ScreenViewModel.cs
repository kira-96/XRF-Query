namespace X射线荧光分析查询软件.ViewModel
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Linq;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Model;

    public class ScreenViewModel : ViewModelBase
    {
        public static readonly Guid QueryToken = Guid.NewGuid();
        private readonly INavigationService _nav = ServiceLocator.Current.GetInstance<INavigationService>();
        private readonly IDialogService _dialog = ServiceLocator.Current.GetInstance<IDialogService>();

        private Table<Element> _elements;
        private Element _选定元素;
        private double _字号 = 18;
        private readonly ObservableCollection<Element> _queryResult = new ObservableCollection<Element>();

        public ObservableCollection<Element> QueryResult => _queryResult;

        /// <summary>
        /// 当前列表中选定的元素
        /// </summary>
        public Element 选定元素
        {
            get => _选定元素;
            set
            {
                Set(ref _选定元素, value);
                if (_选定元素 != null)
                {
                    显示详细信息();
                    // 通知主窗口改变布局
                    Messenger.Default.Send(true, View.MainWindow.LayoutToken);
                }
            }
        }

        /// <summary>
        /// 表格字号
        /// </summary>
        public double 字号
        {
            get => _字号;
            set
            {
                Set(ref _字号, value);
                SizeUpCommand.RaiseCanExecuteChanged();
                SizeDownCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _backCommand, _sizeUpCommand, _sizeDownCommand;
        
        public RelayCommand BackCommand => _backCommand ?? (_backCommand = new RelayCommand(() => 
        {
            Messenger.Default.Send(false, View.MainWindow.LayoutToken);
            _nav.GoBack();
        }));

        /// <summary>
        /// 改变字号
        /// </summary>
        public RelayCommand SizeUpCommand => _sizeUpCommand ?? (_sizeUpCommand = new RelayCommand(FontSizeUp, CanFontSizeUp));
        public RelayCommand SizeDownCommand => _sizeDownCommand ?? (_sizeDownCommand = new RelayCommand(FontSizeDown, CanFontSizeDown));

        /// <summary>
        /// 构造方法
        /// 通过Model.DataService 的GetData方法来获取数据库数据
        /// 参见DataService的GetData方法
        /// </summary>
        /// <param name="dataService">DataService, 见ViewModelLocator 和 Model.DataService</param>
        public ScreenViewModel(IDataService dataService)
        {
            dataService.GetData((elements, error) =>
            {
                if (error != null)
                {
                    _dialog.ShowError(error, "Error", "OK", () =>
                    {
                        elements = null;
                    });
                    // return;
                }
                _elements = elements;
            });
            if (_elements != null)
            {
                显示全部元素();
            }
            // Messenger Moved Here
            Messenger.Default.Register<NotificationMessage<int>>(this, QueryToken, Query);
            /*
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Environment.CurrentDirectory + "\\log.txt"))
            {
                foreach (Element item in _Elements)
                {
                    writer.WriteLine(item.ToString());
                }
            }
            */
        }

        /*
        private void Loaded()
        {
            Messenger.Default.Register<NotificationMessage<int>>(this, QueryToken, Query);
            // 显示全部元素();
        }

        private void Unloaded()
        {
            Messenger.Default.Unregister<NotificationMessage>(this, QueryToken);
        }
        */

        /// <summary>
        /// 查询函数
        /// </summary>
        /// <param name="parameter">查询的分类和信息</param>
        private void Query(NotificationMessage<int> parameter)
        {
            int 分类 = parameter.Content;
            string queryString = parameter.Notification;
            if (_elements == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(queryString))
            {
                显示全部元素();
                return;
            }
            // 分类查询，这里采用Linq的方式
            switch (分类)
            {
                case 0:
                    {
                        /*
                        if (int.TryParse(QueryString, out int ID))
                        {
                            IEnumerable<Element> ele = from e in _Elements.AsEnumerable()
                                                       where e.ID == ID
                                                       select e;
                            UpdateQueryResult(ele);
                        }
                        */
                        IEnumerable<Element> ele = from e in _elements.AsEnumerable()
                                                   where e.序数.ToString().Contains(queryString)
                                                   select e;
                        UpdateQueryResult(ele);
                        break;
                    }
                case 1:
                    {
                        IEnumerable<Element> ele = from e in _elements.AsEnumerable()
                                                   where e.名称 == queryString || 
                                                   RegExp(e.拼音).ToUpper().Contains(queryString.ToUpper())
                                                   select e;
                        UpdateQueryResult(ele);
                        break;
                    }
                case 2:
                    {
                        IEnumerable<Element> ele = from e in _elements.AsEnumerable()
                                                   where e.符号.ToUpper().Contains(queryString.ToUpper())
                                                   select e;
                        UpdateQueryResult(ele);
                        break;
                    }
                case 3:
                    {
                        if (double.TryParse(queryString, out double erg))
                        {
                            double min = 0.95 * erg;
                            double max = 1.05 * erg;
                            IEnumerable<Element> ele = from e in _elements.AsEnumerable()
                                                       where (e.Kα1能量 >= min && e.Kα1能量 <= max) || 
                                                       (e.Kα2能量 >= min && e.Kα2能量 <= max) || 
                                                       (e.Kβ1能量 >= min && e.Kβ1能量 <= max) ||
                                                       (e.Kβ2能量 >= min && e.Kβ2能量 <= max) ||
                                                       (e.K吸收限能量 >= min && e.K吸收限能量 <= max) ||
                                                       (e.LI >= min && e.LI <= max) || 
                                                       (e.LII >= min && e.LII <= max) || 
                                                       (e.LIII >= min && e.LIII <= max) || 
                                                       (e.Lα1能量 >= min && e.Lα1能量 <= max) ||
                                                       (e.Lα2能量 >= min && e.Lα2能量 <= max) ||
                                                       (e.Lβ1能量 >= min && e.Lβ1能量 <= max) ||
                                                       (e.Lβ2能量 >= min && e.Lβ2能量 <= max) ||
                                                       (e.Lγ1能量 >= min && e.Lγ1能量 <= max)
                                                       select e;
                            // UpdateQueryResult(ele);
                            UpdateQueryResultAndAddTag(ele, min, max);
                        }
                        else
                        {
                            _queryResult.Clear();
                            Messenger.Default.Send(false, View.列表视图.LayoutToken);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// 更新视图列表
        /// </summary>
        /// <param name="elements">筛选后的数据列表</param>
        private void UpdateQueryResult(IEnumerable<Element> elements)
        {
            _queryResult.Clear();
            IList<Element> enumerable = elements as IList<Element> ?? elements.ToList();
            if (!enumerable.Any())
            {
                // 显示错误
                Messenger.Default.Send(false, View.列表视图.LayoutToken);
                return;
            }
            foreach (Element e in enumerable)
            {
                _queryResult.Add(e);
            }
            Messenger.Default.Send(true, View.列表视图.LayoutToken);
        }

        /// <summary>
        /// 更新视图列表并显示匹配能量
        /// 2017.07.22
        /// </summary>
        /// <param name="elements">筛选后的数据列表</param>
        /// <param name="min">最小能量</param>
        /// <param name="max">最大能量</param>
        private void UpdateQueryResultAndAddTag(IEnumerable<Element> elements, double min, double max)
        {
            _queryResult.Clear();
            IList<Element> enumerable = elements as IList<Element> ?? elements.ToList();
            if (!enumerable.Any())
            {
                // 显示错误
                Messenger.Default.Send(false, View.列表视图.LayoutToken);
                return;
            }
            foreach (Element e in enumerable)
            {
                string tag = "";
                if (e.K吸收限能量 >= min && e.K吸收限能量 <= max)
                {
                    tag += " Kab: " + e.K吸收限能量;
                }
                if (e.Kα1能量 >= min && e.Kα1能量 <= max)
                {
                    tag += " Kα1: " + e.Kα1能量;
                }
                if (e.Kα2能量 >= min && e.Kα2能量 <= max)
                {
                    tag += " Kα2: " + e.Kα2能量;
                }
                if (e.Kβ1能量 >= min && e.Kβ1能量 <= max)
                {
                    tag += " Kβ1: " + e.Kβ1能量;
                }
                if (e.Kβ2能量 >= min && e.Kβ2能量 <= max)
                {
                    tag += " Kβ2: " + e.Kβ2能量;
                }
                if (e.LI >= min && e.LI <= max)
                {
                    tag += " LI: " + e.LI;
                }
                if (e.LII >= min && e.LII <= max)
                {
                    tag += " LII: " + e.LII;
                }
                if (e.LIII >= min && e.LIII <= max)
                {
                    tag += " LIII: " + e.LIII;
                }
                if (e.Lα1能量 >= min && e.Lα1能量 <= max)
                {
                    tag += " Lα1: " + e.Lα1能量;
                }
                if (e.Lα2能量 >= min && e.Lα2能量 <= max)
                {
                    tag += " Lα2: " + e.Lα2能量;
                }
                if (e.Lβ1能量 >= min && e.Lβ1能量 <= max)
                {
                    tag += " Lβ1: " + e.Lβ1能量;
                }
                if (e.Lβ2能量 >= min && e.Lβ2能量 <= max)
                {
                    tag += " Lβ2: " + e.Lβ2能量;
                }
                if (e.Lγ1能量 >= min && e.Lγ1能量 <= max)
                {
                    tag += " Lγ1:" + e.Lγ1能量;
                }
                e.Tag = tag;
                _queryResult.Add(e);
            }
        }

        /// <summary>
        /// 导航到详情页
        /// </summary>
        private void 显示详细信息()
        {
            _nav.NavigateTo(nameof(View.详情页));
        }

        /// <summary>
        /// 将全部元素数据呈现出来
        /// </summary>
        private void 显示全部元素()
        {
            IEnumerable<Element> ele = from e in _elements.AsEnumerable()
                                       select e;
            UpdateQueryResult(ele);
        }

        private void FontSizeUp()
        {
            字号 += 1;
        }

        private bool CanFontSizeUp()
        {
            return 字号 <= 56;
        }

        private void FontSizeDown()
        {
            字号 -= 1;
        }

        private bool CanFontSizeDown()
        {
            return 字号 >= 8;
        }

        /// <summary>
        /// 通过正则匹配替换掉拼音字符
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>替换后的字符串</returns>
        private static string RegExp(string input)
        {
            string s = input;
            string[] matchs = { "[āáǎà]", "[ōóǒò]", "[ēéěè]", "[īíǐì]", "[ūúǔù]", "[ǖǘǚǜü]" };
            string[] values = { "a", "o", "e", "i", "u", "v" };

            for (int i = 0; i < matchs.Length; i++)
            {
                Regex regex = new Regex(matchs[i]);
                s = regex.Replace(s, values[i]);
            }

            return s;
        }

        public override void Cleanup()
        {
            // Unregister Messenger Here
            Messenger.Default.Unregister<NotificationMessage>(this, QueryToken);
            base.Cleanup();
        }
    }
}
