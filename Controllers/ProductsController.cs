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
    public class ProductsController : Controller
    {
        private readonly theta_ecommerce_dbContext _context;
        private readonly IWebHostEnvironment _he;

        public ProductsController(theta_ecommerce_dbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                    {
                return RedirectToAction("Login","SystemUsers");
            }

            ViewBag.Name = "Theta Solutions in View";

            HttpContext.Session.SetString("CompanyName", "Theta Solutions");

            HttpContext.Session.SetString("CompanyWebsite", "www.thetasolutions.pk");




            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.Categories = _context.Categories.ToList();

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }







        public int getquantity(int id)
        {
            var P = _context.Products.Find(id);
            int Q = P.Quantity.Value;
            return Q;
        }











        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SellerId,Images,Quantity,CategoryId,Price,ShortDescription,LongDescription,DeliveryDays,DeliveryCharges,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData,SeoData")] Product product,
            IList<IFormFile> PP)
        
        
        
        
        
        
        
        {
            var CommaSperated = "";
            foreach (var img in PP)
            {
                string FinalFilePathVirtual = "/data/staff/pps/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                {
                    img.CopyTo(FS);
                }
                CommaSperated = CommaSperated+ "," + FinalFilePathVirtual;
            }


            if (ModelState.IsValid)
            {
                if(CommaSperated.StartsWith(','))
                {
                    CommaSperated = CommaSperated.Remove(0, 1);
                }
                product.Images = CommaSperated;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SellerId,Images,Quantity,CategoryId,Price,ShortDescription,LongDescription,DeliveryDays,DeliveryCharges,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData,SeoData")] Product product,
            IList<IFormFile> PP)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            var CommaSperated = "";
            foreach (var img in PP)
            {
                string FinalFilePathVirtual = "/data/products/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

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
                    product.Images = CommaSperated;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }


        public string GetCounter(int id)
        { 
            return _context.Products.Find(id).NoOfViews.ToString();
        }

        public void UpdateCounter(int id)
        {
            Product P =  _context.Products.Find(id);
            P.NoOfViews = P.NoOfViews++;

            _context.Update(P);
            _context.SaveChanges();
        }

        public string GetRI(int id, int cid)
        {
            //System.Threading.Thread.Sleep(6000);
            //return "<h1 class='alert alert-danger'>Hello I am coming form GetRI Action in Products Controller.</h1>";

           var ProductsData =  _context.Products.Where(abc => abc.CategoryId.Value == cid).Take(5);

            string FinalString = "";
            foreach(Product P in ProductsData)
            {
                FinalString += " <div class='card m-2' style='width: 18rem;'>        <img src='" + P.Images.Split(',')[0] +"' class='card-img-top'>        <div class='card-body'>           <h5 class='card-title'>"+P.Name+"</h5>        </div>   </div>";
            }

            return FinalString;

            //return " <div class='card m-2' style='width: 18rem;'>\r\n        <img src='/data/catagory/3f845921-fa2e-4769-af1a-4946bfceedd1.jpg' class='card-img-top'>\r\n        <div class='card-body'>\r\n            <h5 class='card-title'>Card title</h5>\r\n        </div>\r\n    </div>";
        }



        public string deleteproduct(int id)
        {
            try
            {
                _context.Products.Remove(_context.Products.Find(id));
                _context.SaveChanges();
            }
            catch
            {
                return "2";
            }
            return "1";
        }




        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'theta_ecommerce_dbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
