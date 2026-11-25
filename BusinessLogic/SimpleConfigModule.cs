using BusinessLogic.Reports;
using DataAccessLayer;
using Model;
using Ninject.Modules;

namespace BusinessLogic;

/// <summary>Корень композиции: сопоставляет абстракции и реализации для Ninject.</summary>
public class SimpleConfigModule : NinjectModule
{
    // Один общий файл БД для консольного и WinForms-представлений.
    private static readonly string ConnectionString = BuildConnectionString();

    private static string BuildConnectionString()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GameCatalog");
        Directory.CreateDirectory(folder);
        return $"Data Source={Path.Combine(folder, "games.db")}";
    }

    public override void Load()
    {
        // EF-сценарий (Singleton). БД создаётся при первом создании репозитория.
        Bind<IRepository<VideoGame>>().To<EntityRepository<VideoGame>>()
            .InSingletonScope()
            .WithConstructorArgument("connectionString", ConnectionString)
            .OnActivation(_ => new SqliteDatabaseInitializer(ConnectionString).Initialize());

        // Dapper-сценарий: закомментируйте блок выше и раскомментируйте этот
        // Bind<IRepository<VideoGame>>().To<DapperRepository<VideoGame>>()
        //     .InSingletonScope()
        //     .WithConstructorArgument("connectionString", ConnectionString)
        //     .OnActivation(_ => new SqliteDatabaseInitializer(ConnectionString).Initialize());

        Bind<IGameReport<Dictionary<string, List<VideoGame>>>>().To<GroupByDeveloperReport>();
        Bind<IGameReport<Dictionary<string, double>>>().To<AverageRatingByGenreReport>();
    }
}
