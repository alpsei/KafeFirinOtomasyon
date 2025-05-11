using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services
{
    public class OrderRequest
    {
        public Orders Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
