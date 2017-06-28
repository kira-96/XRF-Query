namespace X射线荧光分析查询软件.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Practices.ServiceLocation;

    public class SearchViewModel : ViewModelBase
    {
        private INavigationService Nav => ServiceLocator.Current.GetInstance<INavigationService>();

        private string _QueryString = "";
        private string _Tip = "原子序数";
        private string _视图 = "列表";
        private int _分类 = 0;

        private RelayCommand _QueryCommand;

        /// <summary>
        /// 文本框中的文本
        /// </summary>
        public string QueryString
        {
            get { return _QueryString; }
            set
            {
                Set(ref _QueryString, value);
                Query();
            }
        }

        /// <summary>
        /// Tip，已废弃
        /// </summary>
        public string Tip
        {
            get { return _Tip; }
            private set { Set(ref _Tip, value); }
        }

        /// <summary>
        /// 查询的分类
        /// </summary>
        public int 分类
        {
            get { return _分类; }
            set
            {
                Set(ref _分类, value);
                /*
                switch (value)
                {
                    case 0:
                        {
                            Tip = "原子序数";
                            break;
                        }
                    case 1:
                        {
                            Tip = "元素名称";
                            break;
                        }
                    case 2:
                        {
                            Tip = "元素符号";
                            break;
                        }
                    case 3:
                        {
                            Tip = "能量";
                            break;
                        }
                    default:
                        break;
                }
                */
            }
        }

        /// <summary>
        /// 数据的呈现方式 列表 or 表格
        /// </summary>
        public string 视图
        {
            get { return _视图; }
            set
            {
                Navigation(value);
                Set(ref _视图, value);
            }
        }

        public RelayCommand QueryCommand => _QueryCommand ?? (_QueryCommand = new RelayCommand(Query));

        public SearchViewModel()
        {
            //
        }

        /// <summary>
        /// 发送查询命令和信息
        /// </summary>
        private void Query()
        {
            Messenger.Default.Send(new NotificationMessage<int>(_分类, _QueryString), ScreenViewModel.QueryToken);
            // Messenger.Default.Send(true, View.MainWindow.LayoutToken);
        }

        /// <summary>
        /// 切换视图
        /// </summary>
        /// <param name="value"></param>
        private void Navigation(string value)
        {
            if (!_视图.Equals(value))
            {
                if ("列表".Equals(value))
                {
                    if (nameof(View.列表视图).Equals(Nav.CurrentPageKey))
                    {
                        return;
                    }
                    Nav.NavigateTo(nameof(View.列表视图));
                }
                else if ("表格".Equals(value))
                {
                    if (nameof(View.表格视图).Equals(Nav.CurrentPageKey))
                    {
                        return;
                    }
                    Nav.NavigateTo(nameof(View.表格视图));
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 外部更改视图，已废弃
        /// </summary>
        /// <param name="value"></param>
        public void Set视图(string value)
        {
            Set(ref _视图, value);
        }
    }
}
