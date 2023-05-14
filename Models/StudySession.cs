using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class StudySession
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Stack")]
        public int StackId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Turns { get; set; }
        public double? Score { get; set; }
        [DisplayName("Right Answers")]
        public int? RightScores { get; set; }
        [DisplayName("Wrong Answers")]
        public int? WrongScores { get; set; }
        //Reference variables: 
        public Stack? Stack { get; set; }
        public ICollection<CardStudySessionScore>? CardStudySessionScores { get; set; } = new List<CardStudySessionScore>();

    }
}
