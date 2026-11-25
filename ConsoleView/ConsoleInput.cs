using Model;

namespace ConsoleView;

/// <summary>Чтение и проверка данных, введённых пользователем в консоли.</summary>
public static class ConsoleInput
{
    public static string ReadString(string prompt)
    {
        WritePrompt(prompt);
        return Console.ReadLine() ?? "";
    }

    public static int ReadInt(string prompt)
    {
        WritePrompt(prompt);
        int value;
        while (!int.TryParse(Console.ReadLine(), out value))
            WritePrompt("Введите целое число: ");
        return value;
    }

    public static double ReadDouble(string prompt)
    {
        WritePrompt(prompt);
        double value;
        while (!double.TryParse(Console.ReadLine(), out value))
            WritePrompt("Введите число: ");
        return value;
    }

    public static VideoGame ReadGame() => new()
    {
        Title = ReadString("Название: "),
        Developer = ReadString("Разработчик: "),
        Genre = ReadString("Жанр: "),
        ReleaseYear = ReadInt("Год выпуска: "),
        Rating = ReadDouble("Оценка (0-10): ")
    };

    private static void WritePrompt(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(text);
        Console.ResetColor();
    }
}
