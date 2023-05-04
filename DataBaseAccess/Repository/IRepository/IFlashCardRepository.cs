using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository.IRepository
{
    public interface IFlashCardRepository
    {
        void Update(FlashCard flashCard);
        void ReOrder(int stackId);
    }
}
