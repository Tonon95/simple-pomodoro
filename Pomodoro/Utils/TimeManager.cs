using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pomodoro.Utils
{
    public static class TimeManager
    {
        public static int WorkBaseTime;
        public static int LongRestBaseTime;
        public static int ShortRestBaseTime;



        /// <summary>
        /// Method called by App.xaml.cs to initiate the values of base times for the timer in the HomePage.
        /// </summary>
        public static async Task InitTimerValuesAsync()
        {
            await LoadTimePreferences();
        }


        public static async Task SaveTimePreferences()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync("settings", CreationCollisionOption.ReplaceExisting);
            StorageFile openFile = await storageFolder.GetFileAsync("settings");


            string fileContent = WorkBaseTime + "\n"
                                 + ShortRestBaseTime + "\n"
                                 + LongRestBaseTime + "\n";

            await FileIO.WriteTextAsync(openFile, fileContent);
        }



        private static async Task LoadTimePreferences()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile settingsFile = await storageFolder.GetFileAsync("settings");

            string content = await FileIO.ReadTextAsync(settingsFile);
            string[] lines = content.Split('\n');

            // The info is saved in this order on the settings file, one in each line
            WorkBaseTime = int.Parse(lines[0]);
            ShortRestBaseTime = int.Parse(lines[1]);
            LongRestBaseTime = int.Parse(lines[2]);
        }
    }
}
