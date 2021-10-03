using Prepify.DAL;
using SQLite;
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
    public partial class ViewMealPlanPage : ContentPage
    {
        public ViewMealPlanPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When this page is navigated to, a query is sent to DatabaseHelper,
        /// that selects all meals with an is_selected property of TRUE.
        /// This query is converted to a List of Meal, which then becomes the
        /// ItemsSource for the ListView.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                var meal_query = "SELECT * FROM Meals WHERE is_selected = 1";
                var meals = conn.Query<Meal>(meal_query).ToList();

                ViewMealPlanListView.ItemsSource = meals;
            }
            
        }

        /// <summary>
        /// When an item in the ListView is tapped, the item's meal_ID is
        /// sent to DatabaseHelper, where it goes through the UpdateEaten and
        /// ToggleSelection methods. The list is then refreshed with a query
        /// of all meals that still have is_selected as TRUE.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMealPlanViewCell_Tapped(object sender, EventArgs e)
        {
            var selected_meal = ViewMealPlanListView.SelectedItem.ToString();
            DatabaseHelper.UpdateEaten(selected_meal);
            DatabaseHelper.ToggleSelection(selected_meal);
            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                var meal_query = "SELECT * FROM Meals WHERE is_selected = 1";
                var meals = conn.Query<Meal>(meal_query).ToList();

                ViewMealPlanListView.ItemsSource = meals;
            }
        }
    }
}