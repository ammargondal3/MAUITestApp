using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_CRUD_SQLite.Models
{
    public class CountryVM
    {
        [PrimaryKey]
        [AutoIncrement]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
