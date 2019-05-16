using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        // Navigation
        private readonly INavigationService _navigationService;
        
        // Commands
        public RelayCommand NavigateToMainCommand { get; private set; }



        public SettingsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateToMainCommand = new RelayCommand(NavigateToMain);
        }


        
        private void NavigateToMain()
        {
            _navigationService.NavigateTo("MainPage");
        }
    }
}
