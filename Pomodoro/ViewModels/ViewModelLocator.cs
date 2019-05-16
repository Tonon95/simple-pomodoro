using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace Pomodoro.ViewModels
{
    public class ViewModelLocator
    {

        public const string HomePageKey = "MainPage";
        public const string SettingsPageKey = "SettingsPage";


        public MainViewModel Main 
        {
            get {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public SettingsViewModel Settings 
        {
            get {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }



        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Creating navigator
            var nav = new NavigationService();
            nav.Configure(HomePageKey, typeof(Views.MainPage));
            nav.Configure(SettingsPageKey, typeof(Views.SettingsPage));

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
        }
    }
}
