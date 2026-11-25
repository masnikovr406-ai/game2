using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>Реализация IRepository на Entity Framework. Контекст создаётся на каждую операцию, поэтому класс безопасен как Singleton.</summary>
public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
{
    private readonly string _connectionString;

    public EntityRepository(string connectionString) => _connectionString = connectionString;

    private AppDbContext CreateContext() => new(_connectionString);

    public void Add(T item)
    {
        item.Id = 0;
        using var context = CreateContext();
        context.Set<T>().Add(item);
        context.SaveChanges();
    }

    public bool Delete(int id)
    {
        using var context = CreateContext();
        var item = context.Set<T>().Find(id);
        if (item == null) return false;

        context.Set<T>().Remove(item);
        context.SaveChanges();
        return true;
    }

    public IEnumerable<T> ReadAll()
    {
        using var context = CreateContext();
        return context.Set<T>().AsNoTracking().ToList();
    }

    public T ReadById(int id)
    {
        using var context = CreateContext();
        return context.Set<T>().AsNoTracking()
            .FirstOrDefault(x => EF.Property<int>(x, "Id") == id);
    }

    public bool Update(T item)
    {
        using var context = CreateContext();
        bool exists = context.Set<T>().AsNoTracking()
            .Any(x => EF.Property<int>(x, "Id") == item.Id);
        if (!exists) return false;

        context.Set<T>().Update(item);
        context.SaveChanges();
        return true;
    }
}
