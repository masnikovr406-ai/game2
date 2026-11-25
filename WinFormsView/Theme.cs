namespace WinFormsView;

/// <summary>Цвета, шрифты и фабрики стилизованных элементов (тёмная тема).</summary>
public static class Theme
{
    public static readonly Color Background = Color.FromArgb(30, 30, 46);
    public static readonly Color Surface = Color.FromArgb(45, 45, 68);
    public static readonly Color RowAlt = Color.FromArgb(36, 36, 54);
    public static readonly Color Text = Color.FromArgb(205, 214, 244);
    public static readonly Color Muted = Color.FromArgb(147, 153, 178);
    public static readonly Color Accent = Color.FromArgb(137, 180, 250);
    public static readonly Color Accent2 = Color.FromArgb(203, 166, 247);
    public static readonly Color Good = Color.FromArgb(166, 227, 161);
    public static readonly Color Warn = Color.FromArgb(249, 226, 175);
    public static readonly Color Bad = Color.FromArgb(243, 139, 168);

    public static readonly Font Regular = new("Segoe UI", 10f);
    public static readonly Font Bold = new("Segoe UI Semibold", 10f);
    public static readonly Font Title = new("Segoe UI Semibold", 22f);

    public static Color RatingColor(double rating) =>
        rating >= 8 ? Good : rating >= 6 ? Warn : Bad;

    public static TextBox NewTextBox(string placeholder, int width) => new()
    {
        PlaceholderText = placeholder,
        Width = width,
        BorderStyle = BorderStyle.FixedSingle,
        BackColor = Background,
        ForeColor = Text,
        Font = Regular,
        Margin = new Padding(6)
    };

    public static Button NewButton(string text, EventHandler onClick, Color? color = null)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            FlatStyle = FlatStyle.Flat,
            BackColor = color ?? Accent,
            ForeColor = Background,
            Font = Bold,
            Cursor = Cursors.Hand,
            Padding = new Padding(10, 3, 10, 3),
            Margin = new Padding(6)
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += onClick;
        return button;
    }
}
