using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        // Navigation
        private readonly INavigationService _navigationService;

        // Binding Attributes
        private int _workTimeSelectedIndex;
        private int _longRestSelectedIndex;
        private int _shortRestSelectedIndex;

        public int WorkTimeSelectedIndex {
            get {
                return _workTimeSelectedIndex;
            }
            set {
                Set(nameof(WorkTimeSelectedIndex), ref _workTimeSelectedIndex, value);
                Utils.TimeManager.WorkBaseTime = (_workTimeSelectedIndex * 5 + 5) * 60;
                RaisePropertyChanged("WorkTimeSelectedIndex");
            }
        }
        public int LongRestSelectedIndex {
            get {
                return _longRestSelectedIndex;
            }
            set {
                Set(nameof(LongRestSelectedIndex), ref _longRestSelectedIndex, value);
                Utils.TimeManager.LongRestBaseTime = (_longRestSelectedIndex * 5 + 5) * 60;
                RaisePropertyChanged("LongRestSelectedIndex");
            }
        }
        public int ShortRestSelectedIndex {
            get {
                return _shortRestSelectedIndex;
            }
            set {
                Set(nameof(ShortRestSelectedIndex), ref _shortRestSelectedIndex, value);
                Utils.TimeManager.ShortRestBaseTime = (_shortRestSelectedIndex * 5 + 5) * 60;
                RaisePropertyChanged("ShortRestSelectedIndex");
            }
        }


        // Commands
        public RelayCommand NavigateToMainCommand { get; private set; }
        public RelayCommand<int> ChangeWorkTimeDurationCommand { get; private set; }
        public RelayCommand<int> ChangeLongRestDurationCommand { get; private set; }
        public RelayCommand<int> ChangeShortRestDurationCommand { get; private set; }



        public SettingsViewModel(INavigationService navigationService)
        {
            WorkTimeSelectedIndex = 4;
            LongRestSelectedIndex = 5;
            ShortRestSelectedIndex = 1;

            // Setting Nav Service
            _navigationService = navigationService;

            // Setting Relay Commands
            NavigateToMainCommand = new RelayCommand(NavigateToMain);
            ChangeWorkTimeDurationCommand = new RelayCommand<int>(param => ChangeWorkTimeDuration(param));
            ChangeLongRestDurationCommand = new RelayCommand<int>(param => ChangeLongRestDuration(param));
            ChangeShortRestDurationCommand = new RelayCommand<int>(param => ChangeShortRestDuration(param));
        }


        
        private void NavigateToMain()
        {
            _navigationService.NavigateTo("MainPage");
        }



        private void ChangeWorkTimeDuration(int index)
        {
            WorkTimeSelectedIndex = index;
        }



        private void ChangeLongRestDuration(int index)
        {
            LongRestSelectedIndex = index;
        }



        private void ChangeShortRestDuration(int index)
        {
            ShortRestSelectedIndex = index;
        }
    }
}
