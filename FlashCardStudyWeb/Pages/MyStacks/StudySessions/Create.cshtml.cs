using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataBaseAccess;
using Models;
using System.Security.Claims;

namespace Web.Pages.MyStacks.StudySessions
{
    public class CreateModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public CreateModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["StackId"] = new SelectList(_context.Stack.Where(s => s.UserId == userId), "Id", "Description");
            return Page();
        }

        [BindProperty]
        public StudySession StudySession { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.StudySession == null || StudySession == null)
            {
                return Page();
            }

            _context.StudySession.Add(StudySession);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
