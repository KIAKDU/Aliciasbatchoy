using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AliciasWebDisplay.Models
{
    public class Users
    {
        [Key]
        public int UserCode { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
    }
}
