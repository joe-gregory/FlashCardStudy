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
    }
}
