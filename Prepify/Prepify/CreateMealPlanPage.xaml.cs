using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Prepify.DAL;


namespace Prepify
{
    public partial class CreateMealPlanPage : ContentPage
    {
        
    public CreateMealPlanPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the page appears, the ListView of meals is sorted by number
        /// of times eaten, then date last eaten.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                var default_sort = "SELECT * FROM Meals ORDER BY times_eaten ASC, last_eaten ASC";
                conn.CreateTable<Meal>();
                var meals = conn.Table<Meal>().ToList();

                mealsListView.ItemsSource = DatabaseHelper.QueryToList(default_sort);
            }
        }

        public void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SettingsPage());
        }

        private void CreateNewMealButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateNewMealPage());
        }

        private void ViewPlanButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewMealPlanPage());
        }

        private void mealsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Keeps track of the index of the currently selected item.
            App.CurrentSelection = e.SelectedItemIndex;
        }

        /// <summary>
        /// Sends the meal_id of the tapped Meal object to DatabaseHelper.
        /// The selection is toggled selected/not selected, and the ListView
        /// is refreshed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mealsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // The meal_id is sent to the DatabaseHelper, which toggles that meal as Selected.
            var selected_meal = mealsListView.SelectedItem.ToString();
            DatabaseHelper.ToggleSelection(selected_meal);

            DatabaseHelper.Refresh(mealsListView);
        }

        /// <summary>
        /// Sends the meal_id of the tapped Meal to DatabaseHelper.
        /// User can confirm deletion, which removes Meal from Meals table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteMealMenuItem_Clicked(object sender, EventArgs e)
        {
            if(mealsListView.SelectedItem != null) { 
                var selected_meal = mealsListView.SelectedItem.ToString();
                string delete_confirmation = await DisplayActionSheet("Delete Meal?", "Cancel", "Delete");
                if(delete_confirmation == "Delete")
                {
                    DatabaseHelper.Delete(selected_meal);
                    DatabaseHelper.Refresh(mealsListView);
                }
            }
            else
            {
                await DisplayAlert("Meal Selection Required", "Please select a meal first.", "OK");
            }
        }

        /// <summary>
        /// Picker selection value changes how the Meal table is sorted.
        /// 0 - Sort by Difficulty
        /// 1 - Sort by Health Rating
        /// 2 - Sort by Cost
        /// This number is sent to a switch statement that modifies what the
        /// current sorting query string should be.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int sorting_choice = picker.SelectedIndex;
            string sorting_query = "";
            
            switch (sorting_choice)
            {
                // Sort by Difficulty
                case 0:
                    sorting_query = "SELECT * FROM Meals ORDER BY difficulty ASC, " +
                        "times_eaten ASC, last_eaten ASC";
                    break;

                // Sort by Health Rating
                case 1:
                    sorting_query = "SELECT * FROM Meals ORDER BY health_rating DESC, " +
                        "times_eaten ASC, last_eaten ASC";
                    break;

                // Sort by Cost
                case 2:
                    sorting_query = "SELECT * FROM Meals ORDER BY cost ASC, " +
                        "times_eaten ASC, last_eaten ASC";
                    break;   
            }

            mealsListView.ItemsSource = DatabaseHelper.QueryToList(sorting_query);
            DatabaseHelper.chosen_sort = sorting_query;
        }
    }
}
