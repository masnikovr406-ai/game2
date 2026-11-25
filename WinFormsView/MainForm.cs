using BusinessLogic;
using Model;

namespace WinFormsView;

public class MainForm : Form
{
    private readonly Logic _logic;

    private readonly Panel _content = new() { Dock = DockStyle.Fill };
    private readonly Panel _gamesPage = new() { Dock = DockStyle.Fill };
    private readonly Panel _developersPage = new() { Dock = DockStyle.Fill };
    private readonly Panel _genresPage = new() { Dock = DockStyle.Fill };

    private readonly DataGridView _grid = new();
    private readonly TreeView _tree = new();
    private readonly RatingChart _chart = new() { Dock = DockStyle.Fill };

    private readonly TextBox _title = Theme.NewTextBox("Название", 200);
    private readonly TextBox _developer = Theme.NewTextBox("Разработчик", 160);
    private readonly TextBox _genre = Theme.NewTextBox("Жанр", 120);
    private readonly TextBox _year = Theme.NewTextBox("Год", 70);
    private readonly TextBox _rating = Theme.NewTextBox("Оценка 0-10", 100);

    public MainForm(Logic logic)
    {
        _logic = logic;

        Text = "Каталог видеоигр";
        ClientSize = new Size(980, 620);
        MinimumSize = new Size(820, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Theme.Background;
        ForeColor = Theme.Text;
        Font = Theme.Regular;

        BuildGamesPage();
        BuildDevelopersPage();
        BuildGenresPage();
        _content.Controls.AddRange(new Control[] { _gamesPage, _developersPage, _genresPage });

        // порядок добавления важен: последний добавленный элемент занимает место первым
        Controls.Add(_content);
        Controls.Add(BuildNavigation());
        Controls.Add(new GradientHeader { Dock = DockStyle.Top, Height = 80 });

        ShowPage(_gamesPage);
    }

    // ---------- построение интерфейса ----------

    private Control BuildNavigation()
    {
        var nav = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 52,
            BackColor = Theme.Surface,
            Padding = new Padding(10, 6, 0, 0)
        };
        nav.Controls.Add(Theme.NewButton("Игры", (_, _) => ShowPage(_gamesPage)));
        nav.Controls.Add(Theme.NewButton("По разработчикам", (_, _) => ShowPage(_developersPage), Theme.Accent2));
        nav.Controls.Add(Theme.NewButton("Оценки по жанрам", (_, _) => ShowPage(_genresPage), Theme.Warn));
        return nav;
    }

    private void BuildGamesPage()
    {
        ConfigureGrid();

        var form = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 100,
            BackColor = Theme.Surface,
            Padding = new Padding(10, 10, 10, 0)
        };
        form.Controls.AddRange(new Control[] { _title, _developer, _genre, _year, _rating });
        form.Controls.Add(Theme.NewButton("Добавить", (_, _) => Save(isNew: true), Theme.Good));
        form.Controls.Add(Theme.NewButton("Изменить", (_, _) => Save(isNew: false)));
        form.Controls.Add(Theme.NewButton("Удалить", (_, _) => DeleteSelected(), Theme.Bad));

        _gamesPage.Controls.Add(_grid);
        _gamesPage.Controls.Add(form);
    }

    private void ConfigureGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.AutoGenerateColumns = false;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.MultiSelect = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.RowHeadersVisible = false;
        _grid.BorderStyle = BorderStyle.None;
        _grid.EnableHeadersVisualStyles = false;
        _grid.BackgroundColor = Theme.Background;
        _grid.GridColor = Theme.Surface;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        _grid.ColumnHeadersHeight = 38;
        _grid.RowTemplate.Height = 32;

        _grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Theme.Surface,
            ForeColor = Theme.Accent,
            Font = Theme.Bold,
            SelectionBackColor = Theme.Surface,
            SelectionForeColor = Theme.Accent
        };
        _grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Theme.Background,
            ForeColor = Theme.Text,
            SelectionBackColor = Theme.Accent2,
            SelectionForeColor = Theme.Background
        };
        _grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Theme.RowAlt };

        AddColumn(nameof(VideoGame.Id), "Id", 30);
        AddColumn(nameof(VideoGame.Title), "Название", 160);
        AddColumn(nameof(VideoGame.Developer), "Разработчик", 120);
        AddColumn(nameof(VideoGame.Genre), "Жанр", 90);
        AddColumn(nameof(VideoGame.ReleaseYear), "Год", 50);
        AddColumn(nameof(VideoGame.Rating), "Оценка", 60);

        _grid.CellFormatting += ColorRating;
        _grid.SelectionChanged += (_, _) => FillInputs();
    }

    private void AddColumn(string property, string header, float weight) =>
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = property,
            DataPropertyName = property,
            HeaderText = header,
            FillWeight = weight
        });

    private void ColorRating(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || _grid.Columns[e.ColumnIndex].Name != nameof(VideoGame.Rating)) return;
        if (e.Value is double rating)
        {
            e.CellStyle.ForeColor = Theme.RatingColor(rating);
            e.CellStyle.Font = Theme.Bold;
        }
    }

    private void BuildDevelopersPage()
    {
        _tree.Dock = DockStyle.Fill;
        _tree.BorderStyle = BorderStyle.None;
        _tree.BackColor = Theme.Surface;
        _tree.ForeColor = Theme.Text;
        _tree.Font = new Font("Segoe UI", 11f);
        _tree.ItemHeight = 30;
        _tree.ShowLines = false;
        _developersPage.Controls.Add(_tree);
    }

    private void BuildGenresPage() => _genresPage.Controls.Add(_chart);

    // ---------- навигация и загрузка данных ----------

    private void ShowPage(Panel page)
    {
        foreach (var p in new[] { _gamesPage, _developersPage, _genresPage })
            p.Visible = p == page;

        if (page == _gamesPage) ReloadGames();
        else if (page == _developersPage) ReloadDevelopers();
        else _chart.SetData(_logic.AverageRatingByGenre());
    }

    private void ReloadGames() => _grid.DataSource = _logic.ReadAll().ToList();

    private void ReloadDevelopers()
    {
        _tree.BeginUpdate();
        _tree.Nodes.Clear();
        foreach (var pair in _logic.GroupByDeveloper().OrderBy(p => p.Key))
        {
            var node = _tree.Nodes.Add($"{pair.Key}  ({pair.Value.Count})");
            node.ForeColor = Theme.Accent2;
            foreach (var game in pair.Value)
            {
                var child = node.Nodes.Add($"{game.Title} — {game.Rating:0.0}");
                child.ForeColor = Theme.RatingColor(game.Rating);
            }
            node.Expand();
        }
        _tree.EndUpdate();
    }

    // ---------- действия пользователя ----------

    private VideoGame Selected => _grid.CurrentRow?.DataBoundItem as VideoGame;

    private void FillInputs()
    {
        var game = Selected;
        if (game == null) return;
        _title.Text = game.Title;
        _developer.Text = game.Developer;
        _genre.Text = game.Genre;
        _year.Text = game.ReleaseYear.ToString();
        _rating.Text = game.Rating.ToString();
    }

    private void ClearInputs()
    {
        foreach (var box in new[] { _title, _developer, _genre, _year, _rating })
            box.Clear();
    }

    private void Save(bool isNew)
    {
        if (string.IsNullOrWhiteSpace(_title.Text))
        {
            Warn("Введите название игры.");
            return;
        }
        if (!int.TryParse(_year.Text, out var year))
        {
            Warn("Год должен быть целым числом.");
            return;
        }
        if (!double.TryParse(_rating.Text, out var rating) || rating < 0 || rating > 10)
        {
            Warn("Оценка должна быть числом от 0 до 10.");
            return;
        }

        var game = new VideoGame
        {
            Title = _title.Text.Trim(),
            Developer = _developer.Text.Trim(),
            Genre = _genre.Text.Trim(),
            ReleaseYear = year,
            Rating = rating
        };

        if (isNew)
        {
            _logic.Create(game);
            ClearInputs();
        }
        else if (Selected != null)
        {
            game.Id = Selected.Id;
            _logic.Update(game);
        }
        else
        {
            Warn("Выберите игру в таблице.");
            return;
        }

        ReloadGames();
    }

    private void DeleteSelected()
    {
        var game = Selected;
        if (game == null) return;

        var answer = MessageBox.Show($"Удалить игру «{game.Title}»?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes) return;

        _logic.Delete(game.Id);
        ReloadGames();
    }

    private static void Warn(string message) =>
        MessageBox.Show(message, "Проверьте данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
