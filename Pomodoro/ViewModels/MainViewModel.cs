using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Microsoft.QueryStringDotNET;

namespace Pomodoro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Navigation
        private readonly INavigationService _navigationService;

        // Binding Attributes
        private string _timer;
        private bool _isPointerOver;
        private bool _isTimerRunning;
        private bool _isStayingAfterClick;
        private string _timerButton_Content;
        private string _timerButton_FontSize;
        private string _timerButton_FontFamily;
        
        public string Timer {
            get {
                return _timer;
            }
            set {
                Set(nameof(Timer), ref _timer, value);
                RaisePropertyChanged("Timer");
            }
        }
        public string TimerButton_Content {
            get {
                return _timerButton_Content;
            }
            set {
                Set(nameof(TimerButton_Content), ref _timerButton_Content, value);
                RaisePropertyChanged("TimerButton_Content");
            }
        }
        public string TimerButton_FontSize {
            get {
                return _timerButton_FontSize;
            }
            set {
                Set(nameof(TimerButton_FontSize), ref _timerButton_FontSize, value);
                RaisePropertyChanged("TimerButton_FontSize");
            }
        }
        public string TimerButton_FontFamily {
            get {
                return _timerButton_FontFamily;
            }
            set {
                Set(nameof(TimerButton_FontFamily), ref _timerButton_FontFamily, value);
                RaisePropertyChanged("TimerButton_FontFamily");
            }
        }


        // Commands
        public RelayCommand StartPauseTimerCommand { get; private set; }
        public RelayCommand TimerButton_OnPointerExitedCommand { get; private set; }
        public RelayCommand TimerButton_OnPointerEnteredCommand { get; private set; }
        public RelayCommand NavigateToSettingsCommand { get; private set; }


        // Logical Attributes: Pomodoro States
        public enum PomodoroStates { WORK, SHORT_REST, LONG_REST }
        private List<PomodoroStates> pomodoroWorkflow;
        private int pomodoroCurrentStateIndex;

        // Logical Attributes: Time
        private int baseTime;
        private DispatcherTimer _dispatcherTimer;





        public MainViewModel(INavigationService navigationService)
        {
            // Initializing values
            _isPointerOver = false;
            _isTimerRunning = false;
            _isStayingAfterClick = false;

            SetupTimer();
            ChangeStartTimerButtonContent("\uE102", "90", "Segoe MDL2 Assets");

            // Setting pomodoro workflow
            pomodoroCurrentStateIndex = 0;
            pomodoroWorkflow = new List<PomodoroStates>();
            for (int i = 0; i < 3; i++)
            {
                pomodoroWorkflow.Add(PomodoroStates.WORK);
                pomodoroWorkflow.Add(PomodoroStates.SHORT_REST);
            }
            pomodoroWorkflow.Add(PomodoroStates.LONG_REST);

            _navigationService = navigationService;

            // Constructing RelayCommands
            StartPauseTimerCommand = new RelayCommand(StartPauseTimer);
            NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
            TimerButton_OnPointerExitedCommand = new RelayCommand(TimerButton_OnPointerExited);
            TimerButton_OnPointerEnteredCommand = new RelayCommand(TimerButton_OnPointerEntered);
        }



        private void SetupTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
        }



        private void DispatcherTimer_Tick(object sender, object e)
        {
            baseTime--;

            if (!_isPointerOver || _isStayingAfterClick)
                ChangeStartTimerButtonContent(String.Format("{0:00}:{1:00}", baseTime / 60, baseTime % 60), "55", "Roboto");

            else
                ChangeStartTimerButtonContent("\uE103", "90", "Segoe MDL2 Assets");
            

            if (baseTime == 0)
            {
                _isTimerRunning = false;
                _dispatcherTimer.Stop();
                SendToastNotification();
                ChangeStartTimerButtonContent("\uE102", "90", "Segoe MDL2 Assets");

                pomodoroCurrentStateIndex = (pomodoroCurrentStateIndex + 1 < pomodoroWorkflow.Count) ? (pomodoroCurrentStateIndex + 1) : 0;

            }
        }



        private void TimerButton_OnPointerExited()
        {
            _isPointerOver = false;
            _isStayingAfterClick = false;

            if (_isTimerRunning)
                ChangeStartTimerButtonContent(String.Format("{0:00}:{1:00}", baseTime / 60, baseTime % 60), "55", "Roboto");
            
        }


        private void TimerButton_OnPointerEntered()
        {
            _isPointerOver = true;

            if (_isTimerRunning)
                ChangeStartTimerButtonContent("\uE103", "90", "Segoe MDL2 Assets");

        }



        private void StartPauseTimer()
        {
            _isStayingAfterClick = true;


            switch (pomodoroWorkflow[pomodoroCurrentStateIndex])
            {
                case PomodoroStates.WORK: baseTime = Utils.TimeManager.WorkBaseTime; break;
                case PomodoroStates.LONG_REST: baseTime = Utils.TimeManager.LongRestBaseTime; break;
                case PomodoroStates.SHORT_REST: baseTime = Utils.TimeManager.ShortRestBaseTime; break;
            }

            Debug.WriteLine("CURRENT INDEX: " + pomodoroCurrentStateIndex);
            Debug.WriteLine("WORK BASE TIME: " + Utils.TimeManager.WorkBaseTime);
            Debug.WriteLine("LONG BASE TIME: " + Utils.TimeManager.LongRestBaseTime);
            Debug.WriteLine("SHORT BASE TIME: " + Utils.TimeManager.ShortRestBaseTime);

            ChangeStartTimerButtonContent(String.Format("{0:00}:{1:00}", baseTime / 60, baseTime % 60), "55", "Roboto");


            if (!_dispatcherTimer.IsEnabled)
            {
                _dispatcherTimer.Start();
                _isTimerRunning = true;
            }
            else
            {
                _dispatcherTimer.Stop();
                _isTimerRunning = false;
                ChangeStartTimerButtonContent("\uE102", "90", "Segoe MDL2 Assets");
            }
        }


        
        private void ChangeStartTimerButtonContent(string content, string fontSize, string fontFamily)
        {
            TimerButton_Content = content;
            TimerButton_FontSize = fontSize;
            TimerButton_FontFamily = fontFamily;
        }



        private void SendToastNotification()
        {
            // Toast Notification Content
            string title = "Good Job!";
            string content = "You've finished this round, hohoho";

            ToastVisual toastVisual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title
                        },
                        new AdaptiveText()
                        {
                            Text = content
                        }
                    }
                }
            };

            ToastContent toastContent = new ToastContent()
            {
                Visual = toastVisual,

                Launch = new QueryString().ToString()
            };


            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }



        private void NavigateToSettings()
        {
            _navigationService.NavigateTo("SettingsPage");
        }
    }
}
