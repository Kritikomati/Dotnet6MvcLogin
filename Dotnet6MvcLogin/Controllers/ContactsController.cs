﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dotnet6MvcLogin.Models.DTO;
using Dotnet6MvcLogin.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Dotnet6MvcLogin.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly DatabaseContext _context;

        public ContactsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("user"))
            {
                return _context.Contact != null ?
                       View(await _context.Contact.Where(x=>x.UserId== Userinfo.userid).ToListAsync()) :
                       Problem("Entity set 'DatabaseContext.Contact'  is null.");
            }
            else
            {
                return _context.Contact != null ?
                       View(await _context.Contact.ToListAsync()) :
                       Problem("Entity set 'DatabaseContext.Contact'  is null.");
            }
           
           
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            List<Itemlist> Statuslist = new List<Itemlist>() {
        new Itemlist { Text = "pending", Value = "pending" },
        new Itemlist { Text = "Approved", Value = "Approved" },
        new Itemlist { Text = "Submitted", Value ="Submitted" }
        };
            ViewData["statuss"] = Statuslist;
            Contact contact=new Contact();
            return View(contact);
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,Name,Address,City,State,Zip,Email,Status,UserId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List < Itemlist > Statuslist = new List<Itemlist>() {
        new Itemlist { Text = "pending", Value = "pending" },
        new Itemlist { Text = "Approved", Value = "Approved" },
        new Itemlist { Text = "Submitted", Value ="Submitted" }
        };
            ViewData["statuss"] = Statuslist;
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Name,Address,City,State,Zip,Email,Status,UserId")] Contact contact)
        {
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();

                   

                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'DatabaseContext.Contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
          return (_context.Contact?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
