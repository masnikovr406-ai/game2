namespace Model;

/// <summary>Видеоигра.</summary>
public class VideoGame : IDomainObject
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Developer { get; set; } = "";
    public string Genre { get; set; } = "";
    public int ReleaseYear { get; set; }
    /// <summary>Оценка от 0 до 10.</summary>
    public double Rating { get; set; }

    public override string ToString() =>
        $"[{Id}] {Title} | {Developer} | {Genre} | {ReleaseYear} | {Rating:0.0}";
}
