using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedClasses.Classes
{
    public class FeedBacks
    {
        [Key]
        public int FeedBackID { get; set; }
        [ForeignKey("Users")]
        public int CustomerID { get; set; }
        public string Content { get; set; }
        public DateTime FBDate { get; set; } = DateTime.Now;
        public bool ReadReceipt { get; set; } = false;

        [JsonIgnore]
        public virtual Users Customer { get; set; }
    }
}
