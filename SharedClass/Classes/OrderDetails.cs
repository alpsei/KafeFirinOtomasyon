using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass.Classes
{
    public class OrderDetails
    {
        [Key]
        public int DetailID { get; set; }
        [ForeignKey("Orders")]
        public int OrderID { get; set; }
        [ForeignKey("Products")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
