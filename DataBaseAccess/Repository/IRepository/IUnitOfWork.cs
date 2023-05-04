using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IStudySessionRepository StudySessions { get; }
        IStackRepository Stacks { get; }
        IFlashCardRepository FlashCards { get; }
        ICardStudySessionScoreRepository CardStudySessionScores { get; }
        void Save();
    }
}
