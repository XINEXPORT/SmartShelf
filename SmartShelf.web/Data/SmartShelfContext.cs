using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Models;

namespace SmartShelf.web.Data
{
    public class SmartShelfContext : DbContext
    {
        public SmartShelfContext(DbContextOptions<SmartShelfContext> options)
            : base(options) { }

        //Models for database tables
        public DbSet<TagReadEvent> TagReadEvents { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Reader> Reader { get; set; }
    }
}