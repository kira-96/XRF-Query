namespace X射线荧光分析查询软件.View
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows;

    /// <summary>
    /// 关于.xaml 的交互逻辑
    /// </summary>
    public partial class 关于
    {
        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(CloseWindow));
        
        public 关于()
        {
            InitializeComponent();
            DataContext = this;
            GetVersion();
        }

        private void GetVersion()
        {
            Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            AppVersion.Text = $"v{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
        }

        private void CloseWindow()
        {
            Close();
        }

        private void Hyperlink_Click(object s, RoutedEventArgs e)
        {
            System.Windows.Documents.Hyperlink hyperlink = s as System.Windows.Documents.Hyperlink;
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(hyperlink.NavigateUri.AbsoluteUri));
        }
    }
}
