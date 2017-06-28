namespace X射线荧光分析查询软件.View
{
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for 信息页.xaml
    /// </summary>
    public partial class 列表视图 : Page
    {
        public static readonly Guid LayoutToken = Guid.NewGuid();

        public 列表视图()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, LayoutToken, OnMsg);
            Unloaded += (s, e) => Messenger.Default.Unregister<bool>(this, LayoutToken);
        }

        /// <summary>
        /// 根据通知消息更改显示内容
        /// </summary>
        /// <param name="parameter"></param>
        private void OnMsg(bool parameter)
        {
            if (parameter)
            {
                NoResult.Visibility = Visibility.Collapsed;
                ResultViewer.Visibility = Visibility.Visible;
            }
            else
            {
                ResultViewer.Visibility = Visibility.Collapsed;
                NoResult.Visibility = Visibility.Visible;
            }
        }
    }
}
