using Flashcards.DiegoPetrola.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Flashcards.DiegoPetrola.Context;

public class FlashcardContext : DbContext
{
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<CardStack> CardStacks { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }

    public FlashcardContext() : base() { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        IConfiguration configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false)
          .Build() ?? throw new Exception("The appsettings.json was deleted or moved, the application can not run.");

        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasOne(fc => fc.CardStack)
                  .WithMany(s => s.Flashcards)
                  .HasForeignKey(fc => fc.CardStackId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(fc => fc.Question).IsRequired();
            entity.Property(fc => fc.Answer).IsRequired();
        });

        modelBuilder.Entity<CardStack>(entity =>
        {
            entity.HasIndex(s => s.Name).IsUnique();
            entity.Property(s => s.Name).IsRequired();
            entity.Property(s => s.Description).IsRequired();
        });

        modelBuilder.Entity<StudySession>(entity =>
        {
            entity.HasOne(ss => ss.CardStack)
                  .WithMany()
                  .HasForeignKey(ss => ss.CardStackId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(ss => ss.StartTime).IsRequired();
            entity.Property(ss => ss.EndTime).IsRequired();
            entity.Property(ss => ss.Score).IsRequired();
            entity.Property(ss => ss.TotalQuestions).IsRequired();
        });
    }
}
