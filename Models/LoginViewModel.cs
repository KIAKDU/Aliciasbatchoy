using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AliciasWebDisplay.Models
{
    public class LoginViewModel
    {
        public int UserCode { get; set; }
        public string AccessCode { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LapazLoginViewModel
    {
        public int UserCode { get; set; }
        public string AccessCode { get; set; }
        public bool RememberMe { get; set; }
    }
    public class VillaLoginViewModel
    {
        public int UserCode { get; set; }
        public string AccessCode { get; set; }
        public bool RememberMe { get; set; }
    }
}
