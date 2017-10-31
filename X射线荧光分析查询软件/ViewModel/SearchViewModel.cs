namespace X射线荧光分析查询软件.ViewModel
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;

    public class SearchViewModel : ViewModelBase
    {
        private readonly INavigationService _nav = ServiceLocator.Current.GetInstance<INavigationService>();

        private string _queryString = "";
        private string _视图 = "列表";
        private int _分类 = 0;

        private RelayCommand _queryCommand;

        /// <summary>
        /// 文本框中的文本
        /// </summary>
        public string QueryString
        {
            get => _queryString;
            set
            {
                Set(ref _queryString, value);
                Query();
            }
        }

        /// <summary>
        /// 查询的分类
        /// </summary>
        public int 分类
        {
            get => _分类;
            set
            {
                Set(ref _分类, value);
                // 2017.07.14 新增
                QueryString = string.Empty;
            }
        }

        /// <summary>
        /// 数据的呈现方式 列表 or 表格
        /// </summary>
        public string 视图
        {
            get => _视图;
            set
            {
                Navigation(value);
                Set(ref _视图, value);
            }
        }

        public RelayCommand QueryCommand => _queryCommand ?? (_queryCommand = new RelayCommand(Query));

        /// <summary>
        /// 发送查询命令和信息
        /// </summary>
        private void Query()
        {
            Messenger.Default.Send(new NotificationMessage<int>(_分类, _queryString), ScreenViewModel.QueryToken);
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
                switch (value)
                {
                    case "列表":
                        if (nameof(View.列表视图).Equals(_nav.CurrentPageKey))
                        {
                            return;
                        }
                        _nav.NavigateTo(nameof(View.列表视图));
                        break;
                    case "表格":
                        if (nameof(View.表格视图).Equals(_nav.CurrentPageKey))
                        {
                            return;
                        }
                        _nav.NavigateTo(nameof(View.表格视图));
                        break;
                    default:
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
