using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedClasses
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
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<Users> Users { get; set; }
    }
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
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        [ForeignKey("ProductCategory")]
        public int CategoryID { get; set; }
    }
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Column("CategoryID")]
        [Key]
        public int CategoryID { get; set; }
        [Column("CategoryName")]
        public string CategoryName { get; set; }
    }
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
    public class Favorites
    {
        [Key]
        public int FavID { get; set; }
        [ForeignKey("Users")]
        public int CustomerID { get; set; }
        [ForeignKey("Products")]
        public int ProductID { get; set; }

        [JsonIgnore]
        public virtual Users Customer { get; set; }
    }
}
