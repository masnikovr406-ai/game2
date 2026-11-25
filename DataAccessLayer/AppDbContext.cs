using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public AppDbContext(string connectionString) => _connectionString = connectionString;

    public DbSet<VideoGame> VideoGames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(_connectionString);
}
