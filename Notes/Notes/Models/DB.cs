using System.Data.Entity;

namespace Notes.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DB_Context")
        { }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
    }

    public static class DB
    {
        public static DBContext db = new DBContext();
    }
}