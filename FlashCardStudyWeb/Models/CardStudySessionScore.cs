using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCardStudyWeb.Models
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
        [Range(1,100)]
        public int Score { get; set; }
    }
}
