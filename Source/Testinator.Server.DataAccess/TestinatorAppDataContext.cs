using Microsoft.EntityFrameworkCore;
using Testinator.Server.DataAccess.Entities;

namespace Testinator.Server.DataAccess
{
    public class TestinatorAppDataContext : DbContext
    {
        public TestinatorAppDataContext(DbContextOptions<TestinatorAppDataContext> options) : base(options) { }

        public DbSet<Setting> Settings { get; set; }
    }
}
