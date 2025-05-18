using SharedClass.Classes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Users
{
    [Key]
    public int UserID { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string SecQuestion { get; set; }
    public string SecAnswer { get; set; }
    public int? Salary { get; set; }

    [InverseProperty("Customer")]
    [JsonIgnore]
    public virtual ICollection<Orders> CustomerOrder { get; set; } = new List<Orders>();

    [InverseProperty("Staff")]
    [JsonIgnore]
    public virtual ICollection<Orders> StaffOrder { get; set; } = new List<Orders>();

    [JsonIgnore]
    public virtual ICollection<Rates> CustomerRates { get; set; } = new List<Rates>();

    [JsonIgnore]
    public virtual ICollection<Rates> StaffRates { get; set; } = new List<Rates>();

    [JsonIgnore]
    public virtual ICollection<Favorites> CustomerFav { get; set; } = new List<Favorites>();

    [JsonIgnore]
    public virtual ICollection<FeedBacks> CustomerFB { get; set; } = new List<FeedBacks>();

    [ForeignKey("Roles")]
    public int RoleID { get; set; }

    [JsonIgnore]
    public Roles Roles { get; set; }
}
