using HR_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HR_API.Data
{
    public class EntryDbContext : DbContext
    {
        public EntryDbContext(DbContextOptions<EntryDbContext> options)
            :base(options)
        {
            
        }
        public DbSet<Entry> Entries { get; set;  }

        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter()
                : base(dateOnly =>
                        dateOnly.ToDateTime(TimeOnly.MinValue),
                    dateTime => DateOnly.FromDateTime(dateTime))
            { }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("datetime2");

            base.ConfigureConventions(builder);

        }

}
}
