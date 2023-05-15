using DataBaseAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;

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
        public IList<FlashCard> FlashCards { get; set; } = new List<FlashCard>();
        public IList<double> FlashCardsAverageScores { get; set; } = new List<double>();
        public IList<int> FlashCardAmountStudied { get; set; } = new List<int>();

        public StackModel(DataBaseAccess.ApplicationDbContext db, FlashCardRepository flashCardRepository)
        {
            _db = db;
            _flashCardRepository = flashCardRepository;
        }
        private async Task LoadStack(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Stack = await _db.Stack
                .Where(s => s.Id == id && s.UserId == userId)
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

            //Average flashcard score
            foreach(var flashCard in Stack.FlashCards)
            {
                FlashCards.Add(flashCard);
                var cardStudySessions = _db.CardStudySessionScore.Where(csss => csss.FlashCardId == flashCard.Id);
                double score = 0.0;
                int amount = 0;
                foreach(var css in cardStudySessions)
                {
                    if (css.Score != null) score += css.Score;
                    amount++;
                }
                FlashCardAmountStudied.Add(amount);
                double average = Math.Round(amount > 0 ?  ((double)score / amount)*100 : 0, 2);
                FlashCardsAverageScores.Add(average);
            }

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
