using Microsoft.EntityFrameworkCore;

namespace SmartShelf.web.Data
{
    public class SmartShelfContext : DbContext
    {
        public SmartShelfContext(DbContextOptions<SmartShelfContext> options)
            : base(options) { }
    }
}