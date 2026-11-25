using System.Drawing.Drawing2D;

namespace WinFormsView;

/// <summary>Столбчатая диаграмма средних оценок по жанрам (рисуется средствами GDI+).</summary>
public class RatingChart : Control
{
    private Dictionary<string, double> _data = new();

    public RatingChart()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    public void SetData(Dictionary<string, double> data)
    {
        _data = data ?? new Dictionary<string, double>();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Theme.Surface);

        TextRenderer.DrawText(g, "Средняя оценка по жанрам", Theme.Bold, new Point(16, 14), Theme.Accent);

        if (_data.Count == 0)
        {
            TextRenderer.DrawText(g, "Нет данных", Theme.Regular, ClientRectangle, Theme.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        const int left = 160, right = 80, top = 56, rowHeight = 46, barHeight = 24;
        int barMax = Math.Max(Width - left - right, 10);
        int y = top;

        foreach (var pair in _data.OrderByDescending(p => p.Value))
        {
            TextRenderer.DrawText(g, pair.Key, Theme.Regular, new Rectangle(16, y, left - 24, barHeight),
                Theme.Text, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            using (var trackBrush = new SolidBrush(Theme.Background))
                g.FillRectangle(trackBrush, new Rectangle(left, y, barMax, barHeight));

            int width = Math.Max((int)(barMax * Math.Clamp(pair.Value, 0, 10) / 10.0), 2);
            var bar = new Rectangle(left, y, width, barHeight);
            Color color = Theme.RatingColor(pair.Value);
            using (var brush = new LinearGradientBrush(bar, Color.FromArgb(120, color), color,
                       LinearGradientMode.Horizontal))
                g.FillRectangle(brush, bar);

            TextRenderer.DrawText(g, pair.Value.ToString("0.0"), Theme.Bold,
                new Rectangle(left + barMax + 8, y, right - 8, barHeight), color, TextFormatFlags.VerticalCenter);

            y += rowHeight;
        }
    }
}
