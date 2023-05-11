using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Stack
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        //Reference variables: 
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public User User { get; set; }
        public ICollection<FlashCard> FlashCards { get; set; }
    }
}
