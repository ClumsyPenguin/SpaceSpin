using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using Nez.BitmapFonts;
using SpaceSpin.Components;
using SpaceSpin.Helpers;

namespace SpaceSpin.Scenes;

public class HexScene : Scene
{
    public UnweightedGraph<Point> Graph;
    public float CellSize = 40f;

    private int _cols;
    private int _rows;

    public override void Initialize()
    {
        base.Initialize();

        ClearColor = Color.CornflowerBlue;

        CreateGridGraph();
        
        var gridEntity = CreateEntity("grid-visualizer");
        gridEntity.AddComponent(new GridVisualizer(this));

        var center = new Vector2(Screen.Width / 2f, Screen.Height / 2f);
        var font = Core.Content.Load<BitmapFont>("NezDefaultBMFont");

        var hexEntity = CreateEntity("hexagon");
        hexEntity.Position = center;
        hexEntity.AddComponent(new HealthComponent(500));

        hexEntity.AddComponent(new CircleCollider(50f));
        var hex = hexEntity.AddComponent(new Hexagon(50f, font));
        hex.Color = Color.Yellow;

        var enemyEntity = CreateEntity("enemy");
        enemyEntity.Position = new Vector2(100f, 100f);
        enemyEntity.AddComponent(new Enemy());
        
        var enemyEntity2 = CreateEntity("enemy");
        enemyEntity2.Position = new Vector2(200f, 800f);
        enemyEntity2.AddComponent(new Enemy());
    }

    private void CreateGridGraph()
    {
        if (CellSize <= 0f)
            CellSize = 40f;

        _cols = (int)(Screen.Width / CellSize);
        _rows = (int)(Screen.Height / CellSize);

        if (_cols <= 0) _cols = 1;
        if (_rows <= 0) _rows = 1;

        Graph = new UnweightedGraph<Point>();

        for (var y = 0; y < _rows; y++)
        {
            for (var x = 0; x < _cols; x++)
            {
                var node = new Point(x, y);
                var neighbors = new List<Point>();

                if (x > 0 && y > 0)                        neighbors.Add(new Point(x - 1, y - 1)); // NW
                if (x < _cols - 1 && y > 0)                neighbors.Add(new Point(x + 1, y - 1)); // NE
                if (x > 0 && y < _rows - 1)                neighbors.Add(new Point(x - 1, y + 1)); // SW
                if (x < _cols - 1 && y < _rows - 1)        neighbors.Add(new Point(x + 1, y + 1)); // SE

                if (x > 0)                                 neighbors.Add(new Point(x - 1, y));     // W
                if (x < _cols - 1)                         neighbors.Add(new Point(x + 1, y));     // E
                if (y > 0)                                 neighbors.Add(new Point(x, y - 1));     // N
                if (y < _rows - 1)                         neighbors.Add(new Point(x, y + 1));     // S

                Graph.AddEdgesForNode(node, neighbors.ToArray());
            }
        }
    }

    public Point WorldToGrid(Vector2 worldPos)
    {
        var gx = (int)(worldPos.X / CellSize);
        var gy = (int)(worldPos.Y / CellSize);

        gx = MathHelper.Clamp(gx, 0, _cols - 1);
        gy = MathHelper.Clamp(gy, 0, _rows - 1);

        return new Point(gx, gy);
    }

    public Vector2 GridToWorld(Point cell)
    {
        return new Vector2(
            cell.X * CellSize + CellSize / 2f,
            cell.Y * CellSize + CellSize / 2f
        );
    }
}