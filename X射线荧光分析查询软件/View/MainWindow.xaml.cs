namespace X射线荧光分析查询软件.View
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly Guid LayoutToken = Guid.NewGuid();
        public INavigationService Nav => ServiceLocator.Current.GetInstance<INavigationService>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();

        private bool _isMaximize = false;
        private Rect _restoreRect;

        private RelayCommand _escapeCommand;

        public RelayCommand EscapeCommand => _escapeCommand ?? (_escapeCommand = new RelayCommand(OnEscape));

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MaxWidth = SystemParameters.WorkArea.Width;
            MaxHeight = SystemParameters.WorkArea.Height;
            // 按键绑定
            InputBindings.Add(new KeyBinding(EscapeCommand, new KeyGesture(Key.Escape)));
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Loaded += (s, e) =>
            {
                Messenger.Default.Register<bool>(this, LayoutToken, OnMsg);
                Nav.NavigateTo(nameof(列表视图));
            };
            Unloaded += (s, e) => Messenger.Default.Unregister<bool>(this, LayoutToken);
        }

        private void OnMouseLeftDown(object s, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (_isMaximize)
                {
                    RestoreWindow(_restoreRect.X, _restoreRect.Y);
                }
                else
                {
                    MaximizeWindow();
                }
                e.Handled = true;
                return;
            }
            if (!_isMaximize && e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 更改页面布局
        /// </summary>
        /// <param name="parameter"></param>
        private void OnMsg(bool parameter)
        {
            SearchPage.Visibility = parameter ? Visibility.Collapsed : Visibility.Visible;
        }


        /// <summary>
        /// 当按下ESC按键时触发此方法
        /// </summary>
        private void OnEscape()
        {
            if (!Nav.CurrentPageKey.Equals(nameof(详情页))) return;
            Nav.GoBack();
            OnMsg(false);
        }

        private void Window_SizeChanged(object s, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeWindow();
                return;
            }
            Rect rect = new Rect(e.NewSize);
            RectangleGeometry rg = new RectangleGeometry(rect, 6, 6);
            ((UIElement) s).Clip = rg;
        }

        private void 关于_Click(object s, RoutedEventArgs e)
        {
            new 关于() { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner }.ShowDialog();
        }

        private void 最小化_Click(object s, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void 最大化_Click(object s, RoutedEventArgs e)
        {
            if (_isMaximize)
            {
                RestoreWindow(_restoreRect.X, _restoreRect.Y);
            }
            else
            {
                MaximizeWindow();
            }
        }

        private void MaximizeWindow()
        {
            _restoreRect = new Rect(Left, Top, Width, Height);
            Left = SystemParameters.WorkArea.X;
            Top = SystemParameters.WorkArea.Y;
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
            _isMaximize = true;
            最大化.Content = "\xF066";
            最大化.ToolTip = "还原";
        }

        private void RestoreWindow(double x, double y)
        {
            Left = x;
            Top = y;
            Width = _restoreRect.Width;
            Height = _restoreRect.Height;
            _isMaximize = false;
            最大化.Content = "\xF065";
            最大化.ToolTip = "最大化";
        }

        private void 关闭_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}