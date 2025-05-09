using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass.Classes
{
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Column("CategoryID")]
        [Key]
        public int CategoryID { get; set; }
        [Column("CategoryName")]
        public string CategoryName { get; set; }
    }
}
