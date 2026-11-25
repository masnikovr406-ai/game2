using DataAccessLayer;
using Model;
using Xunit;

namespace Tests;

/// <summary>
/// Контракт IRepository. Одни и те же тесты запускаются для каждой реализации:
/// если все проходят, реализации взаимозаменяемы (принцип подстановки Лисков).
/// </summary>
public abstract class RepositoryContractTests
{
    private readonly string _connectionString = $"Data Source=test_{Guid.NewGuid():N}.db";

    protected abstract IRepository<VideoGame> CreateRepository(string connectionString);

    private IRepository<VideoGame> Create()
    {
        new SqliteDatabaseInitializer(_connectionString).Initialize();
        return CreateRepository(_connectionString);
    }

    private static VideoGame NewGame(string title = "Hades") => new()
    {
        Title = title, Developer = "Supergiant", Genre = "Roguelike", ReleaseYear = 2020, Rating = 9.5
    };

    [Fact]
    public void Add_AssignsId_AndIgnoresPresetId()
    {
        var repo = Create();
        var game = NewGame();
        game.Id = 999;

        repo.Add(game);

        Assert.NotEqual(999, game.Id);
        Assert.NotNull(repo.ReadById(game.Id));
    }

    [Fact]
    public void ReadById_Missing_ReturnsNull() => Assert.Null(Create().ReadById(12345));

    [Fact]
    public void ReadAll_Empty_ReturnsEmptyCollection() => Assert.Empty(Create().ReadAll());

    [Fact]
    public void Update_Existing_ChangesData_AndReturnsTrue()
    {
        var repo = Create();
        var game = NewGame();
        repo.Add(game);

        game.Rating = 7.0;

        Assert.True(repo.Update(game));
        Assert.Equal(7.0, repo.ReadById(game.Id).Rating);
    }

    [Fact]
    public void Update_Missing_ReturnsFalse()
    {
        var game = NewGame();
        game.Id = 12345;
        Assert.False(Create().Update(game));
    }

    [Fact]
    public void Delete_Existing_RemovesItem_AndReturnsTrue()
    {
        var repo = Create();
        var game = NewGame();
        repo.Add(game);

        Assert.True(repo.Delete(game.Id));
        Assert.Null(repo.ReadById(game.Id));
    }

    [Fact]
    public void Delete_Missing_ReturnsFalse() => Assert.False(Create().Delete(12345));
}

public class EntityRepositoryTests : RepositoryContractTests
{
    protected override IRepository<VideoGame> CreateRepository(string connectionString) =>
        new EntityRepository<VideoGame>(connectionString);
}

public class DapperRepositoryTests : RepositoryContractTests
{
    protected override IRepository<VideoGame> CreateRepository(string connectionString) =>
        new DapperRepository<VideoGame>(connectionString);
}
