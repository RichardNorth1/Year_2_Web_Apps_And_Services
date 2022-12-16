using Microsoft.EntityFrameworkCore;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Data
{
    public class EventsDBContext: DbContext
    {
        public string DbPath { get; }

        public DbSet<Event> Event { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<GuestBooking> GuestBooking { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Staffing> Staffing { get; set; }

        public EventsDBContext(DbContextOptions<EventsDBContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "Events.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);

            // handle the many to many guests at an event

            // Composite key
            modelBuilder.Entity<GuestBooking>()
                .HasKey(a => new { a.EventId, a.GuestId });


            modelBuilder.Entity<GuestBooking>()
                .HasOne(g => g.Guest)
                .WithMany(gb => gb.Events)
                .HasForeignKey(g => g.GuestId);

            // Handle the many to many
            modelBuilder.Entity<GuestBooking>()
                .HasOne(e => e.Event)
                .WithMany(g => g.GuestBookings)
                .HasForeignKey(e => e.EventId);



            // handle the many to many staff at an event

            modelBuilder.Entity<Staffing>()
                .HasKey(a => new { a.StaffId, a.EventId });

            modelBuilder.Entity<Staffing>()
                .HasOne(e => e.Event)
                .WithMany(s => s.Staffs)
                .HasForeignKey(e => e.EventId);

            modelBuilder.Entity<Staffing>()
                .HasOne(m => m.Staff)
                .WithMany(e => e.Events)
                .HasForeignKey(m => m.StaffId);


            // seed data

            modelBuilder.Entity<Event>()
                .HasData(
                //20221107CRKHL
                new Event(1, "The North Family Birthday bash", 1, "Reference1234", "PTY"),
                //20221106TNDMR
                new Event(2, "The annual golf meet up", 2, "Reference2345", "MET")
                );



            modelBuilder.Entity<Guest>()
                .HasData(
                new Guest(1,"Richard", "North", 0164266223, "RichardNorth@email.com", "21 jump street"),
                new Guest(2, "Daniel", "North", 0164263223, "DanielNorth@email.com", "22 hop street"),
                new Guest(3, "Micheal", "North", 01642642223, "MichealNorth@email.com", "23 skip street"),
                new Guest(4, "Robert", "North", 0163366223, "RobertNorth@email.com", "24 run street"),
                new Guest(5, "Aidan", "North", 0164266213, "AidanNorth@email.com", "25 skid street"),
                new Guest(6, "Thomas", "North", 0164266923, "ThomasNorth@email.com", "26 slide street")
                );
            modelBuilder.Entity<GuestBooking>()
                .HasData(
                new GuestBooking(1, 1),
                new GuestBooking(2, 1),
                new GuestBooking(3, 1),
                new GuestBooking(4, 1),
                new GuestBooking(5, 1),
                new GuestBooking(1, 2),
                new GuestBooking(6, 2),
                new GuestBooking(4, 2)
                );

            modelBuilder.Entity<Staff>()
                .HasData(
                new Staff(1, "Chelsea", "Copland", EmployeeType.MANAGER, true, "chelseacopeland@email.com", "password"),
                new Staff(2, "michelle", "Copland", EmployeeType.TEAMMEMBER, false, "michellecopeland@email.com", "password"),
                new Staff(3, "Carly", "Copland", EmployeeType.TEAMMEMBER, false, "Carlycopeland@email.com", "password"),
                new Staff(4, "ciara", "Copland", EmployeeType.TEAMMEMBER, true, "ciaracopeland@email.com", "password"),
                new Staff(5, "anna", "Copland", EmployeeType.TEAMMEMBER, false, "annacopeland@email.com", "password"),
                new Staff(6, "dee", "Copland", EmployeeType.TEAMLEADER, true, "deecopeland@email.com", "password")
                );

            modelBuilder.Entity<Staffing>()
                .HasData(
                new Staffing(1, 1),
                new Staffing(2, 1),
                new Staffing(3, 1),
                new Staffing(4, 1),
                new Staffing(3, 2),
                new Staffing(1, 2),
                new Staffing(6, 2)
                );

        }
    }
}
