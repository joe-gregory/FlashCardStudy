using DataBaseAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Web.Pages.MyStacks
{
    public class ScoreModel : PageModel
    {
        private readonly StudySessionRepository _studySessionRepository;
        public StudySession? StudySession { get; set; }
        public ScoreModel(StudySessionRepository studySessionRepository)
        {
            _studySessionRepository = studySessionRepository;
        }
        public async Task<IActionResult> OnGetAsync(int? studySessionId = null)
        {
            StudySession = _studySessionRepository.GetFirstOrDefault(ss => ss.Id == studySessionId, includeProperties:"Stack,CardStudySessionScores");
            return Page();
        }
    }
}
