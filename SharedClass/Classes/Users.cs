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
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        [Column("UPassword")]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SecQuestion { get; set; }
        public string SecAnswer { get; set; }

        [JsonIgnore]
        public virtual ICollection<Rates> CustomerRates { get; set; }
        [JsonIgnore]
        public virtual ICollection<Rates> StaffRates { get; set; }
        [JsonIgnore]
        public virtual ICollection<Orders> CustomerOrder { get; set; }
        [JsonIgnore]
        public virtual ICollection<Orders> StaffOrder { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorites> CustomerFav { get; set; }
        [JsonIgnore]
        public virtual ICollection<FeedBacks> CustomerFB { get; set; }


        [ForeignKey("Roles")]
        public int RoleID { get; set; }
        [JsonIgnore]
        public Roles Roles { get; set; } // Navigation property

    }
}
