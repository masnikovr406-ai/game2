using Model;

namespace BusinessLogic.Reports;

/// <summary>Считает среднюю оценку игр по жанрам.</summary>
public class AverageRatingByGenreReport : IGameReport<Dictionary<string, double>>
{
    public Dictionary<string, double> Build(IEnumerable<VideoGame> games) =>
        games.GroupBy(g => g.Genre)
             .ToDictionary(g => g.Key, g => g.Average(x => x.Rating));
}
