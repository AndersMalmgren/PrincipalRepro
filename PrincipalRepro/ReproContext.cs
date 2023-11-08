using Microsoft.EntityFrameworkCore;

namespace PrincipalRepro;

internal sealed class ReproContext : DbContext
{
    public ReproContext(DbContextOptions<ReproContext> opt) : base(opt) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityA>(builder =>
        {
            builder.HasKey(e => e.PKKey);
            builder.Property(ac => ac.PKKey)
                .ValueGeneratedNever();

            builder.HasMany(e => e.EntityBs)
                .WithOne()
                .HasForeignKey(e => new { e.PKKeyOne, e.PKKeyTwo })
                .HasPrincipalKey(e => new { e.FKKeyOne, e.FKKeyTwo }) //This creates a alternative key for EntityA´s tracked state which will fail since more than one EntityA can have same EntityB
                .IsRequired(false);

            builder.HasOne(e => e.Parent)
                .WithOne()
                .HasForeignKey<EntityA>(e => e.ParentId)
                .IsRequired(false);
        });


        modelBuilder.Entity<EntityB>(builder => builder.HasKey(e => new { e.PKKeyOne, e.PKKeyTwo, e.PKKeyThree }));


    }
}