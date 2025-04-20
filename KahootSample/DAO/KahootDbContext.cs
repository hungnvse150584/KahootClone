using Microsoft.EntityFrameworkCore;
using BOs.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAO
{
    public class KahootDbContext : DbContext
    {
        public KahootDbContext() : base() { }
        public KahootDbContext(DbContextOptions<KahootDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<QuestionInGame> QuestionsInGame { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<TeamResultInGame> TeamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue(UserRoles.Player);

            // GameSession
            modelBuilder.Entity<GameSession>()
                .HasIndex(gs => gs.Pin)
                .IsUnique();

            modelBuilder.Entity<GameSession>()
                .Property(gs => gs.LoadingInGame)
                .HasDefaultValue(false);

            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.Quiz)
                .WithMany(q => q.GameSessions)
                .HasForeignKey(gs => gs.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.CreatedByUser)
                .WithMany(u => u.Quizzes)
                .HasForeignKey(q => q.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Questions
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(qz => qz.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // QuestionInGame
            modelBuilder.Entity<QuestionInGame>()
                .HasOne(qig => qig.Session)
                .WithMany(gs => gs.QuestionsInGame)
                .HasForeignKey(qig => qig.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionInGame>()
                .HasOne(qig => qig.Question)
                .WithMany(q => q.QuestionsInGame)
                .HasForeignKey(qig => qig.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Players
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Session)
                .WithMany(gs => gs.Players)
                .HasForeignKey(p => p.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.User)
                .WithMany(u => u.Players)
                .HasForeignKey(p => p.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // Teams
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Session)
                .WithMany(gs => gs.Teams)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Responses
            modelBuilder.Entity<Response>()
                .HasOne(r => r.Player)
                .WithMany(p => p.Responses)
                .HasForeignKey(r => r.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Response>()
                .HasOne(r => r.QuestionInGame)
                .WithMany(qig => qig.Responses)
                .HasForeignKey(r => r.QuestionInGameId)
                .OnDelete(DeleteBehavior.Restrict);

            // TeamResultInGame
            modelBuilder.Entity<TeamResultInGame>()
                .HasOne(tr => tr.QuestionInGame)
                .WithMany(qig => qig.TeamResults)
                .HasForeignKey(tr => tr.QuestionInGameId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeamResultInGame>()
                .HasOne(tr => tr.Session)
                .WithMany(gs => gs.TeamResultsInGame)
                .HasForeignKey(tr => tr.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamResultInGame>()
                .HasOne(tr => tr.Team)
                .WithMany(t => t.TeamResults)
                .HasForeignKey(tr => tr.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Kahoot"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            return config["ConnectionStrings:DbConnection"];
        }
    }
}