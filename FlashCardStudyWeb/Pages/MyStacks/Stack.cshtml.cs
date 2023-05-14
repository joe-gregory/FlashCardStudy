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
        public IList<StudySession> StudySessions { get; set; }

        public StackModel(DataBaseAccess.ApplicationDbContext db, FlashCardRepository flashCardRepository)
        {
            _db = db;
            _flashCardRepository = flashCardRepository;
        }
        private async Task LoadStack(int? id)
        {
            Stack = await _db.Stack
                .Include(s => s.FlashCards.OrderBy(f => f.Order))
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IActionResult> OnGet(int? id)
        {
            await LoadStack(id);

            if (Stack == null) { return NotFound(); }
            StudySessions = await _db.StudySession
                .Where(ss => ss.StackId == Stack.Id)
                .OrderBy(ss => ss.StartTime)
                .ToListAsync();

            // Create lists of dates and scores
            List<string> dates = StudySessions.Select(ss => ss.StartTime.ToString("MMMM dd, yyyy")).ToList();
            List<double> scores = StudySessions.Select(ss => ss.Score.HasValue ? ss.Score.Value : 0.0).ToList();

            // Pass these lists as properties
            ViewData["Dates"] = dates;
            ViewData["Scores"] = scores;

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
