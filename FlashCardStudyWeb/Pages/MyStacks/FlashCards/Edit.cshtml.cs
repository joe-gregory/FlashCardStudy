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
using DataBaseAccess.Repository;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.MyStacks.FlashCards
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;
        private readonly FlashCardRepository _flashCardRepository;

        public EditModel(DataBaseAccess.ApplicationDbContext context, FlashCardRepository flashCardRepository)
        {
            _context = context;
            _flashCardRepository = flashCardRepository;
        }

        [BindProperty]
        public FlashCard UpdatedFlashCard { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FlashCard == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var flashcard = await _context.FlashCard.Include(f => f.Stack).FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null || flashcard.Stack.UserId != userId)
            {
                return NotFound();
            }
            UpdatedFlashCard = flashcard;
            ViewData["StackId"] = new SelectList(_context.Stack.Where(s => s.UserId == userId), "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input";
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var flashcard = await _context.FlashCard.Include(f => f.Stack).FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null || flashcard.Stack.UserId != userId)
            {
                return NotFound();
            }

            _flashCardRepository.Update(UpdatedFlashCard);
            TempData["SuccessMessage"] = "Flashcard updated successfully";

            return RedirectToPage("../Stack", new { id = UpdatedFlashCard.StackId });
        }
    }
}
