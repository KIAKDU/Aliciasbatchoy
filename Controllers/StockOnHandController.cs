using AliciasWebDisplay.Models;
using AliciasWebDisplay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AliciasWebDisplay.Controllers
{
    [CustomAuthorize]
    [Route("StockOnHand")]
    public class StockOnHandController : Controller
    {
        private readonly JaroSOHDbContext SOHcontext;
        private readonly LapazSOHDbContext LapazSOHcontext;
        private readonly VillaSOHDbContext VillaSOHcontext;

        //Products DBCONTEXT
        private readonly ApplicationDbContext context;
        private readonly LapazApplicationDbContext lapazContext;
        private readonly VillaApplicationDbContext villaContext;

        public StockOnHandController(JaroSOHDbContext SOHcontext, LapazSOHDbContext LapazSOHcontext, 
            VillaSOHDbContext VillaSOHcontext, ApplicationDbContext context, LapazApplicationDbContext lapazContext,
            VillaApplicationDbContext villaContext)
        {
            this.SOHcontext = SOHcontext;
            this.LapazSOHcontext = LapazSOHcontext;
            this.VillaSOHcontext = VillaSOHcontext;
            //Products DBCONTEXT
            this.context = context;
            this.lapazContext = lapazContext;
            this.villaContext = villaContext;
        }
        //Jaro
        [HttpGet("SOHIndex")]
        public IActionResult SOHIndex(string searchString)
        {
            // Fetch stock-on-hand data
            var stockOnHand = SOHcontext.TStockOnHand.ToList();

            // Fetch product data from ProductsController (ApplicationDbContext)
            var products = context.aTProduct.ToList();

            // Join StockOnHand with Product
            var sohDtos = (from soh in stockOnHand
                           join product in products
                           on soh.ProductCode equals product.ProductCode into productGroup
                           from prod in productGroup.DefaultIfEmpty()
                           orderby soh.StockOnHandCode
                           select new SOHDto
                           {
                               StockOnHandCode = soh.StockOnHandCode,
                               ProductCode = soh.ProductCode,
                               Product = prod != null ? prod.Product : "Unknown Product", // Display product name
                               StockOnHand = soh.StockOnHand,
                               UnitCost = soh.UnitCost
                           }).ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sohDtos = sohDtos
                    .Where(s => s.Product.ToLower().Contains(searchString) ||
                                s.ProductCode.ToString().Contains(searchString))
                    .ToList();
            }

            return View(sohDtos);
        }
        //Lapaz
        [HttpGet("LapazSOHIndex")]
        public IActionResult LapazSOHIndex(string searchString)
        {
            var stockOnHand = LapazSOHcontext.TStockOnHand.ToList();
            var products = lapazContext.aTProduct.ToList();
            var sohDtos = (from soh in stockOnHand
                           join product in products
                           on soh.ProductCode equals product.ProductCode into productGroup
                           from prod in productGroup.DefaultIfEmpty()
                           orderby soh.StockOnHandCode
                           select new SOHDto
                           {
                               StockOnHandCode = soh.StockOnHandCode,
                               ProductCode = soh.ProductCode,
                               Product = prod != null ? prod.Product : "Unknown Product", 
                               StockOnHand = soh.StockOnHand,
                               UnitCost = soh.UnitCost
                           }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sohDtos = sohDtos
                    .Where(s => s.Product.ToLower().Contains(searchString) ||
                                s.ProductCode.ToString().Contains(searchString))
                    .ToList();
            }
            return View(sohDtos);
        }
        //Villa
        [HttpGet("VillaSOHIndex")]
        public IActionResult VillaSOHIndex(string searchString)
        {
            var stockOnHand = VillaSOHcontext.TStockOnHand.ToList();
            var products = villaContext.aTProduct.ToList();
            var sohDtos = (from soh in stockOnHand
                           join product in products
                           on soh.ProductCode equals product.ProductCode into productGroup
                           from prod in productGroup.DefaultIfEmpty()
                           orderby soh.StockOnHandCode
                           select new SOHDto
                           {
                               StockOnHandCode = soh.StockOnHandCode,
                               ProductCode = soh.ProductCode,
                               Product = prod != null ? prod.Product : "Unknown Product",
                               StockOnHand = soh.StockOnHand,
                               UnitCost = soh.UnitCost
                           }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                sohDtos = sohDtos
                    .Where(s => s.Product.ToLower().Contains(searchString) ||
                                s.ProductCode.ToString().Contains(searchString))
                    .ToList();
            }
            return View(sohDtos);
        }
    }
}


