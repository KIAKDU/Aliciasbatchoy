using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AliciasWebDisplay.Models
{
    public class CategoryDto
    {
        [Key]
        public int CategoryCode { get; set; }
        public string Category { get; set; } 
        public bool Active { get; set; }
    }
}
