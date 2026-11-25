using Model;

namespace ConsoleView;

/// <summary>Отвечает только за оформление вывода: цвета, рамки, таблицы, диаграммы.</summary>
public class ConsoleRenderer
{
    private static readonly (string Header, int Width)[] Columns =
    {
        ("Id", 4), ("Название", 28), ("Разработчик", 20), ("Жанр", 14), ("Год", 6), ("Оценка", 7)
    };

    private static readonly (string Key, string Label)[] MenuItems =
    {
        ("1", "Добавить игру"),
        ("2", "Показать все игры"),
        ("3", "Изменить игру"),
        ("4", "Удалить игру"),
        ("5", "Группировка по разработчикам"),
        ("6", "Средняя оценка по жанрам"),
        ("0", "Выход")
    };

    public void Banner()
    {
        const string title = "КАТАЛОГ ВИДЕОИГР";
        const int width = 40;
        WriteLine("╔" + new string('═', width) + "╗", ConsoleColor.Magenta);
        Write("║", ConsoleColor.Magenta);
        Write(title.PadLeft((width + title.Length) / 2).PadRight(width), ConsoleColor.Cyan);
        WriteLine("║", ConsoleColor.Magenta);
        WriteLine("╚" + new string('═', width) + "╝", ConsoleColor.Magenta);
    }

    public void Menu()
    {
        Console.WriteLine();
        foreach (var (key, label) in MenuItems)
        {
            Write($"  [{key}] ", ConsoleColor.Yellow);
            WriteLine(label, ConsoleColor.White);
        }
        Write("\n> ", ConsoleColor.Cyan);
    }

    public void Table(IEnumerable<VideoGame> games)
    {
        Console.WriteLine();
        WriteBorder('┌', '┬', '┐');
        WriteRow(Columns.Select(c => c.Header).ToArray(), ConsoleColor.Cyan, null);
        WriteBorder('├', '┼', '┤');
        foreach (var game in games)
        {
            WriteRow(
                new[] { game.Id.ToString(), game.Title, game.Developer, game.Genre,
                        game.ReleaseYear.ToString(), game.Rating.ToString("0.0") },
                ConsoleColor.White,
                RatingColor(game.Rating));
        }
        WriteBorder('└', '┴', '┘');
    }

    public void Groups(Dictionary<string, List<VideoGame>> groups)
    {
        Console.WriteLine();
        foreach (var pair in groups.OrderBy(p => p.Key))
        {
            Write("■ ", ConsoleColor.Yellow);
            Write(pair.Key, ConsoleColor.Yellow);
            WriteLine($"  ({pair.Value.Count})", ConsoleColor.DarkGray);
            foreach (var game in pair.Value)
            {
                Write("   └ ", ConsoleColor.DarkGray);
                Write(game.Title, ConsoleColor.White);
                WriteLine($"  {game.Rating:0.0}", RatingColor(game.Rating));
            }
        }
    }

    public void Averages(Dictionary<string, double> averages)
    {
        const int cells = 20;
        Console.WriteLine();
        foreach (var pair in averages.OrderByDescending(p => p.Value))
        {
            int filled = (int)Math.Round(Math.Clamp(pair.Value, 0, 10) * cells / 10.0);
            var color = RatingColor(pair.Value);
            Write($"  {Fit(pair.Key, 16)} ", ConsoleColor.White);
            Write(new string('█', filled), color);
            Write(new string('░', cells - filled), ConsoleColor.DarkGray);
            WriteLine($" {pair.Value:0.00}", color);
        }
    }

    public void Success(string message) => WriteLine("✔ " + message, ConsoleColor.Green);

    public void Error(string message) => WriteLine("✖ " + message, ConsoleColor.Red);

    public void Info(string message) => WriteLine("• " + message, ConsoleColor.DarkYellow);

    private static ConsoleColor RatingColor(double rating) =>
        rating >= 8 ? ConsoleColor.Green : rating >= 6 ? ConsoleColor.Yellow : ConsoleColor.Red;

    private static void WriteBorder(char left, char middle, char right)
    {
        string line = left +
                      string.Join(middle.ToString(), Columns.Select(c => new string('─', c.Width + 2))) +
                      right;
        WriteLine(line, ConsoleColor.DarkGray);
    }

    private static void WriteRow(string[] cells, ConsoleColor color, ConsoleColor? ratingColor)
    {
        Write("│", ConsoleColor.DarkGray);
        for (int i = 0; i < Columns.Length; i++)
        {
            bool isRating = i == Columns.Length - 1 && ratingColor.HasValue;
            Write(" " + Fit(cells[i], Columns[i].Width) + " ", isRating ? ratingColor.Value : color);
            Write("│", ConsoleColor.DarkGray);
        }
        Console.WriteLine();
    }

    private static string Fit(string text, int width) =>
        text.Length <= width ? text.PadRight(width) : text[..(width - 1)] + "…";

    private static void Write(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    private static void WriteLine(string text, ConsoleColor color)
    {
        Write(text, color);
        Console.WriteLine();
    }
}
