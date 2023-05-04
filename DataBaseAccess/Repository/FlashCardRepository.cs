using DataBaseAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository
{
    public class FlashCardRepository : Repository<FlashCard>, IFlashCardRepository
    {
        private readonly ApplicationDbContext _db;
        public FlashCardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(FlashCard flashCard)
        {
            FlashCard? flashCardFromDataBase = _db.FlashCard.FirstOrDefault(u => u.Id == flashCard.Id);
            if (flashCardFromDataBase == null) return;
            
                flashCardFromDataBase.StackId = flashCard.StackId;
                flashCardFromDataBase.Front = flashCard.Front;
                flashCardFromDataBase.Back = flashCard.Back;
                flashCardFromDataBase.Order = flashCard.Order;

                Reorder(flashCardFromDataBase.StackId);
            
        }
        public new void Remove(FlashCard flashCard)
        {
            dbSet.Remove(flashCard);
            Reorder(flashCard.StackId);

        }
        public void Reorder(int stackId)
        {
            //Get the stack
            List<FlashCard> flashCardStack = GetAll(u => u.StackId == stackId, orderby: cards => cards.OrderBy(c => c.Order)).ToList();
            //Renumber Order in each flashCard
            for (int i = 1; i <= flashCardStack.Count; i++)
            {
                flashCardStack[i - 1].Order = i;
            }
        }
    }
}
