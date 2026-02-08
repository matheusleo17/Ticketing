using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options) { }


        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Status)
                      .HasConversion<string>();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(o => o.Id);

                entity.Property(o => o.Status)
                      .HasConversion<string>();

                entity.Property(o=> o.ExpiresAt)
                      .IsRequired();

                entity.Property(o => o.TicketId).IsRequired();

                entity.HasOne<Ticket>().WithMany()
                                       .HasForeignKey(o=> o.TicketId);


            });
        }
    }
}
