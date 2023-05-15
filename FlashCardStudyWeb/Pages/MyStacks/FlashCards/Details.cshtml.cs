using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.MyStacks.FlashCards
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public DetailsModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public FlashCard FlashCard { get; set; } = default!;
        public IList<CardStudySessionScore> CardStudySessionScores { get; set; }
        public double AverageScore { get; set; }

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
            else
            {
                FlashCard = flashcard;
            }

            CardStudySessionScores = await _context.CardStudySessionScore
                .Where(ss => ss.FlashCardId == FlashCard.Id)
                .Include(ss => ss.StudySession)
                .OrderBy(ss => ss.StudySession.StartTime)
                .ToListAsync();

            List<string> dates = CardStudySessionScores.Select(ss => ss.StudySession.StartTime.ToString("MMMM dd, yyyy")).ToList();
            List<int> intScores = CardStudySessionScores.Select(ss => ss.Score * 100).ToList();
            List<double> scores = intScores.Select(i => (double)i).ToList();
            ViewData["Dates"] = dates;
            ViewData["Scores"] = scores;
            double sumScores = 0;
            int amountScores = 0;
            foreach(var score in scores)
            {
                sumScores = sumScores + score;
                amountScores++;
            }
            AverageScore = Math.Round(sumScores / amountScores, 2);
            return Page();
        }
    }
}
