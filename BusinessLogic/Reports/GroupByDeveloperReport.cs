using Model;

namespace BusinessLogic.Reports;

/// <summary>Группирует игры по разработчикам.</summary>
public class GroupByDeveloperReport : IGameReport<Dictionary<string, List<VideoGame>>>
{
    public Dictionary<string, List<VideoGame>> Build(IEnumerable<VideoGame> games) =>
        games.GroupBy(g => g.Developer)
             .ToDictionary(g => g.Key, g => g.ToList());
}
