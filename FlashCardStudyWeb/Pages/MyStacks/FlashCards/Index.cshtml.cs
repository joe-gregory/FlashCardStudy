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
    public class IndexModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public IndexModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<FlashCard> FlashCards { get; set; } = new List<FlashCard>();
        public async Task OnGetAsync()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stacks = await _context.Stack
                .Where(s => s.UserId == userId)
                .Include(s => s.FlashCards)
                .ToListAsync();

            foreach (var stack in stacks)
            {
                foreach (var flashCard in stack.FlashCards)
                {
                    FlashCards.Add(flashCard);
                }
            }
        }
    }
}
