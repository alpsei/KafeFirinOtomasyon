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
    public class FeedBacks
    {
        [Key]
        public int FeedBackID { get; set; }

        public int CustomerID { get; set; }

        public string Content { get; set; }
        public DateTime FBDate { get; set; } = DateTime.Now;
        public bool ReadReceipt { get; set; } = false;

        [ForeignKey(nameof(CustomerID))]
        [InverseProperty("CustomerFB")]
        [JsonIgnore]
        public virtual Users Customer { get; set; }
    }

}
