﻿namespace X射线荧光分析查询软件.View
{
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Practices.ServiceLocation;
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using X射线荧光分析查询软件.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Guid LayoutToken = Guid.NewGuid();
        public INavigationService Nav => ServiceLocator.Current.GetInstance<INavigationService>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();

        private bool isMaximize = false;
        private Rect restoreRect;

        private RelayCommand _EscapeCommand;

        public RelayCommand EscapeCommand => _EscapeCommand ?? (_EscapeCommand = new RelayCommand(OnEscape));

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
            // Loaded += (s, e) => Messenger.Default.Register<bool>(this, LayoutToken, OnMsg);
            Unloaded += (s, e) => Messenger.Default.Unregister<bool>(this, LayoutToken);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
            base.OnMouseLeftButtonDown(e);
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

        private void Window_SizeChanged(object s, SizeChangedEventArgs e)
        {
            Rect rect = new Rect(e.NewSize);
            RectangleGeometry rg = new RectangleGeometry(rect, 6, 6);
            (s as UIElement).Clip = rg;
        }

        private void 关于_Click(object sender, RoutedEventArgs e)
        {
            new 关于() { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner }.ShowDialog();
        }

        private void 最小化_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void 最大化_Click(object sender, RoutedEventArgs e)
        {
            if (isMaximize)
            {
                Left = restoreRect.X;
                Top = restoreRect.Y;
                Width = restoreRect.Width;
                Height = restoreRect.Height;
                isMaximize = false;
            }
            else
            {
                restoreRect = new Rect(Left, Top, Width, Height);
                Left = SystemParameters.WorkArea.X;
                Top = SystemParameters.WorkArea.Y;
                Width = SystemParameters.WorkArea.Width;
                Height = SystemParameters.WorkArea.Height;
                isMaximize = true;
            }
            最大化.Content = isMaximize ? "\xE92C" : "\xE92D";
            最大化.ToolTip = isMaximize ? "还原" : "最大化";
        }

        private void 关闭_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}