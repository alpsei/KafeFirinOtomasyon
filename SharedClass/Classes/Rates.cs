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
    public class Rates
    {
        [Key]
        public int RateID { get; set; }
        [ForeignKey("Users")]
        public int CustomerID { get; set; }
        [ForeignKey("Users")]
        public int StaffID { get; set; }
        public int Rate { get; set; }

        [JsonIgnore]
        public virtual Users Customer { get; set; }
        [JsonIgnore]
        public virtual Users Staff { get; set; }

    }
}
