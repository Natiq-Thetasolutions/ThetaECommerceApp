using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThetaECommerceApp.Models;

namespace ThetaECommerceApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly theta_ecommerce_dbContext _context;
        private readonly IWebHostEnvironment _he;

        public CategoriesController(theta_ecommerce_dbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Images,Description,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData,SeoData")] Category category,
            IList<IFormFile> PP)
        {
            var CommaSperated = "";
            foreach (var img in PP)
            {
                string FinalFilePathVirtual = "/data/catagory/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                {
                    img.CopyTo(FS);
                }
                CommaSperated = CommaSperated + "," + FinalFilePathVirtual;
            }
            if (ModelState.IsValid)
            {
                if (CommaSperated.StartsWith(','))
                {
                    CommaSperated = CommaSperated.Remove(0, 1);
                }
                category.Images = CommaSperated;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Images,Description,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData,SeoData")] Category category,
            IList<IFormFile> PP)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            var CommaSperated = "";
            foreach (var img in PP)
            {
                string FinalFilePathVirtual = "/data/catagory/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                {
                    img.CopyTo(FS);
                }
                CommaSperated = CommaSperated + "," + FinalFilePathVirtual;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (CommaSperated.StartsWith(','))
                    {
                        CommaSperated = CommaSperated.Remove(0, 1);
                    }
                    category.Images = CommaSperated;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'theta_ecommerce_dbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return _context.Categories.Any(e => e.Id == id);
        }
    }
}
