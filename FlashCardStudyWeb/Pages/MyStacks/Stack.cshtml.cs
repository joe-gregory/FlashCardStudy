using DataBaseAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Web.Pages.MyStacks
{
    public class StackModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _db;
        private readonly FlashCardRepository _flashCardRepository;
        public Stack? Stack { get; set; }
        [BindProperty]
        public FlashCard NewFlashCard { get; set; }

        public StackModel(DataBaseAccess.ApplicationDbContext db, FlashCardRepository flashCardRepository)
        {
            _db = db;
            _flashCardRepository = flashCardRepository;
        }
        private async Task LoadStack(int? id)
        {
            var stack = await _db.Stack
                .Include(s => s.FlashCards.OrderBy(f => f.Order))
                .FirstOrDefaultAsync(m => m.Id == id);
            Stack = stack;
        }
        public async Task<IActionResult> OnGet(int? id)
        {
            await LoadStack(id);
            if (Stack == null) { return NotFound(); }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            await LoadStack(id);
            if (Stack == null) { return NotFound(); }
            if (!NewFlashCard.Order.HasValue)
            {
                NewFlashCard.Order = Stack.FlashCards.Any() ? Stack.FlashCards.Max(c => c.Order) + 1 : 1;
            }
            NewFlashCard.StackId = Stack.Id;
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input";
                return Page();
            }
            _flashCardRepository.InsertAndReorder(NewFlashCard);
            TempData["SuccessMessage"] = "New flash card added to stack! ❤️";
            return RedirectToPage(new { id = id });
        }
    }
}
