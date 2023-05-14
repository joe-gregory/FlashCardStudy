﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;

namespace Web.Pages.MyStacks.StudySessions
{
    public class EditModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public EditModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudySession StudySession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudySession == null)
            {
                return NotFound();
            }

            var studysession =  await _context.StudySession.FirstOrDefaultAsync(m => m.Id == id);
            if (studysession == null)
            {
                return NotFound();
            }
            StudySession = studysession;
           ViewData["StackId"] = new SelectList(_context.Stack, "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(StudySession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudySessionExists(StudySession.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StudySessionExists(int id)
        {
          return (_context.StudySession?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
