using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Linq;
using Xamarin.Forms;

namespace Prepify.DAL
{
    class DatabaseHelper
    {
        public static string chosen_sort;
        public static string fontSettingXDocString = "";

        /// <summary>
        /// Takes a SQL query string and returns a sorted List of Meals.
        /// </summary>
        /// <param name="query_str"></param>
        /// <returns>List of Meal</returns>
        public static List<Meal> QueryToList(string query_str)
        {
            using(SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                conn.CreateTable<Meal>();
                var queried_meals = conn.Query<Meal>(query_str).ToList();
                return queried_meals;
            }
        }

        /// <summary>
        /// Takes in an Android ListView, and refreshes its ItemsSource property
        /// <br/> to make sure the UI is up-to-date after data changes.
        /// </summary>
        /// <param name="listView"></param>
        public static void Refresh(ListView listView)
        {
            // Method checks if a sort option has already been chosen.
            // If it has, that sort query will be re-used.
            if (chosen_sort != null)
            {
                listView.ItemsSource = QueryToList(chosen_sort);
            }

            // If there is no sort option, the list is returned in default order.
            // The default order shows all meals, sorted by the fewest times eaten
            // ascending. It is then further sorted by date,
            // with meals eaten longer ago appearing higher.
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
                {
                    var default_sort = "SELECT * FROM Meals ORDER BY times_eaten ASC, last_eaten ASC";
                    conn.CreateTable<Meal>();
                    var meals = conn.Table<Meal>().ToList();

                    listView.ItemsSource = QueryToList(default_sort);
                }
            }
        }

        /// <summary>
        /// Takes in a Meal object, and uses the library sqlite-net-pcl to convert it
        /// into a new row in the Meals table of the Prepify database.
        /// </summary>
        /// <param name="meal"></param>
        public static void Insert(Meal meal)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                conn.CreateTable<Meal>();
                conn.Insert(meal);

            }
        }

        /// <summary>
        /// Takes in a FontSetting object and uses the library sqlite-net-pcl to convert it
        /// into a new row in the FontSetting table of the Prepify database.
        /// </summary>
        /// <param name="fontSetting"></param>
        public static void Insert(FontSetting fontSetting)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                conn.CreateTable<FontSetting>();
                conn.Insert(fontSetting);

            }
        }

        /// <summary>
        /// Takes in a meal_id string, then deletes that row from the Meals table, 
        /// by passing in that Meal's ID.
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id)
        {
            var delete_query = ("DELETE FROM Meals WHERE meal_id = " + id);
            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                conn.Execute(delete_query);
            }
        }

        /// <summary>
        /// Takes in a meal_id string and creates a timestamp, then updates the
        /// last_eaten property of the Meal whose ID matches. 
        /// Also increases the counter of the times_eaten property by 1.
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateEaten(string id)
        {
            // The date format is simplified to YYYY/MM/DD.
            var current_datetime = DateTime.Now.Date.ToString("yyyy/MM/dd");
            
            // Updates are made to the last_eaten and times_eaten columns.
            var update_eaten_query = ("UPDATE Meals SET last_eaten = '" + current_datetime + 
                "', times_eaten = (times_eaten + 1) WHERE meal_id = " + id);
            
            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                conn.Execute(update_eaten_query);
            }
        }

        /// <summary>
        /// Takes in a meal_id string, and updates the corresponding meal in the
        /// Meals table by toggling the is_selected property between TRUE or FALSE.
        /// Also updates the background colour of the Meal object, which changes how
        /// it appears in the UI, making it easier for the user to see what they have
        /// selected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void ToggleSelection(string id)
        {
            /// A query is created that selects the "is_selected" property of the meal.
            var meal_query = ("SELECT is_selected FROM Meals WHERE meal_id = " + id);
            var update_query_true = ("UPDATE Meals SET is_selected = 1, bg_colour = '#f6b8b8' WHERE meal_id = " + id);
            var update_query_false = ("UPDATE Meals SET is_selected = 0, bg_colour = 'White' WHERE meal_id = " + id);

            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                var selection = conn.Query<Meal>(meal_query).ToList();

                foreach (var meal in selection)
                {
                    if (meal.is_selected == true)
                    {
                        meal.is_selected = false;
                        conn.Execute(update_query_false);
                    }

                    else
                    {
                        meal.is_selected = true;
                        conn.Execute(update_query_true);
                    }
                }            
            }
        }

        

        /// <summary>
        /// Retrieve most recent settings from FontSettings table.
        /// <br/>All the recent changes to font settings are iterated through, to the last one.
        /// <br/>This latest setting is then applied to the static fontsetting variable
        /// in DatabaseHelper.
        /// </summary>
        /// <param name="filePath"></param>
        public static void LoadFontSettingFromDB()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DBFilePath))
            {
                conn.CreateTable<FontSetting>();
                var selection = conn.Table<FontSetting>().ToList();

                foreach (var fontSetting in selection)
                {
                    fontSettingXDocString = fontSetting.xdoc_string; 
                }
            }
        }


    }
}
