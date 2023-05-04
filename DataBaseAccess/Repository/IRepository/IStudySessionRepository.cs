using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Repository.IRepository
{
    public interface IStudySessionRepository
    {
        void Update(StudySession studySession);
    }
}
