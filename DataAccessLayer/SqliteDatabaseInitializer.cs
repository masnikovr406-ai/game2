namespace DataAccessLayer;

/// <summary>Создаёт файл базы данных и таблицы, если их ещё нет. Используется обоими сценариями (EF и Dapper).</summary>
public class SqliteDatabaseInitializer
{
    private readonly string _connectionString;

    public SqliteDatabaseInitializer(string connectionString) => _connectionString = connectionString;

    public void Initialize()
    {
        using var context = new AppDbContext(_connectionString);
        context.Database.EnsureCreated();
    }
}
