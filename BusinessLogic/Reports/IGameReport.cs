using Model;

namespace BusinessLogic.Reports;

/// <summary>Отчёт по набору игр. Новый отчёт добавляется новым классом без изменения Logic.</summary>
public interface IGameReport<out TResult>
{
    /// <summary>Строит отчёт по переданным играм.</summary>
    TResult Build(IEnumerable<VideoGame> games);
}
