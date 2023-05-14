using DataBaseAccess;
using DataBaseAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Models;

namespace Web.Pages.MyStacks
{
    [BindProperties]
    public class StudySessionModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public readonly StackRepository _stackRepository;
        private readonly StudySessionRepository _studySessionRepository;
        public readonly CardStudySessionScoreRepository _cardStudySessionScoreRepository;
        private readonly FlashCardRepository _flashCardRepository;
        public Stack? Stack { get; set; }
        public StudySession? StudySession { get; set; }
        public int Turn { get; set; }
        public FlashCard? CurrentFlashCard { get; set; }
        public StudySessionModel(ApplicationDbContext db, StackRepository stackRepository, StudySessionRepository studySessionRepository, CardStudySessionScoreRepository cardStudySessionRepository, FlashCardRepository flashCardRepository)
        {
            _db = db;
            _stackRepository = stackRepository;
            _studySessionRepository = studySessionRepository;
            _cardStudySessionScoreRepository = cardStudySessionRepository;
            _flashCardRepository = flashCardRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? stackId = null, string order = "orderly", int? studySessionId = null, int? turn = null, int score = 0)
        {
            //If stackId && order, the study session is just beginning,
            //create StudySession and the CardStudySessionScores
            Stack = _stackRepository.GetFirstOrDefault(s => s.Id == stackId, includeProperties: "FlashCards");
            if (Stack == null)
            {
                return NotFound();
            }
            if(Stack.FlashCards == null || Stack.FlashCards.Count == 0)
            {
                TempData["ErrorMessage"] = "Add some flash cards first!";
                return RedirectToPage("/MyStacks/Stack", new { id = Stack.Id });
            }
            if (studySessionId == null)
            {

                if (order == "randomly")
                {
                    var rng = new Random();
                    Stack.FlashCards = Stack.FlashCards.OrderBy(f => rng.Next()).ToList();
                }
                else
                {
                    Stack.FlashCards = Stack.FlashCards.OrderBy(f => f.Order).ToList();
                }
                //hasta aqui ya tengo el stack con las cartas en orden. Now I need to create the study session y 
                //las Cardstudysessionscores individuales. 
                StudySession = new StudySession();
                StudySession.StackId = Stack.Id;
                StudySession.StartTime = DateTime.UtcNow;
                StudySession.Turns = Stack.FlashCards.Count;
                _studySessionRepository.Add(StudySession);
                //Now, create the CardStudySessionScores
                int counter = 0;
                foreach (var flashCard in Stack.FlashCards)
                {
                    if (counter == 0)
                    {
                        CurrentFlashCard = flashCard;
                        Turn = 0;
                    }
                    var cardStudySessionScore = new CardStudySessionScore();
                    cardStudySessionScore.StudySessionId = StudySession.Id;
                    cardStudySessionScore.FlashCardId = flashCard.Id;
                    cardStudySessionScore.Turn = counter;
                    _cardStudySessionScoreRepository.Add(cardStudySessionScore);
                    counter++;
                }
                return Page();
            }
            else if (studySessionId != null && turn != null)
            {
                //First, save the current score in CardStudySessionScore and then Set the new card
                //Retrieve the CardStudySessionScore and update the score
                StudySession = _studySessionRepository.GetFirstOrDefault(ss => ss.Id == studySessionId);
                if (StudySession == null)
                {
                    return NotFound();
                }
                //I don't need the study session. 
                //What I need is the CardStudySessionScore that has StudySessionId == studySessionId && Turn = Turn
                var cardStudySessionScore = _cardStudySessionScoreRepository.GetFirstOrDefault(csss => csss.StudySessionId == studySessionId && csss.Turn == turn);
                cardStudySessionScore.Score = score;
                _cardStudySessionScoreRepository.Update(cardStudySessionScore);

                //Step 2, setting up new session
                //check if this was the last round
                if (turn == StudySession.Turns - 1)
                {
                    //if this was the last turn
                    //Apply scoring logic and redirect to score page
                    //If this is the final round, update score of StudySession
                    //In order to update the Score of StudySession, I need to aggregate all the scores from
                    //the CardStudySessionScore that have StudySession = StudySession. 
                    List<CardStudySessionScore> cardStudySessionScores = _cardStudySessionScoreRepository.GetAll(csss => csss.StudySessionId == StudySession.Id).ToList();
                    int correctAnswers = 0;
                    int wrongAnswers = 0;
                    foreach (var csss in cardStudySessionScores)
                    {
                        if (csss.Score == 1) correctAnswers++;
                        else wrongAnswers++;
                    }
                    StudySession.RightScores = correctAnswers;
                    StudySession.WrongScores = wrongAnswers;
                    StudySession.Score = Math.Round(((double)correctAnswers / (wrongAnswers + correctAnswers)) * 100,2);
                    StudySession.EndTime = DateTime.UtcNow;
                    _studySessionRepository.Update(StudySession);
                    return RedirectToPage("/MyStacks/Score", new { studySessionId = StudySession.Id });
                }
                //if not last round, prepare for next one
                Turn = turn.Value + 1;
                //Get CurrentFlashCard whose Id will be in the CardStudySessionScore that
                //has Turn == Turn and StudySessionId == studySessionId
                var nextCardStudySessionScore = _cardStudySessionScoreRepository.GetFirstOrDefault(csss => csss.StudySessionId == studySessionId && csss.Turn == Turn);
                CurrentFlashCard = _flashCardRepository.GetFirstOrDefault(fc => fc.Id == nextCardStudySessionScore.FlashCardId);
                return Page();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
