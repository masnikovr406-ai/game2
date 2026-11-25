using BusinessLogic.Reports;
using DataAccessLayer;
using Model;

namespace BusinessLogic;

/// <summary>
/// Бизнес-логика каталога видеоигр. Зависит только от абстракций:
/// хранилища (IRepository) и отчётов (IGameReport). Построение отчётов вынесено в отдельные классы.
/// </summary>
public class Logic
{
    private readonly IGameReport<Dictionary<string, List<VideoGame>>> _groupByDeveloper;
    private readonly IGameReport<Dictionary<string, double>> _averageRatingByGenre;

    public IRepository<VideoGame> Repository { get; }

    public Logic(
        IRepository<VideoGame> repository,
        IGameReport<Dictionary<string, List<VideoGame>>> groupByDeveloper,
        IGameReport<Dictionary<string, double>> averageRatingByGenre)
    {
        Repository = repository;
        _groupByDeveloper = groupByDeveloper;
        _averageRatingByGenre = averageRatingByGenre;
    }

    // ---- CRUD ----
    public void Create(VideoGame game) => Repository.Add(game);

    public IEnumerable<VideoGame> ReadAll() => Repository.ReadAll();

    public VideoGame ReadById(int id) => Repository.ReadById(id);

    public bool Update(VideoGame game) => Repository.Update(game);

    public bool Delete(int id) => Repository.Delete(id);

    // ---- Бизнес-функции ----

    /// <summary>Группирует игры по разработчикам.</summary>
    public Dictionary<string, List<VideoGame>> GroupByDeveloper() => BuildReport(_groupByDeveloper);

    /// <summary>Считает среднюю оценку игр по жанрам.</summary>
    public Dictionary<string, double> AverageRatingByGenre() => BuildReport(_averageRatingByGenre);

    /// <summary>Точка расширения: строит любой отчёт по всем играм каталога.</summary>
    public TResult BuildReport<TResult>(IGameReport<TResult> report) => report.Build(Repository.ReadAll());
}
