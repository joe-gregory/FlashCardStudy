using DataBaseAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            var stack = GetAll(c => c.StackId == flashCard.StackId, orderby: cards => cards.OrderBy(c => c.Order)).ToList();
            FlashCard? flashCardFromDataBase = _db.FlashCard.FirstOrDefault(u => u.Id == flashCard.Id);
            if (flashCardFromDataBase == null) return;
            if (flashCard.Order < flashCardFromDataBase.Order)
            {
                foreach (var card in stack.Where(c => c.Order >= flashCard.Order))
                {
                    card.Order++;
                }
            }
            else if(flashCardFromDataBase.Order < flashCard.Order)
            {
                foreach (var card in stack.Where(c => c.Order > flashCard.Order))
                {
                    card.Order++;
                }
                foreach(var card in stack.Where(c => c.Order > flashCardFromDataBase.Order && c.Order <= flashCard.Order))
                {
                    card.Order--;
                }
            }


            flashCardFromDataBase.StackId = flashCard.StackId;
            flashCardFromDataBase.Front = flashCard.Front;
            flashCardFromDataBase.Back = flashCard.Back;
            flashCardFromDataBase.Order = flashCard.Order;
            _db.SaveChanges();
            Reorder(flashCard.StackId);

        }
        public new void Remove(FlashCard flashCard)
        {
            dbSet.Remove(flashCard);
            _db.SaveChanges();
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
            _db.SaveChanges();
        }

        public void InsertAndReorder(FlashCard flashCard)
        {
            Reorder(flashCard.StackId);
            var flashCardsInStack = GetAll(c => c.StackId == flashCard.StackId, orderby: cards => cards.OrderBy(c => c.Order)).ToList();
            if (flashCard.Order < 1) { flashCard.Order = 1; }
            if (flashCard.Order <= flashCardsInStack.Count)
            {
                foreach (var card in flashCardsInStack.Where(c => c.Order >= flashCard.Order))
                {
                    card.Order++;
                }
            }
            else
            {
                flashCard.Order = flashCardsInStack.Count + 1;
            }
            Add(flashCard);
            _db.SaveChanges();
        }
    }
}
