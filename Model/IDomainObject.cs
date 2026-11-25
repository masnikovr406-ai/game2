namespace Model;

/// <summary>Объект предметной области, имеющий идентификатор.</summary>
public interface IDomainObject
{
    int Id { get; set; }
}
