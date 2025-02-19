using Microsoft.AspNetCore.Mvc.Rendering;

namespace AliciasWebDisplay.Models
{
    public class ProductEditViewModel
    {
        public int ProductCode { get; set; }
        public string Product { get; set; }
        public decimal SellingPrice { get; set; }
        public bool Active { get; set; }
        public int? CategoryCode { get; set; } // Nullable to allow no category
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
