using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseAccess;
using Models;

namespace Web.Pages.MyStacks
{
    public class IndexModel : PageModel
    {
        private readonly DataBaseAccess.ApplicationDbContext _context;

        public IndexModel(DataBaseAccess.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Stack> Stack { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Stack != null)
            {
                Stack = await _context.Stack
                .Include(s => s.User).ToListAsync();
            }
        }
    }
}
