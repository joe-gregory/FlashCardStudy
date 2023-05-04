using DataBaseAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            StudySessions = new StudySessionRepository(_db);
            Stacks = new StackRepository(_db);
            FlashCards = new FlashCardRepository(_db);
            CardStudySessionScores = new CardStudySessionScoreRepository(_db);
        }

        public IStudySessionRepository StudySessions { get; private set; }

        public IStackRepository Stacks { get; private set; }

        public IFlashCardRepository FlashCards { get; private set; }

        public ICardStudySessionScoreRepository CardStudySessionScores { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
