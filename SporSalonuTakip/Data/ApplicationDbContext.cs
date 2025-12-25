using Microsoft.EntityFrameworkCore;
using SporSalonuTakip.Models;

namespace SporSalonuTakip.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanındaki 'Members' tablosunu temsil eder
        public DbSet<Member> Members { get; set; }
    }
}