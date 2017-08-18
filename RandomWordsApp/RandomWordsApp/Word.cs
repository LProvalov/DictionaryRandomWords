using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace RandomWordsApp
{
    [Table("Words")]
    public class Word
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set;}

        public string Rus { get; set; }
        public string Eng { get; set; }
        public string Description { get; set; }
    }
}
