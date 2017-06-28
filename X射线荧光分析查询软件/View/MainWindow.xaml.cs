namespace X射线荧光分析查询软件.View
{
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Practices.ServiceLocation;
    using System;
    using System.Windows;
    using System.Windows.Input;
    using X射线荧光分析查询软件.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Guid LayoutToken = Guid.NewGuid();
        public INavigationService Nav => ServiceLocator.Current.GetInstance<INavigationService>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();

        private RelayCommand _EscapeCommand;

        public RelayCommand EscapeCommand => _EscapeCommand ?? (_EscapeCommand = new RelayCommand(OnEscape));

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // 按键绑定
            InputBindings.Add(new KeyBinding(EscapeCommand, new KeyGesture(Key.Escape)));
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Loaded += (s, e) =>
            {
                Messenger.Default.Register<bool>(this, LayoutToken, OnMsg);
                Nav.NavigateTo(nameof(列表视图));
            };
            // Loaded += (s, e) => Messenger.Default.Register<bool>(this, LayoutToken, OnMsg);
            Unloaded += (s, e) => Messenger.Default.Unregister<bool>(this, LayoutToken);
        }

        /// <summary>
        /// 更改页面布局
        /// </summary>
        /// <param name="parameter"></param>
        private void OnMsg(bool parameter)
        {
            if (parameter)
            {
                // Row1.Height = GridLength.Auto;
                // Row2.Height = new GridLength(1, GridUnitType.Star);
                // NavigationFrame.Visibility = Visibility.Visible;
                SearchPage.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Row1.Height = new GridLength(1, GridUnitType.Star);
                // Row2.Height = GridLength.Auto;
                // NavigationFrame.Visibility = Visibility.Collapsed;
                SearchPage.Visibility = Visibility.Visible;
            }
        }


        /// <summary>
        /// 当按下ESC按键时触发此方法
        /// </summary>
        private void OnEscape()
        {
            if (Nav.CurrentPageKey.Equals(nameof(详情页)))
            {
                Nav.GoBack();
                OnMsg(false);
                // Search.Set视图("列表");
                // return;
            }
        }
    }
}