using DataBaseAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository
{
    public class CardStudySessionScoreRepository : Repository<CardStudySessionScore>, ICardStudySessionScoreRepository
    {
        private readonly ApplicationDbContext _db;
        public CardStudySessionScoreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(CardStudySessionScore cardStudySessionScore)
        {
            CardStudySessionScore? cardStudySessionScoreFromDB = _db.CardStudySessionScore.FirstOrDefault(u => u.Id == cardStudySessionScore.Id);
            if (cardStudySessionScoreFromDB == null) return;
            cardStudySessionScoreFromDB.StudySessionId = cardStudySessionScore.StudySessionId;
            cardStudySessionScoreFromDB.FlashCardId = cardStudySessionScore.FlashCardId;
            cardStudySessionScoreFromDB.Turn = cardStudySessionScore.Turn;
            cardStudySessionScoreFromDB.Score = cardStudySessionScore.Score;
            _db.SaveChanges();
        }
    }
}
