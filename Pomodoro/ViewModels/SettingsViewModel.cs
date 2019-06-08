using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Pomodoro.Utils;

namespace Pomodoro.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        // Navigation
        private readonly INavigationService _navigationService;

        // Binding Attributes
        private int _workTimeSelectedIndex;
        private int _shortRestSelectedIndex;
        private int _longRestSelectedIndex;

        public int WorkTimeSelectedIndex {
            get {
                _workTimeSelectedIndex = ((Utils.TimeManager.WorkBaseTime / 60) / 5) - 1;
                return _workTimeSelectedIndex;
            }
            set {
                Set(nameof(WorkTimeSelectedIndex), ref _workTimeSelectedIndex, value);
                TimeManager.WorkBaseTime = GetTimeByIndex(_workTimeSelectedIndex);
                RaisePropertyChanged("WorkTimeSelectedIndex");
            }
        }
        public int ShortRestSelectedIndex {
            get {
                _shortRestSelectedIndex = ((Utils.TimeManager.ShortRestBaseTime / 60) / 5) - 1;
                return _shortRestSelectedIndex;
            }
            set {
                Set(nameof(ShortRestSelectedIndex), ref _shortRestSelectedIndex, value);
                TimeManager.ShortRestBaseTime = GetTimeByIndex(_shortRestSelectedIndex);
                RaisePropertyChanged("ShortRestSelectedIndex");
            }
        }
        public int LongRestSelectedIndex {
            get {
                _longRestSelectedIndex = ((Utils.TimeManager.LongRestBaseTime / 60) / 5) - 1;
                return _longRestSelectedIndex;
            }
            set {
                Set(nameof(LongRestSelectedIndex), ref _longRestSelectedIndex, value);
                TimeManager.LongRestBaseTime = GetTimeByIndex(_longRestSelectedIndex);
                RaisePropertyChanged("LongRestSelectedIndex");
            }
        }



        // Commands
        public RelayCommand NavigateToMainCommand { get; private set; }
        public RelayCommand<int> ChangeWorkTimeDurationCommand { get; private set; }
        public RelayCommand<int> ChangeShortRestDurationCommand { get; private set; }
        public RelayCommand<int> ChangeLongRestDurationCommand { get; private set; }



        public SettingsViewModel(INavigationService navigationService)
        {
            WorkTimeSelectedIndex = GetIndexByTime(TimeManager.WorkBaseTime);
            ShortRestSelectedIndex = GetIndexByTime(TimeManager.ShortRestBaseTime);
            LongRestSelectedIndex = GetIndexByTime(TimeManager.LongRestBaseTime);

            // Setting Nav Service
            _navigationService = navigationService;

            // Setting Relay Commands
            NavigateToMainCommand = new RelayCommand(NavigateToMain);
            ChangeWorkTimeDurationCommand = new RelayCommand<int>(param => ChangeWorkTimeDuration(param));
            ChangeShortRestDurationCommand = new RelayCommand<int>(param => ChangeShortRestDuration(param));
            ChangeLongRestDurationCommand = new RelayCommand<int>(param => ChangeLongRestDuration(param));
        }


        
        private async void NavigateToMain()
        {
            _navigationService.NavigateTo("MainPage");
            await Utils.TimeManager.SaveTimePreferences();
        }



        private void ChangeWorkTimeDuration(int index)
        {
            WorkTimeSelectedIndex = index;
        }



        private void ChangeShortRestDuration(int index)
        {
            ShortRestSelectedIndex = index;
        }



        private void ChangeLongRestDuration(int index)
        {
            LongRestSelectedIndex = index;
        }



        private int GetTimeByIndex(int index)
        {
            return (index * 5 + 5) * 60;
        }


        private int GetIndexByTime(int time)
        {
            return ((time / 60) / 5) - 1;
        }
    }
}
