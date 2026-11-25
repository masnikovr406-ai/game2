using System.Drawing.Drawing2D;

namespace WinFormsView;

/// <summary>Шапка окна: градиент, нарисованный в коде геймпад и заголовок.</summary>
public class GradientHeader : Panel
{
    public GradientHeader() => DoubleBuffered = true;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (Width <= 0 || Height <= 0) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var brush = new LinearGradientBrush(ClientRectangle, Theme.Accent2, Theme.Accent,
                   LinearGradientMode.Horizontal))
            g.FillRectangle(brush, ClientRectangle);

        DrawGamepad(g, 24, 20);
        TextRenderer.DrawText(g, "Каталог видеоигр", Theme.Title, new Point(110, 8), Theme.Background);
        TextRenderer.DrawText(g, "Ваша коллекция игр", Theme.Regular, new Point(114, 50), Theme.Background);
    }

    private static void DrawGamepad(Graphics g, int x, int y)
    {
        using var body = RoundedRectangle(new Rectangle(x, y, 72, 42), 16);
        using var bodyBrush = new SolidBrush(Theme.Background);
        using var light = new SolidBrush(Theme.Text);
        using var green = new SolidBrush(Theme.Good);
        using var red = new SolidBrush(Theme.Bad);

        g.FillPath(bodyBrush, body);
        g.FillRectangle(light, x + 14, y + 19, 18, 5);
        g.FillRectangle(light, x + 20, y + 13, 5, 17);
        g.FillEllipse(green, x + 46, y + 13, 9, 9);
        g.FillEllipse(red, x + 55, y + 22, 9, 9);
    }

    private static GraphicsPath RoundedRectangle(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
