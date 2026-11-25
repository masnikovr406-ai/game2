using BusinessLogic;

namespace ConsoleView;

/// <summary>Консольное меню: разбирает команды и вызывает бизнес-логику. Оформление делает ConsoleRenderer.</summary>
public class GameConsoleApp
{
    private readonly Logic _logic;
    private readonly ConsoleRenderer _view;

    public GameConsoleApp(Logic logic, ConsoleRenderer view)
    {
        _logic = logic;
        _view = view;
    }

    public void Run()
    {
        _view.Banner();
        bool running = true;
        while (running)
        {
            _view.Menu();
            running = Handle(Console.ReadLine());
        }
    }

    private bool Handle(string choice)
    {
        switch (choice)
        {
            case "1": AddGame(); break;
            case "2": ShowAll(); break;
            case "3": EditGame(); break;
            case "4": DeleteGame(); break;
            case "5": ShowGroups(); break;
            case "6": ShowAverages(); break;
            case "0": return false;
            default: _view.Error("Неизвестная команда."); break;
        }
        return true;
    }

    private void AddGame()
    {
        _logic.Create(ConsoleInput.ReadGame());
        _view.Success("Игра добавлена.");
    }

    private void ShowAll()
    {
        var games = _logic.ReadAll().ToList();
        if (games.Count == 0) _view.Info("Список пуст.");
        else _view.Table(games);
    }

    private void EditGame()
    {
        int id = ConsoleInput.ReadInt("Id: ");
        if (_logic.ReadById(id) == null)
        {
            _view.Error("Игра не найдена.");
            return;
        }

        var game = ConsoleInput.ReadGame();
        game.Id = id;
        if (_logic.Update(game)) _view.Success("Игра изменена.");
        else _view.Error("Игра не найдена.");
    }

    private void DeleteGame()
    {
        int id = ConsoleInput.ReadInt("Id: ");
        if (_logic.Delete(id)) _view.Success("Игра удалена.");
        else _view.Error("Игра не найдена.");
    }

    private void ShowGroups()
    {
        var groups = _logic.GroupByDeveloper();
        if (groups.Count == 0) _view.Info("Список пуст.");
        else _view.Groups(groups);
    }

    private void ShowAverages()
    {
        var averages = _logic.AverageRatingByGenre();
        if (averages.Count == 0) _view.Info("Список пуст.");
        else _view.Averages(averages);
    }
}
