using BusinessLogic.Reports;
using Model;
using Xunit;

namespace Tests;

public class ReportTests
{
    private static readonly VideoGame[] Games =
    {
        new() { Title = "A", Developer = "X", Genre = "RPG", Rating = 8 },
        new() { Title = "B", Developer = "X", Genre = "RPG", Rating = 6 },
        new() { Title = "C", Developer = "Y", Genre = "Puzzle", Rating = 9 }
    };

    [Fact]
    public void GroupByDeveloper_GroupsGames()
    {
        var result = new GroupByDeveloperReport().Build(Games);
        Assert.Equal(2, result["X"].Count);
        Assert.Single(result["Y"]);
    }

    [Fact]
    public void AverageRatingByGenre_ComputesAverage()
    {
        var result = new AverageRatingByGenreReport().Build(Games);
        Assert.Equal(7.0, result["RPG"]);
        Assert.Equal(9.0, result["Puzzle"]);
    }
}
