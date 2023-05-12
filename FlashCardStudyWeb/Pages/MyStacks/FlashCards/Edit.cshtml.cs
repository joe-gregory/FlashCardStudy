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

namespace Web.Pages.MyStacks.FlashCards
{
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

            var flashcard = await _context.FlashCard.FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null)
            {
                return NotFound();
            }
            UpdatedFlashCard = flashcard;
            ViewData["StackId"] = new SelectList(_context.Stack, "Id", "Description");
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

            _flashCardRepository.Update(UpdatedFlashCard);
            TempData["SuccessMessage"] = "Flashcard updated successfully";

            return RedirectToPage("../Stack", new { id = UpdatedFlashCard.StackId });
        }
    }
}
