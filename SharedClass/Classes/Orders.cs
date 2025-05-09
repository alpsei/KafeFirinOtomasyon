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
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        [ForeignKey("Users")]
        public int CustomerID { get; set; }
        [ForeignKey("Users")]
        public int StaffID { get; set; }
        public string OrderStatus { get; set; } = "Hazırlanıyor";
        public string OrderNote { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public bool DiscountApplied { get; set; } = false;

        [JsonIgnore]
        public virtual Users Customer { get; set; }
        [JsonIgnore]
        public virtual Users Staff { get; set; }
    }
}
