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
    public class staffsController : Controller
    {
        private readonly theta_ecommerce_dbContext _context;
        private readonly IWebHostEnvironment _he;

        public staffsController(theta_ecommerce_dbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: staffs
        public async Task<IActionResult> Index()
        {
              return View(await _context.staff.ToListAsync());
        }

        // GET: staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.staff == null)
            {
                return NotFound();
            }

            var staff = await _context.staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: staffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Name,Email,PhoneNumber,City,Address,Dob,SystemUserId,Role,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData")] staff staff,
            IFormFile PP)
        {
            string FinalFilePathVirtual = "/data/staff/pps/" + Guid.NewGuid().ToString() + Path.GetExtension(PP.FileName);

            using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
            {
                PP.CopyTo(FS);
            }


            if (ModelState.IsValid)
            {

                staff.Image = FinalFilePathVirtual;
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.staff == null)
            {
                return NotFound();
            }

            var staff = await _context.staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        // POST: staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Email,PhoneNumber,City,Address,Dob,SystemUserId,Role,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData")] staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!staffExists(staff.Id))
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
            return View(staff);
        }

        // GET: staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.staff == null)
            {
                return NotFound();
            }

            var staff = await _context.staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.staff == null)
            {
                return Problem("Entity set 'theta_ecommerce_dbContext.staff'  is null.");
            }
            var staff = await _context.staff.FindAsync(id);
            if (staff != null)
            {
                _context.staff.Remove(staff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool staffExists(int id)
        {
          return _context.staff.Any(e => e.Id == id);
        }
    }
}
