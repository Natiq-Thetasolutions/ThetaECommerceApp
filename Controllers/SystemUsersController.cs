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
    public class SystemUsersController : Controller
    {
        private readonly theta_ecommerce_dbContext _context;

        public SystemUsersController(theta_ecommerce_dbContext context)
        {
            _context = context;
        }

        // GET: SystemUsers
        public async Task<IActionResult> Index()
        {
              return View(await _context.SystemUsers.ToListAsync());
        }

        // GET: SystemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SystemUsers == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // GET: SystemUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Type,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SystemUsers == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUsers.FindAsync(id);
            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Type,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemUserExists(systemUser.Id))
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
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SystemUsers == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SystemUsers == null)
            {
                return Problem("Entity set 'theta_ecommerce_dbContext.SystemUsers'  is null.");
            }
            var systemUser = await _context.SystemUsers.FindAsync(id);
            if (systemUser != null)
            {
                _context.SystemUsers.Remove(systemUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemUserExists(int id)
        {
          return _context.SystemUsers.Any(e => e.Id == id);
        }
    }
}
