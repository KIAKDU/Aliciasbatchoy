using AliciasWebDisplay.Models;
using AliciasWebDisplay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace AliciasWebDisplay.Controllers
{
    [CustomAuthorize]
    public class ProductsController : Controller
        {
            private readonly ApplicationDbContext context;
            private readonly LapazApplicationDbContext lapazContext;
            private readonly VillaApplicationDbContext villaContext;
            private readonly JaroCategoryDbContext _context;
            private readonly LapazCategoryDbContext _lapazContext;
            private readonly VillaCategoryDbContext _villaContext;


        public ProductsController(ApplicationDbContext context, LapazApplicationDbContext lapazContext, 
            VillaApplicationDbContext villaContext, JaroCategoryDbContext _context, LapazCategoryDbContext _lapazContext, VillaCategoryDbContext  _villaContext)
            {
                this.context = context;
                this.lapazContext = lapazContext;
                this.villaContext = villaContext;

                this._context = _context;
                this._lapazContext = _lapazContext;
                this._villaContext = _villaContext;
        }
        public IActionResult Index(string searchString)
        {         
            var products = context.aTProduct.OrderBy(p => p.ProductCode).ToList();       
            var categories = _context.aTCategory.ToList();
            var productDtos = (from product in products
                               join category in categories
                               on product.CategoryCode equals category.CategoryCode into categoryGroup
                               from cat in categoryGroup.DefaultIfEmpty()
                               select new ProductDto
                               {
                                   ProductCode = product.ProductCode,
                                   Product = product.Product,
                                   SellingPrice = product.SellingPrice,
                                   Active = product.Active,
                                   SelectedCategoryCode = product.CategoryCode,
                                   Category = cat != null
                                       ? $"{cat.Category} " //({cat.CategoryCode}) "
                                       : "No Category" 
                               }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                productDtos = productDtos
                    .Where(p => p.Product.ToLower().Contains(searchString) ||
                                p.ProductCode.ToString().ToLower().Contains(searchString) ||
                                p.Category.ToLower().Contains(searchString))
                    .ToList();
            }
            return View(productDtos);
        }     
            
        public IActionResult Create()
        {          
            var categories = _context.aTCategory
                .Where(c => c.Active) 
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(), 
                    Text = $"{c.Category}" 
                })
                .ToList();
            
            var model = new ProductDto
            {
                Categories = categories
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                productDto.Categories = _context.aTCategory.Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = c.Category
                }).ToList();
                return View(productDto);
            }
                var newProduct = new aTProducts
                {
                    Product = productDto.Product,
                    SellingPrice = productDto.SellingPrice,
                    Active = productDto.Active,
                    CategoryCode = productDto.SelectedCategoryCode 
                };
                context.aTProduct.Add(newProduct);
                context.SaveChanges();
            TempData["AlertMessage"] = string.Format("Jaro Product Code {0} has been CREATED.", newProduct.ProductCode);
            return RedirectToAction("Index");                   
        }
      public IActionResult Edit(int productCode)
        {
            var product = context.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (product == null)
            {
                TempData["AlertMessage"] = "Jaro Product not found!";
                return RedirectToAction("Index");
            }

            var categories = _context.aTCategory
               .Where(c => c.Active)
               .Select(c => new SelectListItem
               {
                   Value = c.CategoryCode.ToString(),
                   Text = $"{c.Category}"
               })
               .ToList();

            var productDto = new ProductDto()
            {
                ProductCode = product.ProductCode,
                Product = product.Product,
                SellingPrice = product.SellingPrice,
                Active = product.Active,
                SelectedCategoryCode = product.CategoryCode,
                Categories = categories
            };
            ViewData["ProductCode"] = product.ProductCode;
            // ViewData["CreatedAt"] = product.CreatedAt?.ToString("MM/dd/yyyy") ?? "N/A";
            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int productCode, ProductDto productDto)
        {
            var product = context.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (product == null)
            {
                TempData["AlertMessage"] = "Jaro Product not found!";
                return RedirectToAction("Index", productDto);
            }
            // Repopulate dropdown list in case of validation errors
            productDto.Categories = _context.aTCategory
                .Where(c => c.Active)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = $"{c.Category}"
                })
                .ToList();
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Log model state errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Debug.WriteLine(error.ErrorMessage);
                }
                // Ensure ProductCode is set in the productDto
                productDto.ProductCode = productCode; // Ensure the ProductCode is set               
            }
            // Update product fields
            product.Product = productDto.Product;
            product.SellingPrice = productDto.SellingPrice;
            product.Active = productDto.Active;
            product.CategoryCode = productDto.SelectedCategoryCode;

            // Save changes to the database
            context.SaveChanges();
            TempData["AlertMessage"] = string.Format("Jaro Product Code {0} has been UPDATED.", product.ProductCode);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int productCode)
        {
        var product = context.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
                if (product == null)
                {
                    TempData["AlertMessage"] = "Jaro Product not found!";
                    return RedirectToAction("Index");
                }

                context.aTProduct.Remove(product);
                context.SaveChanges();
                TempData["AlertMessage"] = "Jaro Product deleted successfully!";
                return RedirectToAction("Index");
        }


        //Lapaz

        public IActionResult LapazIndex(string searchString)
        {
            var lapazProducts = lapazContext.aTProduct.OrderBy(p => p.ProductCode).ToList();
            var categories = _lapazContext.aTCategory.ToList();
            var productDtos = (from product in lapazProducts
                               join category in categories
                               on product.CategoryCode equals category.CategoryCode into categoryGroup
                               from cat in categoryGroup.DefaultIfEmpty()
                               select new ProductDto
                               {
                                   ProductCode = product.ProductCode,
                                   Product = product.Product,
                                   SellingPrice = product.SellingPrice,
                                   Active = product.Active,
                                   SelectedCategoryCode = product.CategoryCode,
                                   Category = cat != null
                                       ? $"{cat.Category}"
                                       : "No Category"
                               }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                productDtos = productDtos
                    .Where(p => p.Product.ToLower().Contains(searchString) ||
                                p.ProductCode.ToString().ToLower().Contains(searchString) ||
                                p.Category.ToLower().Contains(searchString))
                    .ToList();
            }
            return View(productDtos);
        }
        public IActionResult LapazCreate()
        {
            var categories = _lapazContext.aTCategory
                .Where(c => c.Active)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = $"{c.Category}"
                })
                .ToList();

            var model = new ProductDto
            {
                Categories = categories
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult LapazCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                productDto.Categories = _lapazContext.aTCategory.Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = c.Category
                }).ToList();
                return View(productDto);
            }
            var newLapazProduct = new LapazProduct
            {
                Product = productDto.Product,
                SellingPrice = productDto.SellingPrice,
                Active = productDto.Active,
                CategoryCode = productDto.SelectedCategoryCode
            };
            lapazContext.aTProduct.Add(newLapazProduct);
            lapazContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Lapaz Product Code {0} has been CREATED.", newLapazProduct.ProductCode);
            return RedirectToAction("LapazIndex");
        }

        public IActionResult LapazEdit(int productCode)
        {
            var product = lapazContext.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (product == null)
            {
                TempData["AlertMessage"] = "Lapaz Product not found!";
                return RedirectToAction("LapazIndex");
            }

            var categories = _lapazContext.aTCategory
               .Where(c => c.Active)
               .Select(c => new SelectListItem
               {
                   Value = c.CategoryCode.ToString(),
                   Text = $"{c.Category}"
               })
               .ToList();

            var productDto = new ProductDto()
            {
                ProductCode = product.ProductCode,
                Product = product.Product,
                SellingPrice = product.SellingPrice,
                Active = product.Active,
                SelectedCategoryCode = product.CategoryCode,
                Categories = categories
            };
            ViewData["ProductCode"] = product.ProductCode;
            // ViewData["CreatedAt"] = product.CreatedAt?.ToString("MM/dd/yyyy") ?? "N/A";
            return View(productDto);
        }
        [HttpPost]
        public IActionResult LapazEdit(int productCode, ProductDto productDto)
        {
            var product = lapazContext.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (product == null)
            {
                TempData["AlertMessage"] = "Lapaz Product not found!";
                return RedirectToAction("LapazIndex", productDto);
            }

            // Repopulate dropdown list in case of validation errors
            productDto.Categories = _lapazContext.aTCategory
                .Where(c => c.Active)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = $"{c.Category}"
                })
                .ToList();

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Log model state errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Debug.WriteLine(error.ErrorMessage);
                }

                // Ensure ProductCode is set in the productDto
                productDto.ProductCode = productCode; // Ensure the ProductCode is set

            }

            // Update product fields
            product.Product = productDto.Product;
            product.SellingPrice = productDto.SellingPrice;
            product.Active = productDto.Active;
            product.CategoryCode = productDto.SelectedCategoryCode;

            // Save changes to the database
            lapazContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Lapaz Product Code {0} has been UPDATED.", product.ProductCode);
            return RedirectToAction("LapazIndex");
        }


        [HttpPost]
              public IActionResult DeleteLapaz(int productCode)
        {
            var lapazProduct = lapazContext.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (lapazProduct == null)
            {
                TempData["AlertMessage"] = "Lapaz product not found!";
                return RedirectToAction("LapazIndex");
            }

            lapazContext.aTProduct.Remove(lapazProduct);
            lapazContext.SaveChanges();
            TempData["AlertMessage"] = "Lapaz product deleted successfully!";
            return RedirectToAction("LapazIndex");
        }


        //Villa

        public IActionResult VillaIndex(string searchString)
        {
            var VillaProducts = villaContext.aTProduct.OrderBy(p => p.ProductCode).ToList();
            var categories = _villaContext.aTCategory.ToList();
            var productDtos = (from product in VillaProducts
                               join category in categories
                               on product.CategoryCode equals category.CategoryCode into categoryGroup
                               from cat in categoryGroup.DefaultIfEmpty()
                               select new ProductDto
                               {
                                   ProductCode = product.ProductCode,
                                   Product = product.Product,
                                   SellingPrice = product.SellingPrice,
                                   Active = product.Active,
                                   SelectedCategoryCode = product.CategoryCode,
                                   Category = cat != null
                                       ? $"{cat.Category}"
                                       : "No Category"
                               }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                productDtos = productDtos
                    .Where(p => p.Product.ToLower().Contains(searchString) ||
                                p.ProductCode.ToString().ToLower().Contains(searchString) ||
                                p.Category.ToLower().Contains(searchString))
                    .ToList();
            }
            return View(productDtos);
        }
        public IActionResult VillaCreate()
        {
            var categories = _villaContext.aTCategory
                 .Where(c => c.Active)
                 .Select(c => new SelectListItem
                 {
                     Value = c.CategoryCode.ToString(),
                     Text = $"{c.Category}"
                 })
                 .ToList();

            var model = new ProductDto
            {
                Categories = categories
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult VillaCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                productDto.Categories = _villaContext.aTCategory.Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = c.Category
                }).ToList();
                return View(productDto);
            }
            var newVillaProduct = new VillaProducts
            {
                Product = productDto.Product,
                SellingPrice = productDto.SellingPrice,
                Active = productDto.Active,
                CategoryCode = productDto.SelectedCategoryCode
            };
            villaContext.aTProduct.Add(newVillaProduct);
            villaContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Villa Product Code {0} has been CREATED.", newVillaProduct.ProductCode);
            return RedirectToAction("VillaIndex");
        }

        public IActionResult VillaEdit(int productCode)
        {
            var product = villaContext.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (product == null)
            {
                TempData["AlertMessage"] = "Villa Product not found!";
                return RedirectToAction("VillaIndex");
            }

            var categories = _villaContext.aTCategory
               .Where(c => c.Active)
               .Select(c => new SelectListItem
               {
                   Value = c.CategoryCode.ToString(),
                   Text = $"{c.Category}"
               })
               .ToList();

            var productDto = new ProductDto()
            {
                ProductCode = product.ProductCode,
                Product = product.Product,
                SellingPrice = product.SellingPrice,
                Active = product.Active,
                SelectedCategoryCode = product.CategoryCode,
                Categories = categories
            };
            ViewData["ProductCode"] = product.ProductCode;
            return View(productDto);
        }

        [HttpPost]
            public IActionResult VillaEdit(int productCode, ProductDto productDto)
            {
            var product = villaContext.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
            if (product == null)
            {
                TempData["AlertMessage"] = "Villa Product not found!";
                return RedirectToAction("VillaIndex", productDto);
            }
            productDto.Categories = _villaContext.aTCategory
                .Where(c => c.Active)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryCode.ToString(),
                    Text = $"{c.Category}"
                })
                .ToList();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Debug.WriteLine(error.ErrorMessage);
                }
                productDto.ProductCode = productCode;         
            }
            product.Product = productDto.Product;
            product.SellingPrice = productDto.SellingPrice;
            product.Active = productDto.Active;
            product.CategoryCode = productDto.SelectedCategoryCode;

            villaContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Villa Product Code {0} has been UPDATED.", product.ProductCode);
            return RedirectToAction("VillaIndex");
        }
        
            [HttpPost]
            public IActionResult DeleteVilla(int productCode)
            {
                var villaProduct = villaContext.aTProduct.FirstOrDefault(p => p.ProductCode == productCode);
                if (villaProduct == null)
                {
                    TempData["AlertMessage"] = "Villa product not found!";
                    return RedirectToAction("VillaIndex");
                }

                villaContext.aTProduct.Remove(villaProduct);
                villaContext.SaveChanges();
                TempData["AlertMessage"] = "Villa product deleted successfully!";
                return RedirectToAction("VillaIndex");
            }
        }
}

