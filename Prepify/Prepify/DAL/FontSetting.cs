using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Prepify.DAL
{
    [Table("FontSetting")]
    public class FontSetting
    {
        [PrimaryKey, AutoIncrement]
        public int settings_id
        {
            get;
            set;
        }
        public string xdoc_string
        {
            get;
            set;
        }

        public FontSetting()
        {

        }
    }
}
