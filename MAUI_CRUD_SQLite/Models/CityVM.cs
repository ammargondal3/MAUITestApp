using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_CRUD_SQLite.Models
{
    public class CityVM
    {
        [PrimaryKey]
        [AutoIncrement]
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
