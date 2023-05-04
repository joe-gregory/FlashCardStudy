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
    }
}
