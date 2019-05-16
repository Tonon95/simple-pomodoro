using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Utils
{
    public static class TimeManager
    {
        public static int WorkBaseTime { get; set; }
        public static int LongRestBaseTime { get; set; }
        public static int ShortRestBaseTime { get; set; }



        /// <summary>
        /// Method called by App.xaml.cs to initiate the values of base times for the timer in the HomePage.
        /// </summary>
        public static void InitTimerValues()
        {
            WorkBaseTime = 1 * 60;
            LongRestBaseTime = 2 * 60;
            ShortRestBaseTime = 1 * 60;
        }
    }
}
