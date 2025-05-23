using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Helpers
{
    public class CartItemRequest
    {
        public Products Product { get; set; }
        public int Quantity { get; set; }
    }
}
