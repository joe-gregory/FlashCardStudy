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

namespace Web.Pages.MyStacks
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public DetailsModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

      public Stack Stack { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Stack == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var stack = await _context.Stack.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (stack == null)
            {
                return NotFound();
            }
            else 
            {
                Stack = stack;
            }
            return Page();
        }
    }
}
