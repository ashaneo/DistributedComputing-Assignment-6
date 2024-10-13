using Microsoft.EntityFrameworkCore;
using DataTierAPI.Models;

namespace DataTierAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<DataIntermed> DataRecords { get; set; }
    }
}