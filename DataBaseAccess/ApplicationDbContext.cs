using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CardStudySessionScore> CardStudySessionScore { get; set; }
        public DbSet<FlashCard> FlashCard { get; set; }
        public DbSet<Stack> Stack { get; set; }
        public DbSet<StudySession> StudySession { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Stack>() //This is about Stack
                .HasOne(s => s.User) //the entity stack has one user
                .WithMany(u => u.Stacks) //and each user has many stacks
                .HasForeignKey(s => s.UserId) //each stack has a foreign key of user id
                .OnDelete(DeleteBehavior.Cascade); //if a user is deleted, delete all stacks associated with it

            modelBuilder.Entity<StudySession>()
                .HasOne(ss => ss.Stack) //the entity study session has one stack
                .WithMany(s => s.StudySessions) //and each stack has many study sessions
                .HasForeignKey(ss => ss.StackId) //each study session has a foreign key of stack id
                .OnDelete(DeleteBehavior.Cascade); //if a stack is deleted, delete all study sessions associated with it

            modelBuilder.Entity<CardStudySessionScore>()
                .HasOne(csss => csss.StudySession) //the entity CardStudySessionScore has one StudySession
                .WithMany(ss => ss.CardStudySessionScores) //each StudySession has many CardStudySessionScores
                .HasForeignKey(csss => csss.StudySessionId) //each CardStudySessionScore has a foreign key of StudySession id
                .OnDelete(DeleteBehavior.Cascade); //if a StudySession is deleted, delete all CardStudySessionScores associated with it

            modelBuilder.Entity<StudySession>()
                .HasMany(ss => ss.CardStudySessionScores) //the entity StudySession has many CardStudySessionScores
                .WithOne(csss => csss.StudySession) //each CardStudySessionScore has one StudySession
                .OnDelete(DeleteBehavior.Cascade); //if a CardStudySessionScore is deleted, delete the StudySession associated with it

            modelBuilder.Entity<FlashCard>()
                .HasMany(fc => fc.CardStudySessionScores) //the entity FlashCard has many CardStudySessionScores
                .WithOne(csss => csss.FlashCard) //each CardStudySessionScore has one FlashCard 
                .OnDelete(DeleteBehavior.Restrict); //if a CardStudySessionScore is deleted, do not delete the FlashCard associated with it
        }
    }
}
