using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;
using DataBaseAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Web.Pages.MyStacks.FlashCards
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;
        private readonly FlashCardRepository _flashCardRepository;

        public DeleteModel(DataBaseAccess.ApplicationDbContext context, FlashCardRepository flashCardRepository)
        {
            _context = context;
            _flashCardRepository = flashCardRepository;
        }

        [BindProperty]
      public FlashCard FlashCard { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FlashCard == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var flashcard = await _context.FlashCard
                .Include(f => f.Stack)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flashcard == null || flashcard.Stack.UserId != userId)
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
            var flashCard = await _context.FlashCard.FirstOrDefaultAsync(m => m.Id == id);
            if (flashCard == null)
            {
                return NotFound();
            }
            FlashCard = flashCard;
            int stackId = FlashCard.StackId;
            if (id == null || _context.FlashCard == null)
            {
                return NotFound();
            }
            _flashCardRepository.Remove(FlashCard);
            TempData["SuccessMessage"] = "Flashcard deleted successfully";

            return RedirectToPage("../Stack", new { id = stackId });
        }
    }
}
