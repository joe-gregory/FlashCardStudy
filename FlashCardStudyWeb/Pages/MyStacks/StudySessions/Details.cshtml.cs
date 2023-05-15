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

namespace Web.Pages.MyStacks.StudySessions
{
    public class DetailsModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public DetailsModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

      public StudySession StudySession { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudySession == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var studysession = await _context.StudySession.Include(s => s.Stack).FirstOrDefaultAsync(m => m.Id == id);
            if (studysession == null || studysession.Stack.UserId != userId)
            {
                return NotFound();
            }
            else 
            {
                StudySession = studysession;
            }
            return Page();
        }
    }
}
