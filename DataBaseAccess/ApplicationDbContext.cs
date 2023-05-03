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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
        }
        public DbSet<CardStudySessionScore> CardStudySessionScore { get; set; }
        public DbSet<FlashCard> FlashCard { get; set; }
        public DbSet<Stack> Stack { get; set; }
        public DbSet<StudySession> StudySession { get; set; }
        public DbSet<User> User { get; set; }
    }
}
