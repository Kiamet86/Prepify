using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Prepify.DAL;

namespace Prepify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewMealPage : ContentPage
    {
        public static string mealDB_title;

        public CreateNewMealPage()
        {
            InitializeComponent();
        }

        private void TheMealDBButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TheMealDBPage());
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SettingsPage());
        }

        private void ServeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ServeSlider.Value = (int)Math.Round(e.NewValue);
        }

        /// <summary>
        /// A new Meal object is created from the controls in this view,
        /// including Name, Number of Serves, etc. By default, is_selected is
        /// set to false, and the last_eaten date is timestamped for the date
        /// the Meal is created. The background colour is set to White to show
        /// that it is unselected in the UI.
        /// The meal is inserted into the Meals table of the Prepify database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMealButton_Clicked(object sender, EventArgs e)
        {
            Meal meal = new Meal()
            {
                meal_name = mealNameEntry.Text,
                number_of_serves = Convert.ToInt32(numberOfServesLabel.Text),
                difficulty = Convert.ToInt32(RadioButtonGroup.GetSelectedValue(difficultyRadioButtonGroup)),
                health_rating = Convert.ToInt32(RadioButtonGroup.GetSelectedValue(healthRatingRadioButtonGroup)),
                cost = Convert.ToInt32(RadioButtonGroup.GetSelectedValue(costRadioButtonGroup)),
                is_selected = false,
                last_eaten = DateTime.Now.Date.ToString("yyyy/MM/dd"),
                bg_colour = "White"
            };

            DatabaseHelper.Insert(meal);
            mealDB_title = null;
            Navigation.PopAsync();
            
        }

        /// <summary>
        /// Whenever the Create New Meal page is navigated to, this method checks
        /// whether the user has generated a Meal name in the MealDB page.
        /// If meal_db is not null, it's value is assigned to the mealNameEntry.Text
        /// property.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(mealDB_title != null)
            {
                mealNameEntry.Text = mealDB_title;
            }
        }
    }
}