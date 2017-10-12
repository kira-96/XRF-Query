namespace X射线荧光分析查询软件.View
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows;

    /// <summary>
    /// 关于.xaml 的交互逻辑
    /// </summary>
    public partial class 关于 : Window
    {
        private RelayCommand _CloseCommand;

        public RelayCommand CloseCommand => _CloseCommand ?? (_CloseCommand = new RelayCommand(CloseWindow));
        
        public 关于()
        {
            InitializeComponent();
            this.DataContext = this;
            GetVersion();
        }

        private void GetVersion()
        {
            Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            version.Text = $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void Hyperlink_Click(object s, RoutedEventArgs e)
        {
            System.Windows.Documents.Hyperlink hyperlink = s as System.Windows.Documents.Hyperlink;
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(hyperlink.NavigateUri.AbsoluteUri));
        }
    }
}
