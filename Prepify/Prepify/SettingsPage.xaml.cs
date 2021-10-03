using Prepify.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prepify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private double buttonSize;
        private double labelSize;
        private double entrySize;

        public SettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the RadioButton is checked, the Dynamic Resources buttonFontSize,
        /// labelFontSize and entryFontSize are updated to new values. These dynamic
        /// resources are part of the implicit style established in App.xaml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmallButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.Current.Resources["buttonFontSize"] = 20.00;
            App.Current.Resources["labelFontSize"] = 34.00;
            App.Current.Resources["entryFontSize"] = 20.00;

            buttonSize = 20.00;
            labelSize = 34.00;
            entrySize = 20.00;
        }

        private void MediumButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.Current.Resources["buttonFontSize"] = 22.00;
            App.Current.Resources["labelFontSize"] = 38.00;
            App.Current.Resources["entryFontSize"] = 22.00;

            buttonSize = 22.00;
            labelSize = 38.00;
            entrySize = 22.00;
        }

        private void LargeButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.Current.Resources["buttonFontSize"] = 24.00;
            App.Current.Resources["labelFontSize"] = 42.00;
            App.Current.Resources["entryFontSize"] = 22.00;

            buttonSize = 24.00;
            labelSize = 42.00;
            entrySize = 22.00;
        }

        /// <summary>
        /// Saves current font settings to a string with font settings inside an XDocument.
        /// <br/>Creates a new FontSetting object that contains this string.
        /// <br/>The FontSetting object is sent to the Database Helper, and saved in the
        /// FontSetting table of the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSettingsButton_Clicked(object sender, EventArgs e)
        {
            var settings = XMLHelper.CreateXDocumentString(buttonSize, labelSize, entrySize);
            FontSetting fontsetting = new FontSetting()
            {
                xdoc_string = settings
            };

            DatabaseHelper.Insert(fontsetting);

            Navigation.PopModalAsync();
        }
    }
}