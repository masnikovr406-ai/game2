using System.Reflection;
using Dapper;
using Microsoft.Data.Sqlite;
using Model;

namespace DataAccessLayer;

/// <summary>Реализация IRepository на Dapper. Таблица называется как тип + "s" (VideoGame -> VideoGames).</summary>
public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
{
    private static readonly string Table = typeof(T).Name + "s";

    // все свойства, кроме Id
    private static readonly string[] Columns = typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.CanRead && p.CanWrite && p.Name != nameof(IDomainObject.Id))
        .Select(p => p.Name)
        .ToArray();

    private readonly string _connectionString;

    public DapperRepository(string connectionString) => _connectionString = connectionString;

    private SqliteConnection Open()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public void Add(T item)
    {
        var sql = $"INSERT INTO {Table} ({string.Join(", ", Columns)}) " +
                  $"VALUES ({string.Join(", ", Columns.Select(c => "@" + c))}); " +
                  "SELECT last_insert_rowid();";
        using var db = Open();
        item.Id = (int)db.ExecuteScalar<long>(sql, item);
    }

    public bool Delete(int id)
    {
        using var db = Open();
        return db.Execute($"DELETE FROM {Table} WHERE Id = @id", new { id }) > 0;
    }

    public IEnumerable<T> ReadAll()
    {
        using var db = Open();
        return db.Query<T>($"SELECT * FROM {Table}").ToList();
    }

    public T ReadById(int id)
    {
        using var db = Open();
        return db.QueryFirstOrDefault<T>($"SELECT * FROM {Table} WHERE Id = @id", new { id });
    }

    public bool Update(T item)
    {
        var sql = $"UPDATE {Table} SET {string.Join(", ", Columns.Select(c => $"{c} = @{c}"))} WHERE Id = @Id";
        using var db = Open();
        return db.Execute(sql, item) > 0;
    }
}
