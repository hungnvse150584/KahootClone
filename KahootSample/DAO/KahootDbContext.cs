using Microsoft.EntityFrameworkCore;
using BOs.Model;
using Microsoft.Extensions.Configuration;

namespace DAO
{
    public class KahootDbContext : DbContext
    {
        public KahootDbContext() : base() { }
        public KahootDbContext(DbContextOptions<KahootDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Cấu hình Role với giá trị mặc định
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue(UserRoles.Player); // Giá trị mặc định là Player (2)

            // GameSession
            modelBuilder.Entity<GameSession>()
                .HasIndex(q => q.Pin)
                .IsUnique();

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

            // Answers
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // GameSessions
            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.Quiz)
                .WithMany(q => q.GameSessions)
                .HasForeignKey(gs => gs.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            // Teams
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Session)
                .WithMany(gs => gs.Teams)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // TeamMembers
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Player)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(tm => tm.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

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

            // Responses
            modelBuilder.Entity<Response>()
                .HasOne(r => r.Player)
                .WithMany(p => p.Responses)
                .HasForeignKey(r => r.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Response>()
                .HasOne(r => r.Question)
                .WithMany(q => q.Responses)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Response>()
                .HasOne(r => r.Answer)
                .WithMany(a => a.Responses)
                .HasForeignKey(r => r.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Scores
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Scores)
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Score>()
                .HasOne(s => s.Session)
                .WithMany(gs => gs.Scores)
                .HasForeignKey(s => s.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed dữ liệu mẫu cho Users (bao gồm Role)
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", Password = "hashedpassword", Email = "admin@example.com", CreatedAt = DateTime.UtcNow, Role = UserRoles.Admin },
                new User { UserId = 2, Username = "teacher1", Password = "hashedpassword", Email = "teacher1@example.com", CreatedAt = DateTime.UtcNow, Role = UserRoles.Host },
                new User { UserId = 3, Username = "student1", Password = "hashedpassword", Email = "student1@example.com", CreatedAt = DateTime.UtcNow, Role = UserRoles.Player }
            );
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
