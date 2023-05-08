using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataBaseAccess;
using Models;
using Microsoft.AspNetCore.Identity;

namespace Web.Pages.MyStacks
{
    public class CreateModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(DataBaseAccess.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Stack Stack { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Stack == null || Stack == null)
            {
                return Page();
            }

            Stack.UserId = _userManager.GetUserId(User);

            _context.Stack.Add(Stack);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
