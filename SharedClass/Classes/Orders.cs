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
        [Column("CustomerID")]
        public int CustomerID { get; set; }
        public int StaffID { get; set; }
        public string OrderStatus { get; set; } = "Onay Bekliyor";
        public string? OrderNote { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public bool DiscountApplied { get; set; } = false;
        public decimal DiscountRate { get; set; } = 0m;

        [ForeignKey(nameof(CustomerID))]
        [JsonIgnore]
        public virtual Users Customer { get; set; }

        [ForeignKey(nameof(StaffID))]
        [JsonIgnore]
        public virtual Users Staff { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } // Siparişe ait ürün detayları
    }

}
