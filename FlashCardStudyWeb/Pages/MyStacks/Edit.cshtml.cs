using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Security.Claims;

namespace Web.Pages.MyStacks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public EditModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Stack Stack { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Stack == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var stack = await _context.Stack.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (stack == null)
            {
                return NotFound();
            }
            Stack = stack;
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Stack.CreationDate");
            ModelState.Remove("Stack.LastModifiedDate");
            ModelState.Remove("Stack.UserId");
            ModelState.Remove("Stack.User");
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input";
                return Page();
            }

            var stackToUpdate = await _context.Stack.FirstOrDefaultAsync(m => m.Id == Stack.Id);

            if (stackToUpdate == null)
            {
                TempData["ErrorMessage"] = "Stack not found";
                return Page();
            }
            stackToUpdate.Name = Stack.Name;
            stackToUpdate.Description = Stack.Description;
            stackToUpdate.LastModifiedDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StackExists(Stack.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Stack", new { id = Stack.Id });
        }

        private bool StackExists(int id)
        {
            return (_context.Stack?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
