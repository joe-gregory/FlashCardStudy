using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Web.Pages.MyStacks
{
    public class StudySessionModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Stack? Stack { get; set; }
        public StudySessionModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int? stackId, string order = "orderly")
        {
            if (stackId == null)
            {
                return NotFound();
            }
            Stack = await _db.Stack
                .Include(s => s.FlashCards)
                .FirstOrDefaultAsync(m => m.Id == stackId);
            if (Stack == null || Stack.FlashCards == null)
            {
                return NotFound();
            }

            if (order == "randomly")
            {
                var rng = new Random();
                Stack.FlashCards = Stack.FlashCards.OrderBy(f => rng.Next()).ToList();
            }
            else
            {
                Stack.FlashCards = Stack.FlashCards.OrderBy(f => f.Order).ToList();
            }
            return Page();
        }
    }
}
