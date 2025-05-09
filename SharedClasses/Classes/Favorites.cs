using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedClasses.Classes
{
    public class Favorites
    {
        [Key]
        public int FavID { get; set; }
        [ForeignKey("Users")]
        public int CustomerID { get; set; }
        [ForeignKey("Products")]
        public int ProductID { get; set; }

        [JsonIgnore]
        public virtual Users Customer { get; set; }
    }
}
