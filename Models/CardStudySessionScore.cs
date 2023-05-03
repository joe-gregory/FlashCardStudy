using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CardStudySessionScore
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("StudySession")]
        public int StudySessionId { get; set; }
        [Required]
        [ForeignKey("FlashCard")]
        public int FlashCardId { get; set; }
        [Required]
        [Range(1, 100)]
        public int Score { get; set; }
        public FlashCard FlashCard { get; set; }
        public StudySession StudySession { get; set; }

    }
}
