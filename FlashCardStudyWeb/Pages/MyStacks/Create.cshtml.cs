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
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.MyStacks
{
    [Authorize]
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
            ModelState.Remove("Stack.CreationDate");
            ModelState.Remove("Stack.LastModifiedDate");
            ModelState.Remove("Stack.UserId");
            ModelState.Remove("Stack.User");

            if (!ModelState.IsValid || _context.Stack == null || Stack == null)
            {
                TempData["ErrorMessage"] = "Invalid input";
                return Page();
            }

            Stack.UserId = _userManager.GetUserId(User);
            Stack.CreationDate = DateTime.UtcNow;
            Stack.LastModifiedDate = DateTime.UtcNow;

            _context.Stack.Add(Stack);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Stack created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
