using Microsoft.EntityFrameworkCore;
using Npgsql;
using TodoApi.Helpers;

namespace TodoApi.Models
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Todo> Todo { get; set; }

        static DataContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<todo_status>
                ("todo_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Let PSQL Database know to use database enum
            modelBuilder.HasPostgresEnum<todo_status>();
        }
    }
}
