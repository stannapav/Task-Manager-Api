using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    /// <summary>
    /// DataContext is an instance that can be used to query and save objects to database
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TaskNode> Tasks { get; set; }
    }
}
