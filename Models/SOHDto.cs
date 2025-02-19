using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace AliciasWebDisplay.Models
{
    public class SOHDto
    {
        [Key]
        public int StockOnHandCode { get; set; }
        public int ProductCode { get; set; }
        public string Product { get; set; } // Product name from ProductsController
        public decimal StockOnHand { get; set; }
        public decimal UnitCost { get; set; }
    }
}
