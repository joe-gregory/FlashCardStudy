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
    public class DeleteModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public DeleteModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public StudySession StudySession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudySession == null)
            {
                return NotFound();
            }

            var studysession = await _context.StudySession.FirstOrDefaultAsync(m => m.Id == id);

            if (studysession == null)
            {
                return NotFound();
            }
            else 
            {
                StudySession = studysession;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.StudySession == null)
            {
                return NotFound();
            }
            var studysession = await _context.StudySession.FindAsync(id);

            if (studysession != null)
            {
                StudySession = studysession;
                _context.StudySession.Remove(StudySession);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
