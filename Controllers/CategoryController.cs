using AliciasWebDisplay.Models;
using AliciasWebDisplay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AliciasWebDisplay.Controllers
{
    [CustomAuthorize]
    public class CategoryController : Controller
    {
        private readonly JaroCategoryDbContext _context;
        private readonly LapazCategoryDbContext _lapazContext;
        private readonly VillaCategoryDbContext _villaContext;

        public CategoryController(JaroCategoryDbContext _context, LapazCategoryDbContext _lapazContext, VillaCategoryDbContext _villaContext)
        {
            this._context = _context;
            this._lapazContext = _lapazContext;
            this._villaContext = _villaContext;
        }
    
        public IActionResult Index(string searchString)
        {
            var categories = _context.aTCategory.OrderBy(c => c.CategoryCode).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                categories = categories.Where(n => n.Category.ToLower().Contains(searchString) ||
                                                   n.CategoryCode.ToString().ToLower().Contains(searchString)).ToList();
            }

            return View(categories);
        }

        public IActionResult LapazIndex(string searchString)
        {
            var lapazCategories = _lapazContext.aTCategory.OrderBy(c => c.CategoryCode).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                lapazCategories = lapazCategories.Where(n => n.Category.ToLower().Contains(searchString) ||
                                                              n.CategoryCode.ToString().ToLower().Contains(searchString)).ToList();
            }

            return View("LapazIndex", lapazCategories);
        }

        public IActionResult VillaIndex(string searchString)
        {
            var villaCategories = _villaContext.aTCategory.OrderBy(c => c.CategoryCode).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                villaCategories = villaCategories.Where(n => n.Category.ToLower().Contains(searchString) ||
                                                              n.CategoryCode.ToString().ToLower().Contains(searchString)).ToList();
            }

            return View("VillaIndex", villaCategories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", categoryDto);
            }

            var category = new aTCategorys()
            {
                CategoryCode = categoryDto.CategoryCode,
                Category = categoryDto.Category,
                Active = categoryDto.Active,
            };

            _context.aTCategory.Add(category);
            _context.SaveChanges();            
            TempData["AlertMessage"] = string.Format("Jaro Category Code {0} has been CREATED.", category.CategoryCode);
            return RedirectToAction("Index");
        }

        public IActionResult LapazCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LapazCreate(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View("LapazCreate", categoryDto);
            }

            var category = new LapazCategory()
            {
                CategoryCode = categoryDto.CategoryCode,
                Category = categoryDto.Category,
                Active = categoryDto.Active,
            };

            _lapazContext.aTCategory.Add(category);
            _lapazContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Lapaz Category Code {0} has been CREATED.", category.CategoryCode);
            return RedirectToAction("LapazIndex");
        }

        public IActionResult VillaCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VillaCreate(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View("VillaCreate", categoryDto);
            }

            var category = new VillaCategory()
            {
                CategoryCode = categoryDto.CategoryCode,
                Category = categoryDto.Category,
                Active = categoryDto.Active,
            };

            _villaContext.aTCategory.Add(category);
            _villaContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Villa Category Code {0} has been CREATED.", category.CategoryCode);
            return RedirectToAction("VillaIndex");
        }

        public IActionResult Edit(int categoryCode)
        {
            var category = _context.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (category == null)
            {
                TempData["AlertMessage"] = "Category not found!";
                return RedirectToAction("Index");
            }

            var categoryDto = new CategoryDto()
            {
                Category = category.Category,
                CategoryCode = category.CategoryCode,
                Active = category.Active,
            };

            ViewData["CategoryCode"] = category.CategoryCode;
            ViewData["Active"] = category.Active;
            return View(categoryDto);
        }

        [HttpPost]
        public IActionResult Edit(int categoryCode, CategoryDto categoryDto)
        {
            var category = _context.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (category == null)
            {
                TempData["AlertMessage"] = "Category not found!";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryCode"] = category.CategoryCode;
                ViewData["Active"] = category.Active;
                return View(categoryDto);
            }

            category.Category = categoryDto.Category;
            category.Active = categoryDto.Active;

            _context.SaveChanges();
            TempData["AlertMessage"] = string.Format("Jaro Category Code {0} has been UPDATED.", category.CategoryCode);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int categoryCode)
        {
            var category = _context.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (category == null)
            {
                TempData["AlertMessage"] = "Category not found!";
                return RedirectToAction("Index");
            }

            _context.aTCategory.Remove(category);
            _context.SaveChanges();
            TempData["AlertMessage"] = string.Format("Jaro Category Code {0} has been DELETED.", category.CategoryCode);
            return RedirectToAction("Index");
        }

     
        public IActionResult LapazEdit(int categoryCode)
        {
            var lapazCategory = _lapazContext.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (lapazCategory == null)
            {
                TempData["AlertMessage"] = "Lapaz category not found!";
                return RedirectToAction("LapazIndex");
            }

            var categoryDto = new CategoryDto()
            {
                Category = lapazCategory.Category,
                CategoryCode = lapazCategory.CategoryCode,
                Active = lapazCategory.Active,
            };

            ViewData["CategoryCode"] = lapazCategory.CategoryCode;
            ViewData["Active"] = lapazCategory.Active;
            return View("LapazEdit", categoryDto);
        }

        [HttpPost]
        public IActionResult LapazEdit(int categoryCode, CategoryDto categoryDto)
        {
            var lapazCategory = _lapazContext.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (lapazCategory == null)
            {
                TempData["AlertMessage"] = "Lapaz category not found!";
                return RedirectToAction("LapazIndex");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryCode"] = lapazCategory.CategoryCode;
                ViewData["Active"] = lapazCategory.Active;
                return View("LapazEdit", categoryDto);
            }

            lapazCategory.Category = categoryDto.Category;
            lapazCategory.Active = categoryDto.Active;

            _lapazContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Lapaz Category Code {0} has been UPDATED.", lapazCategory.CategoryCode);
            return RedirectToAction("LapazIndex");
        }

        public IActionResult VillaEdit(int categoryCode)
        {
            var villaCategory = _villaContext.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (villaCategory == null)
            {
                TempData["AlertMessage"] = "Villa category not found!";
                return RedirectToAction("VillaIndex");
            }

            var categoryDto = new CategoryDto()
            {
                Category = villaCategory.Category,
                CategoryCode = villaCategory.CategoryCode,
                Active = villaCategory.Active,
            };

            ViewData["CategoryCode"] = villaCategory.CategoryCode;
            ViewData["Active"] = villaCategory.Active;
            return View("VillaEdit", categoryDto);
        }

        [HttpPost]
        public IActionResult VillaEdit(int categoryCode, CategoryDto categoryDto)
        {
            var villaCategory = _villaContext.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (villaCategory == null)
            {
                TempData["AlertMessage"] = "Villa category not found!";
                return RedirectToAction("VillaIndex");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryCode"] = villaCategory.CategoryCode;
                ViewData["Active"] = villaCategory.Active;
                return View("VillaEdit", categoryDto);
            }

            villaCategory.Category = categoryDto.Category;
            villaCategory.Active = categoryDto.Active;

            _villaContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Villa Category Code {0} has been UPDATED.", villaCategory.CategoryCode);
            return RedirectToAction("VillaIndex");
        }

        [HttpPost]
        public IActionResult DeleteLapaz(int categoryCode)
        {
            var lapazCategory = _lapazContext.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (lapazCategory == null)
            {
                TempData["AlertMessage"] = "Lapaz category not found!";
                return RedirectToAction("LapazIndex");
            }

            _lapazContext.aTCategory.Remove(lapazCategory);
            _lapazContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("lapaz Category Code {0} has been DELETED.", lapazCategory.CategoryCode);
            return RedirectToAction("LapazIndex");
        }

        [HttpPost]
        public IActionResult DeleteVilla(int categoryCode)
        {
            var villaCategory = _villaContext.aTCategory.FirstOrDefault(c => c.CategoryCode == categoryCode);
            if (villaCategory == null)
            {
                TempData["AlertMessage"] = "Villa category not found!";
                return RedirectToAction("VillaIndex");
            }

            _villaContext.aTCategory.Remove(villaCategory);
            _villaContext.SaveChanges();
            TempData["AlertMessage"] = string.Format("Villa Category Code {0} has been DELETED.", villaCategory.CategoryCode);
            return RedirectToAction("VillaIndex");
        }
    }
}

