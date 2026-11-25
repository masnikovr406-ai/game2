# GameCatalog (сущность: VideoGame)

Слои: Model <- DataAccessLayer <- BusinessLogic <- ConsoleView / WinFormsView.  Tests - проверки контракта.

Создание решения:
    dotnet new sln -n GameCatalog
    dotnet sln add Model DataAccessLayer BusinessLogic ConsoleView WinFormsView Tests

Запуск:
    dotnet run --project ConsoleView
    dotnet run --project WinFormsView      (только Windows)
    dotnet test

Переключение EF <-> Dapper: BusinessLogic/SimpleConfigModule.cs

## SOLID в проекте
S - Logic: CRUD и делегирование; отчёты вынесены в BusinessLogic/Reports; инициализация БД - SqliteDatabaseInitializer;
    консоль разбита на Program (композиция), GameConsoleApp (меню), ConsoleInput (ввод).
O - новое хранилище = новый класс IRepository<T>; новый отчёт = новый класс IGameReport<T> + Logic.BuildReport,
    Logic и существующие классы не меняются.
L - Tests/RepositoryContractTests: один набор тестов для EF- и Dapper-репозитория (Update/Delete несуществующего -> false,
    ReadById -> null, Id при Add игнорируется). Summary интерфейсов описывает контракт, а не EF или Dapper.
I - IRepository<T> = IReadRepository<T> + IWriteRepository<T>; отчёты получают IEnumerable<VideoGame>, а не репозиторий.
D - Logic получает IRepository и IGameReport через конструктор; привязки и Singleton - в SimpleConfigModule (Ninject).
