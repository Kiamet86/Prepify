using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prepify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TheMealDBPage : ContentPage
    {
        public TheMealDBPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// A new HttpClient object sends a request message to the MealDB uri, that
        /// returns a string of a recipe name with ingredients and instructions.
        /// The returned string is processed so only a substring is returned that
        /// gives the meal name only. This meal name is then assigned to the 
        /// RandomMealLabel control so it can be viewed by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RandomMealButton_Clicked(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://www.themealdb.com/api/json/v1/1/random.php"),
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var title_start = body.IndexOf("strMeal") + 10;
                var title_end = body.IndexOf("\",\"strDrinkAlternate");
                var title_length = title_end - title_start;
                var meal_title = body.Substring(title_start, title_length);
                RandomMealLabel.Text = meal_title;
                CreateNewMealPage.mealDB_title = meal_title;
            }
        }

        /// <summary>
        /// The RandomMealLabel.Text property is given the value of the static
        /// string mealDB_title. If that string is not null, the settings window
        /// is closed. If it IS null, an alert asks the user to generate a meal first.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMealButton_Clicked(object sender, EventArgs e)
        {
            CreateNewMealPage.mealDB_title = RandomMealLabel.Text;

            if(CreateNewMealPage.mealDB_title != null)
            {
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Generate a Meal", "Click Suggest a Meal first.", "OK");
            }
            
        }
    }
}