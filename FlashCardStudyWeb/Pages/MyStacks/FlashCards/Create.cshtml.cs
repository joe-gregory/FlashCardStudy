using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataBaseAccess;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Web.Pages.MyStacks.FlashCards
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public CreateModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["StackId"] = new SelectList(_context.Stack.Where(s => s.UserId == userId), "Id", "Description");
            return Page();
        }

        [BindProperty]
        public FlashCard FlashCard { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.FlashCard == null || FlashCard == null)
            {
                return Page();
            }

            _context.FlashCard.Add(FlashCard);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
