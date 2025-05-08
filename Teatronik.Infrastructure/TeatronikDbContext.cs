using Microsoft.EntityFrameworkCore;
using Teatronik.Infrastructure.Configurations;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure
{
    public class TeatronikDbContext(DbContextOptions<TeatronikDbContext> options) : DbContext(options)
    {
        DbSet<ComponentEntity> Components { get; set; }
        DbSet<ComponentModelEntity> ComponentModels { get; set; }
        DbSet<EventEntity> Events { get; set; }
        DbSet<KindEntity> Kinds { get; set; }
        DbSet<PropEntity> Props { get; set; }
        DbSet<PropSchemaEntity> PropSchemas { get; set; }
        DbSet<RoleEntity> Roles { get; set; }
        DbSet<SeasonEntity> Seasons { get; set; }
        DbSet<TypeEntity> Types { get; set; }
        DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ComponentConfiguration());
            modelBuilder.ApplyConfiguration(new ComponentModelConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new KindConfiguration());
            modelBuilder.ApplyConfiguration(new PropConfiguration());
            modelBuilder.ApplyConfiguration(new PropSchemaConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new SeasonConfiguration());
            modelBuilder.ApplyConfiguration(new TypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
