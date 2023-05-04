using DataBaseAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository
{
    public class StudySessionRepository : Repository<StudySession>, IStudySessionRepository
    {
        private readonly ApplicationDbContext _db;
        public StudySessionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(StudySession studySession)
        {
            StudySession? studySessionFromDB = _db.StudySession.FirstOrDefault(u => u.Id == studySession.Id);
                       if (studySessionFromDB == null) return;
            studySessionFromDB.StackId = studySession.StackId;
            studySessionFromDB.StartTime = studySession.StartTime;
            studySessionFromDB.EndTime = studySession.EndTime;
        }
    }
}
