using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Web.Pages.MyStacks
{
    public class StackModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _db;
        public Stack? Stack { get; set; }

        public StackModel(DataBaseAccess.ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || _db.Stack == null)
            {
                return NotFound();
            }
            var stack = await _db.Stack.FirstOrDefaultAsync(m => m.Id == id);
            if (stack == null)
            {
                return NotFound();
            }
            Stack = stack;
            return Page();

        }
    }
}
