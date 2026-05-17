using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStepsServer.Data.Models;

namespace SmartStepsServer.Data;

public class SmartStepsDbContext : DbContext
{
    public SmartStepsDbContext(DbContextOptions<SmartStepsDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Island> Islands { get; set; } = null!;
    public DbSet<Situation> Situations { get; set; } = null!;
    public DbSet<SituationStep> SituationSteps { get; set; } = null!;
    public DbSet<Flashcard> Flashcards { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<SituationSkill> SituationSkills { get; set; } = null!;
    public DbSet<UserProgress> UserProgresses { get; set; } = null!;
    public DbSet<UserAnswer> UserAnswers { get; set; } = null!;
    public DbSet<ParentReviewQuestion> ParentReviewQuestions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", table =>
            {
                table.HasCheckConstraint("CK_Users_Role", "[Role] IN ('Child', 'Parent', 'Admin', 'ContentCreator')");
            });

            entity.HasKey(e => e.UserId);
            entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Password).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
            ConfigureAuditColumns(entity);

            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure Island
        modelBuilder.Entity<Island>(entity =>
        {
            entity.ToTable("Island", table =>
            {
                table.HasCheckConstraint("CK_Island_Status", "[Status] IN ('Active', 'Hidden')");
            });

            entity.HasKey(e => e.IslandId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.Status).HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
            ConfigureAuditColumns(entity);

            entity.HasMany(e => e.Situations)
                .WithOne(e => e.Island)
                .HasForeignKey(e => e.IslandId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure Situation
        modelBuilder.Entity<Situation>(entity =>
        {
            entity.ToTable("Situation", table =>
            {
                table.HasCheckConstraint(
                    "CK_Situation_Status",
                    "[Status] IN ('Draft', 'Pending', 'Approved', 'Rejected', 'Published', 'Hidden')");
            });

            entity.HasKey(e => e.SituationId);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Status).HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
            ConfigureAuditColumns(entity);

            entity.HasMany(e => e.SituationSteps)
                .WithOne(e => e.Situation)
                .HasForeignKey(e => e.SituationId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(e => e.Flashcards)
                .WithOne(e => e.Situation)
                .HasForeignKey(e => e.SituationId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure SituationStep
        modelBuilder.Entity<SituationStep>(entity =>
        {
            entity.ToTable("SituationStep", table =>
            {
                table.HasCheckConstraint("CK_SituationStep_StepType", "[StepType] IN ('Intro', 'Story', 'Flashcard', 'Result')");
            });

            entity.HasKey(e => e.StepId);
            entity.Property(e => e.StepType).HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
            entity.Property(e => e.MediaUrl).HasColumnType("varchar(500)").HasMaxLength(500);
            ConfigureAuditColumns(entity);
        });

        // Configure Flashcard
        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.ToTable("Flashcard", table =>
            {
                table.HasCheckConstraint("CK_Flashcard_CorrectAnswer", "[CorrectAnswer] IN ('A', 'B')");
            });

            entity.HasKey(e => e.FlashcardId);
            entity.Property(e => e.Question).IsRequired();
            entity.Property(e => e.OptionA).HasMaxLength(500).IsRequired();
            entity.Property(e => e.OptionB).HasMaxLength(500).IsRequired();
            entity.Property(e => e.CorrectAnswer).HasColumnType("char(1)").HasMaxLength(1).IsFixedLength().IsRequired();
            ConfigureAuditColumns(entity);

            entity.HasMany(e => e.UserAnswers)
                .WithOne(e => e.Flashcard)
                .HasForeignKey(e => e.FlashcardId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure Skill
        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("Skill");
            entity.HasKey(e => e.SkillId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            ConfigureAuditColumns(entity);
        });

        // Configure SituationSkill (Many-to-Many)
        modelBuilder.Entity<SituationSkill>(entity =>
        {
            entity.ToTable("SituationSkill");
            entity.HasKey(e => new { e.SituationId, e.SkillId });

            entity.HasOne(e => e.Situation)
                .WithMany(e => e.SituationSkills)
                .HasForeignKey(e => e.SituationId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Skill)
                .WithMany(e => e.SituationSkills)
                .HasForeignKey(e => e.SkillId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure UserProgress
        modelBuilder.Entity<UserProgress>(entity =>
        {
            entity.ToTable("UserProgress", table =>
            {
                table.HasCheckConstraint("CK_UserProgress_Status", "[Status] IN ('InProgress', 'Completed')");
            });

            entity.HasKey(e => e.ProgressId);
            entity.Property(e => e.Status).HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
            entity.Property(e => e.LastAccessedAt).HasColumnType("datetime");
            ConfigureAuditColumns(entity);

            entity.HasOne(e => e.User)
                .WithMany(e => e.UserProgresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Island)
                .WithMany(e => e.UserProgresses)
                .HasForeignKey(e => e.IslandId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Situation)
                .WithMany(e => e.UserProgresses)
                .HasForeignKey(e => e.SituationId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.SituationStep)
                .WithMany(e => e.UserProgresses)
                .HasForeignKey(e => e.CurrentStep)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure UserAnswer
        modelBuilder.Entity<UserAnswer>(entity =>
        {
            entity.ToTable("UserAnswer", table =>
            {
                table.HasCheckConstraint("CK_UserAnswer_SelectedAnswer", "[SelectedAnswer] IN ('A', 'B')");
                table.HasCheckConstraint("CK_UserAnswer_AttemptCount", "[AttemptCount] >= 1");
            });

            entity.HasKey(e => e.AnswerId);
            entity.Property(e => e.SelectedAnswer).HasColumnType("char(1)").HasMaxLength(1).IsFixedLength().IsRequired();
            entity.Property(e => e.AnsweredAt).HasColumnType("datetime");
            ConfigureAuditColumns(entity);

            entity.HasOne(e => e.User)
                .WithMany(e => e.UserAnswers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Flashcard)
                .WithMany(e => e.UserAnswers)
                .HasForeignKey(e => e.FlashcardId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure ParentReviewQuestion
        modelBuilder.Entity<ParentReviewQuestion>(entity =>
        {
            entity.ToTable("ParentReviewQuestion");
            entity.HasKey(e => e.QuestionId);
            entity.Property(e => e.QuestionText).IsRequired();
            ConfigureAuditColumns(entity);

            entity.HasOne(e => e.Skill)
                .WithMany(e => e.ParentReviewQuestions)
                .HasForeignKey(e => e.SkillId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Situation)
                .WithMany(e => e.ParentReviewQuestions)
                .HasForeignKey(e => e.SituationId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureAuditColumns<TEntity>(EntityTypeBuilder<TEntity> entity)
        where TEntity : class
    {
        entity.Property<DateTime>(nameof(User.CreatedAt))
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        entity.Property<DateTime?>(nameof(User.UpdatedAt))
            .HasColumnType("datetime");
    }
}
