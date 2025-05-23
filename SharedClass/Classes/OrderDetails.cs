﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SharedClass.Classes
{
    public class OrderDetails
    {
        [Key]
        public int DetailID { get; set; }

        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        [ForeignKey(nameof(OrderID))]
        [JsonIgnore]
        public virtual Orders Order { get; set; }

        [ForeignKey(nameof(ProductID))]
        public virtual Products Product { get; set; }
    }

}
