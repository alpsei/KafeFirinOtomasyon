using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedClass.Classes
{
    public class Favorites
    {
        [Key]
        public int FavID { get; set; }

        public int CustomerID { get; set; }
        public int ProductID { get; set; }

        [ForeignKey(nameof(CustomerID))]
        [InverseProperty("CustomerFav")]
        [JsonIgnore]
        public virtual Users Customer { get; set; }

        [ForeignKey(nameof(ProductID))]
        public virtual Products Product { get; set; }
    }

}
