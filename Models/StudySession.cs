﻿using System.ComponentModel.DataAnnotations;
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
        //Reference variables: 
        public Stack? Stack { get; set; }
        public ICollection<CardStudySessionScore> CardStudySessionScores { get; set; } = new List<CardStudySessionScore>();

    }
}
