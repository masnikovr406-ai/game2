using Model;

namespace DataAccessLayer;

/// <summary>Операции чтения хранилища.</summary>
public interface IReadRepository<T> where T : IDomainObject
{
    /// <summary>Возвращает все сохранённые объекты. Если объектов нет, возвращает пустую коллекцию.</summary>
    IEnumerable<T> ReadAll();

    /// <summary>Возвращает объект с указанным идентификатором или null, если такого объекта нет.</summary>
    T ReadById(int id);
}

/// <summary>Операции записи хранилища.</summary>
public interface IWriteRepository<T> where T : IDomainObject
{
    /// <summary>Сохраняет новый объект. Ранее заданный Id игнорируется; присвоенный Id записывается в item.Id.</summary>
    void Add(T item);

    /// <summary>Заменяет сохранённый объект с тем же Id. Возвращает false, если такого объекта нет.</summary>
    bool Update(T item);

    /// <summary>Удаляет объект с указанным Id. Возвращает false, если такого объекта нет.</summary>
    bool Delete(int id);
}

/// <summary>Фасад CRUD: чтение и запись вместе. Клиенты, которым нужна только часть операций, зависят от узких интерфейсов.</summary>
public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : IDomainObject
{
}
