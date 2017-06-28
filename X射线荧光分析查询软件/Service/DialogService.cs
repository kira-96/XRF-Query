namespace X射线荧光分析查询软件.Service
{
    using System;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight.Views;
    using System.Windows;

    public class DialogService : IDialogService
    {
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessage(string message, string title)
        {
            // throw new NotImplementedException();
            MessageBox.Show(message, title);
            return null;
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            // throw new NotImplementedException();
            MessageBox.Show(message, title, MessageBoxButton.OK);
            afterHideCallback?.Invoke();
            return null;
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowMessageBox(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
            return null;
        }
    }
}
