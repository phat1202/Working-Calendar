using Microsoft.EntityFrameworkCore;

namespace Calendar.Models
{
    public class CalendarContext : DbContext
    {
        public CalendarContext()
        { }
        public CalendarContext(DbContextOptions<CalendarContext> options) : base(options) { }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<WorkingDay> workingDays { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {
            optionsBuilder.UseMySQL("Server=localhost;port=3306;username=root;Password=123456;Database=holidays;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
        }


    }
}
