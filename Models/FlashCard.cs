using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class FlashCard
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Stack")]
        public int StackId { get; set; }
        [Required]
        public string Front { get; set; }
        [Required]
        public string Back { get; set; }
        public Stack? Stack { get; set; } //Reference variable
        [Required]
        public int Order { get; set; }
        public ICollection<CardStudySessionScore> CardStudySessionScores { get; set; } = new List<CardStudySessionScore>();
    }
}
