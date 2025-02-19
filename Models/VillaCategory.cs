using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AliciasWebDisplay.Models
{
    public class VillaCategory
    {
        [Key]
        public int CategoryCode { get; set; }
        public string Category { get; set; } 
        public bool Active { get; set; }
    }
}
