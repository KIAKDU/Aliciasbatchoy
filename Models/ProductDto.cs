using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AliciasWebDisplay.Models
{
    public class ProductDto
    {
        [Key]
        public int ProductCode { get; set; }
        public string Product { get; set; } = "";            
        public bool Active { get; set; }      
        public decimal SellingPrice { get; set; }
        public int? CategoryCode { get; set; } // Internal usage, e.g., for filtering or display
        public int? SelectedCategoryCode { get; set; } // Used for binding the selected category in the dropdown     
        public string Category { get; set; } // Display category name (optional)
        public List<SelectListItem> Categories { get; set; } = new(); // Dropdown options for categories
    }
}
