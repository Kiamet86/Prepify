using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Prepify.DAL
{
    [Table("Meals")]
    public class Meal
    {
        [PrimaryKey, AutoIncrement]
        public int meal_id
        {
            get;
            set;
        }

        [MaxLength(200)] [NotNull] // varchar(200)
        public string meal_name
        {
            get;
            set;
        }

        public int number_of_serves
        {
            get;
            set;
        }

        public int difficulty
        {
            get;
            set;
        }

        public int health_rating
        {
            get;
            set;
        }

        public int cost
        {
            get;
            set;
        }

        public string last_eaten
        {
            get;
            set;
        }

        public int times_eaten
        {
            get;
            set;
        }

        public bool is_selected
        {
            get;
            set;
        }

        public string bg_colour
        {
            get;
            set;
        }

        public Meal()
        {

        }

        /// <summary>
        /// Overrides the ToString method to provide the meal_id property as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert.ToString(this.meal_id);
        }
    }
}
