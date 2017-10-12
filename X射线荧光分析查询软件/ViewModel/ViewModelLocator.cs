/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:X射线荧光分析查询软件.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

namespace X射线荧光分析查询软件.ViewModel
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    // using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Views;
    using System;
    using IoC;
    using X射线荧光分析查询软件.Model;
    using X射线荧光分析查询软件.Service;
    using X射线荧光分析查询软件.View;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            // ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            ServiceLocator.SetLocatorProvider(() => IoC.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                IoC.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                IoC.Default.Register<IDataService, DataService>();
            }

            // SimpleIoc.Default.Register<MainViewModel>();
            IoC.Default.Register<MainViewModel>();
            IoC.Default.Register<SearchViewModel>();
            IoC.Default.Register<ScreenViewModel>();
            IoC.Default.Register(() => InitNavigationService());
            IoC.Default.Register<IDialogService, DialogService>();
        }

        private static INavigationService InitNavigationService()
        {
            NavigationService nav = new NavigationService();
            nav.Configure(nameof(列表视图), new Uri("pack://application:,,,/View/列表视图.xaml"));
            nav.Configure(nameof(表格视图), new Uri("pack://application:,,,/View/表格视图.xaml"));
            nav.Configure(nameof(详情页), new Uri("pack://application:,,,/View/详情页.xaml"));
            return nav;
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();
        public ScreenViewModel Screen => ServiceLocator.Current.GetInstance<ScreenViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().Cleanup();
            ServiceLocator.Current.GetInstance<SearchViewModel>().Cleanup();
            ServiceLocator.Current.GetInstance<ScreenViewModel>().Cleanup();
        }
    }
}