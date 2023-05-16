using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Web.Pages.MyStacks
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public DeleteModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Stack Stack { get; set; } = default!;

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
            else 
            {
                Stack = stack;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Stack == null)
            {
                return NotFound();
            }
            var stack = await _context.Stack.FindAsync(id);

            if (stack != null)
            {
                Stack = stack;
                _context.Stack.Remove(Stack);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Stack deleted successfully!";

            return RedirectToPage("./Index");
        }
    }
}
