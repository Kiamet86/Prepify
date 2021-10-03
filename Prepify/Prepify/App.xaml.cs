using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using Prepify.DAL;

namespace Prepify
{
    public partial class App : Application
    {
        public static string DBFilePath;
        public static int CurrentSelection;
        public static List<string> CurrentMealPlan = new List<string> { "Empty" };
       
        public App()
        {
            InitializeComponent();
        
            MainPage = new NavigationPage(new CreateMealPlanPage());
        }

        /// <summary>
        /// Opens the app on the main Prepify screen, with a filepath to
        /// the Prepify database.
        /// </summary>
        /// <param name="dbFilePath"></param>
        public App(string dbFilePath)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new CreateMealPlanPage());
            DBFilePath = dbFilePath;
        }

        /// <summary>
        /// When the program starts, the most recent FontSettings are loaded through XML.
        /// <br/>The app checks that the global settings are not empty. If they ARE empty,
        /// <br/>default font sizes are loaded instead.
        /// </summary>
        protected override void OnStart()
        {
            DatabaseHelper.LoadFontSettingFromDB();
            var recentFontSettings = DatabaseHelper.fontSettingXDocString;

            if(recentFontSettings != "")
            {
                var parsed_Xdoc = XMLHelper.ParseXDocString(recentFontSettings);
                var font_choice_array = XMLHelper.RetrieveFontSettings(parsed_Xdoc);
                App.Current.Resources["buttonFontSize"] = font_choice_array[0];
                App.Current.Resources["labelFontSize"] = font_choice_array[1];
                App.Current.Resources["entryFontSize"] = font_choice_array[2];
            }

            else
            {
                App.Current.Resources["buttonFontSize"] = 20.00;
                App.Current.Resources["labelFontSize"] = 34.00;
                App.Current.Resources["entryFontSize"] = 20.00;
            }
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        
    }
}
