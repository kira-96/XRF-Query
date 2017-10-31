namespace X射线荧光分析查询软件.Service
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Media;

    public class NavigationService : ViewModelBase, INavigationService
    {
        private readonly Dictionary<string, Uri> _pagesByKey;
        private readonly List<string> _historic;
        private string _currentPageKey;
        public object Parameter { get; private set; }

        public NavigationService()
        {
            _pagesByKey = new Dictionary<string, Uri>();
            _historic = new List<string>();
        }

        public string CurrentPageKey
        {
            get => _currentPageKey;
            private set => Set(ref _currentPageKey, value);
        }

        public void GoBack()
        {
            if (_historic.Count <= 1) return;
            _historic.Remove(_historic.Last());
            NavigateTo(_historic.Last(), "Back");
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, "Next");
        }

        public virtual void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"No such page: {pageKey}", nameof(pageKey));
                }
                if (GetDescendantFromName(Application.Current.MainWindow, "NavigationFrame") is Frame frame)
                {
                    frame.Source = _pagesByKey[pageKey];
                    // frame.Navigate(_PagesByKey[pageKey]);
                }
                Parameter = parameter;
                if (parameter.ToString().Equals("Next"))
                {
                    _historic.Add(pageKey);
                }
                CurrentPageKey = pageKey;
            }
        }

        public void Configure(string key, Uri pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    _pagesByKey[key] = pageType;
                }
                else
                {
                    _pagesByKey.Add(key, pageType);
                }
            }
        }

        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (int i = 0; i < count; i++)
            {
                FrameworkElement frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }
            return null;
        }
    }
}
