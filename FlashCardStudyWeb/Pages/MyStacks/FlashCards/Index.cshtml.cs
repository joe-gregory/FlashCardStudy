using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;

namespace Web.Pages.MyStacks.FlashCards
{
    public class IndexModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public IndexModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<FlashCard> FlashCard { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.FlashCard != null)
            {
                FlashCard = await _context.FlashCard
                .Include(f => f.Stack).ToListAsync();
            }
        }
    }
}
