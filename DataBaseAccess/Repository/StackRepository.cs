using DataBaseAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository
{
    public class StackRepository : Repository<Stack>, IStackRepository
    {
        private readonly ApplicationDbContext _context;
        public StackRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Stack stack)
        {
            Stack? stackFromDB = _context.Stack.FirstOrDefault(u => u.Id == stack.Id);
            if (stackFromDB == null) return;

            stackFromDB.UserId = stack.Id;
            stackFromDB.Name = stack.Name;
            stackFromDB.Description = stack.Description;

        }
    }
}
