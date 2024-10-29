
using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence;

public class DevFreelaDbContext : DbContext
{
    public DevFreelaDbContext(DbContextOptions<DevFreelaDbContext> options) : base(options)
    {

    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }
    public DbSet<ProjectComment> ProjectComments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<Skill>(e =>
            {
                e.HasKey(s => s.Id);
            });

        builder
            .Entity<UserSkill>(e =>
            {
                e.HasKey(us => us.Id);

                e.HasOne(u => u.Skill)//1 usuario tem 1 habilidade
                    .WithMany(u => u.UserSkills)//1 habilidade tem muitos usuarios
                    .HasForeignKey(s => s.IdSkill)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        builder
            .Entity<ProjectComment>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasOne(p => p.Project)//1 comentario tem 1 projeto
                    .WithMany(p => p.Comments)//1 projeto tem muitos comentarios
                    .HasForeignKey(p => p.IdProject)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(p => p.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        builder
            .Entity<User>(e =>
            {
                e.HasKey(u => u.Id);

                e.HasMany(u => u.Skills) //1 usuario tem muitas habilidades
                    .WithOne(us => us.User) //1 habilidade tem 1 unico usuario
                    .HasForeignKey(us => us.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        builder
            .Entity<Project>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasOne(p => p.Freelancer)//1 projeto tem 1 freelance
                    .WithMany(f => f.FreelanceProjects)//1 freelance tem muitos projeots
                    .HasForeignKey(p => p.IdFreelancer)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.Client)//1 projeto tem 1 cliente
                    .WithMany(c => c.OwnedProjects)//1 cliente tem muitos proprietarios de projeto
                    .HasForeignKey(p => p.IdClient)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        base.OnModelCreating(builder);
    }
}
