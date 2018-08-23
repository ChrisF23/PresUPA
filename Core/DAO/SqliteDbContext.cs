using Microsoft.EntityFrameworkCore;

namespace Core.DAO
{
    /// <summary>
    /// Sqlite3 DB Context
    /// </summary>
    public sealed class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
            // Al momento de crear el contexto, se borra y crea la base de datos.
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Lugar fisico de la base de datos.
            optionsBuilder.UseSqlite(@"Data Source=database.db");
        }
    }
}