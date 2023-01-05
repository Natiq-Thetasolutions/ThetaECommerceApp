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
    public class FeedbacksController : Controller
    {
        private readonly theta_ecommerce_dbContext _context;
        private readonly IWebHostEnvironment _he;

        public FeedbacksController(theta_ecommerce_dbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
              return View(await _context.Feedbacks.ToListAsync());
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,OrderId,Feedback1,Images,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Feedback feedback,
            IList<IFormFile> PP)
        {
            var CommaSperated = "";
            foreach (var img in PP)
            {
                string FinalFilePathVirtual = "/data/feedback/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

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
                feedback.Images = CommaSperated;
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,OrderId,Feedback1,Images,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Feedback feedback,
            IList<IFormFile> PP)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }
            var CommaSperated = "";
            foreach (var img in PP)
            {
                string FinalFilePathVirtual = "/data/feedback/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

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
                    feedback.Images = CommaSperated;
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'theta_ecommerce_dbContext.Feedbacks'  is null.");
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
          return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}
