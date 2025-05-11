using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SharedClass.Classes
{
    public class ProductCategory
    {
        [Key]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Products> Products { get; set; } = new List<Products>();
    }

}
