using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;

namespace Web.Pages.MyStacks.StudySessions
{
    public class IndexModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public IndexModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<StudySession> StudySessions { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.StudySession != null)
            {
                StudySessions = await _context.StudySession
                .Include(s => s.Stack)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
            }
        }
    }
}
