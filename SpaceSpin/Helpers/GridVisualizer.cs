namespace SpaceSpin.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using RectangleF = Nez.RectangleF;
using Scenes;

public class GridVisualizer : RenderableComponent
{
    private readonly HexScene _scene;

    public GridVisualizer(HexScene scene)
    {
        _scene = scene;
        RenderLayer = 1000;
    }

    public override RectangleF Bounds => new(0, 0, Screen.Width, Screen.Height);

    public override void Render(Batcher batcher, Camera camera)
    {
        if (_scene?.Graph == null)
            return;

        var cell = _scene.CellSize;

        var cols = (int)(Screen.Width / cell);
        var rows = (int)(Screen.Height / cell);

        
        for (var x = 0; x <= cols; x++)
        {
            var xPos = x * cell;
            var from = new Vector2(xPos, 0);
            var to   = new Vector2(xPos, rows * cell);
            batcher.DrawLine(from, to, Color.DarkSlateGray);
        }

        for (var y = 0; y <= rows; y++)
        {
            var yPos = y * cell;
            var from = new Vector2(0, yPos);
            var to   = new Vector2(cols * cell, yPos);
            batcher.DrawLine(from, to, Color.DarkSlateGray);
        }

        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                var center = new Vector2(
                    x * cell + cell / 2f,
                    y * cell + cell / 2f
                );
                const float half = 2f;
                batcher.DrawLine(center + new Vector2(-half, 0), center + new Vector2(half, 0), Color.Gray);
                batcher.DrawLine(center + new Vector2(0, -half), center + new Vector2(0, half), Color.Gray);
            }
        }
    }
}
