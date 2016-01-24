namespace KalashianFamily.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NickBeckyWedding : DbContext
    {
        public NickBeckyWedding()
            : base("name=NickBeckyWedding")
        {
        }

        public virtual DbSet<RSVP> RSVPs { get; set; }
        public virtual DbSet<RSVPAttendee> RSVPAttendees { get; set; }
        public virtual DbSet<GuestBookMessage> GuestBookMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RSVP>()
                .HasMany(e => e.RSVPAttendees)
                .WithRequired(e => e.RSVP)
                .HasForeignKey(e => e.RSVP_ID)
                .WillCascadeOnDelete(false);
        }
    }
}
