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

namespace Web.Pages.MyStacks.StudySessions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public IndexModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<StudySession> StudySessions { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_context.StudySession != null && _context.Stack != null)
            {
                StudySessions = await _context.StudySession
                .Include(s => s.Stack)
                .Where(s => s.Stack.UserId == userId)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
            }
        }
    }
}
