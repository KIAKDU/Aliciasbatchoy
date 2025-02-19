using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AliciasWebDisplay.Models
{
    public class aTProducts
    {
        [Key]
        public int ProductCode { get; set; }
        [MaxLength(100)]
        public string Product { get; set; } = "";
        public int? CategoryCode {  get; set; }
        public bool Active { get; set; } 
        [Precision(16, 2)]
        public decimal SellingPrice { get; set; }
       
        //  public DateTime? CreatedAt { get; set; }

    }
}

