using Microsoft.EntityFrameworkCore;
using Teatronik.Infrastructure.Configurations;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure
{
    public class TeatronikDbContext(DbContextOptions<TeatronikDbContext> options) : DbContext(options)
    {
        public DbSet<ComponentEntity> Components { get; set; }
        public DbSet<ComponentModelEntity> ComponentModels { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<KindEntity> Kinds { get; set; }
        public DbSet<PropEntity> Props { get; set; }
        public DbSet<PropSchemaEntity> PropSchemas { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<SeasonEntity> Seasons { get; set; }
        public DbSet<TypeEntity> Types { get; set; }
        public DbSet<UserEntity> Users { get; set; }

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
