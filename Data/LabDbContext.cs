using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Data;

public class LabDbContext(DbContextOptions<LabDbContext> options) : DbContext(options)
{
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<LabWork> Works => Set<LabWork>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Discipline).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<LabWork>(entity =>
        {
            entity.Property(item => item.Title).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Instructions).IsRequired();
            entity.Property(item => item.Discipline).HasMaxLength(100).IsRequired();
            entity.Property(item => item.RequiredEquipment).HasMaxLength(100).IsRequired();
            entity.Property(item => item.MaxDeviationPercent).HasPrecision(5, 2);
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.Property(item => item.Number).HasMaxLength(30).IsRequired();
            entity.Property(item => item.Equipment).HasColumnType("nvarchar(max)").IsRequired();
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.Property(item => item.DeviationPercent).HasPrecision(5, 2);
            entity.Property(item => item.Conclusion).HasMaxLength(300).IsRequired();
            entity.HasOne<Teacher>().WithMany().HasForeignKey(item => item.TeacherId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<LabWork>().WithMany().HasForeignKey(item => item.LabWorkId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Classroom>().WithMany().HasForeignKey(item => item.ClassroomId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
