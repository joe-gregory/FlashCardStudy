using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;

namespace Web.Pages.MyStacks.FlashCards
{
    public class DeleteModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public DeleteModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public FlashCard FlashCard { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FlashCard == null)
            {
                return NotFound();
            }

            var flashcard = await _context.FlashCard.FirstOrDefaultAsync(m => m.Id == id);

            if (flashcard == null)
            {
                return NotFound();
            }
            else 
            {
                FlashCard = flashcard;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.FlashCard == null)
            {
                return NotFound();
            }
            var flashcard = await _context.FlashCard.FindAsync(id);

            if (flashcard != null)
            {
                FlashCard = flashcard;
                _context.FlashCard.Remove(FlashCard);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
