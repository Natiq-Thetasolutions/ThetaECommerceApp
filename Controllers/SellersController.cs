﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThetaECommerceApp.Models;

namespace ThetaECommerceApp.Controllers
{
    public class SellersController : Controller
    {
        private readonly theta_ecommerce_dbContext _context;
        private readonly IWebHostEnvironment _he;

        public SellersController(theta_ecommerce_dbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Sellers
        public async Task<IActionResult> Index()
        {
            ViewBag.CompanyName = HttpContext.Session.GetString("CompanyName");
              return View(await _context.Sellers.ToListAsync());
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Sellers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,CompanyName,WebsiteUrl,Cnic,City,ShortDescription,LongDescription,Email,Gender,PhoneNumber,Address,Dob,SystemUserId,Type,MaritalStatus,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData,SeoData")] Seller seller,
            IFormFile PP)
        {
            string FinalFilePathVirtual = "/data/seller/" + Guid.NewGuid().ToString() + Path.GetExtension(PP.FileName);

            using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
            {
                PP.CopyTo(FS);
            }
            if (ModelState.IsValid)
            {
                seller.Image = FinalFilePathVirtual;
                _context.Add(seller);
                await _context.SaveChangesAsync();

                //send welcome email to seller
                if (!string.IsNullOrEmpty(seller.Email))
                {
                    MailMessage oEmail = new MailMessage();

                    oEmail.Subject = "Welcome <b>"+seller.Name+"</b> to Theta Solutions";
                    oEmail.Body = "Welcome " + seller.Name + ",<br><br>" +
                        "Thank you for signing up on our system. feel free to contact us if you need any help.<br><br>" +
                        "Regards,<br> <span style='color:green;'>Theta Support Team</span>";
                    oEmail.To.Add(seller.Email);
                    oEmail.CC.Add("info@thetasolutions.pk");
                    oEmail.Bcc.Add("natiqbutt2018@gmail.com");



                    oEmail.From = new MailAddress("students@thetademos.com","Theta Solutions");

                   oEmail.Attachments.Add(new Attachment(_he.WebRootPath+FinalFilePathVirtual));


                    SmtpClient oSMTP = new SmtpClient();
                    oSMTP.Port = 465;
                    oSMTP.Host = "mail.thetademos.com";
                    oSMTP.Credentials = new NetworkCredential("students@thetademos.com", "P@kist@n@@123");

                    oSMTP.EnableSsl = true;
                    try { 
                    oSMTP.Send(oEmail);
                         }
                    catch(Exception ex)
                    {

                    }
                
                
                
                
                
                }






                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }

        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,CompanyName,WebsiteUrl,Cnic,City,ShortDescription,LongDescription,Email,Gender,PhoneNumber,Address,Dob,SystemUserId,Type,MaritalStatus,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,MetaData,SeoData")] Seller seller,
            IFormFile PP)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }
            string FinalFilePathVirtual = "/data/seller/" + Guid.NewGuid().ToString() + Path.GetExtension(PP.FileName);

            using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
            {
                PP.CopyTo(FS);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    seller.Image = FinalFilePathVirtual;
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
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
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sellers == null)
            {
                return Problem("Entity set 'theta_ecommerce_dbContext.Sellers'  is null.");
            }
            var seller = await _context.Sellers.FindAsync(id);
            if (seller != null)
            {
                _context.Sellers.Remove(seller);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
          return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
