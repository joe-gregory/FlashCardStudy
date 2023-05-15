using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Web.Pages.MyStacks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public IndexModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Stack> Stacks { get; set; } = default!;
        public IList<double> AverageScores { get; set; } = new List<double>();

        public async Task OnGetAsync()
        {
            if (_context.Stack != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Stacks = await _context.Stack
             .Where(s => s.User.Id == userId)
             .Include(s => s.User)
             .ToListAsync();
            }
            foreach (var stack in Stacks)
            {
                //Get all the study sessions associated with stack and average them
                var studySessions = _context.StudySession.Where(ss => ss.StackId == stack.Id);
                double sum = 0;
                int amount = 0;
                foreach (var session in studySessions)
                {
                    if (session.Score != null)
                    {
                        sum += session.Score.Value;
                    }
                    amount++;
                }
                double average = Math.Round(amount > 0 ? sum / amount : 0, 2);
                AverageScores.Add(average);
            }
        }
    }
}
