using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using RectangleF = Nez.RectangleF;
using SpaceSpin.Scenes;

namespace SpaceSpin.Components;

public class Enemy : RenderableComponent, IUpdatable
{
    private HexScene _hexScene;
    private Entity _target;

    private List<Point> _path;
    private int _pathIndex;

    private Point _currentCell;
    private bool _hasCell; 

    private const float Speed = 80f;
    private const float Size = 16f;

    public override void OnAddedToEntity()
    {
        base.OnAddedToEntity();

        _hexScene = Entity.Scene as HexScene;
        _target = Entity.Scene.FindEntity("hexagon");

        var health = Entity.AddComponent(new HealthComponent(100));
        health.OnDeath += _ => Entity.Destroy();

        Entity.AddComponent(new CircleCollider(Size * 0.6f));

        if (_hexScene is not null)
        {
            _currentCell = _hexScene.WorldToGrid(Entity.Position);
            _hasCell = true;
            RecalculatePath();
        }
    }

    public override RectangleF Bounds =>
        new(
            Entity.Position.X - Size,
            Entity.Position.Y - Size,
            Size * 2f,
            Size * 2f
        );

    public void Update()
    {
        if (_hexScene?.Graph == null || _target == null)
            return;

        var newCell = _hexScene.WorldToGrid(Entity.Position);
        if (!_hasCell || newCell != _currentCell)
        {
            _currentCell = newCell;
            _hasCell = true;
            RecalculatePath();
        }

        if (_path == null || _pathIndex >= _path.Count)
            return;

        var cell = _path[_pathIndex];
        var targetPos = _hexScene.GridToWorld(cell);

        var pos = Entity.Position;
        var to = targetPos - pos;
        var dist = to.Length();

        if (dist < 1f)
        {
            _pathIndex++;
        }
        else
        {
            var dir = to / dist;
            var step = Speed * Time.DeltaTime;
            if (step > dist) step = dist;
            Entity.Position += dir * step;
        }
        
        HandleCollisionWithTarget();
    }
    
    private void HandleCollisionWithTarget()
    {
        var enemyCollider = Entity.GetComponent<Collider>();
        if (enemyCollider is null || _target is null)
            return;

        var targetCollider = _target.GetComponent<Collider>();
        if (targetCollider is null)
            return;

        if (!enemyCollider.Overlaps(targetCollider))
            return;

        var enemyHealth  = Entity.GetComponent<HealthComponent>();
        var targetHealth = _target.GetComponent<HealthComponent>();

        if (enemyHealth == null || targetHealth == null)
            return;

        var damage = enemyHealth.CurrentHealth;

        targetHealth.TakeDamage(damage);
        enemyHealth.TakeDamage(damage);
    }

    private void RecalculatePath()
    {
        if (_hexScene is null || _target is null)
            return;

        var start = _currentCell;
        var goal  = _hexScene.WorldToGrid(_target.Position);

        var result = BreadthFirstPathfinder.Search(_hexScene.Graph, start, goal);

        if (result is null)
        {
            _path = null;
            _pathIndex = 0;
            return;
        }

        _path = new List<Point>(result);
        
        if (_path.Count > 0 && _path[0] == start)
            _pathIndex = 1;
        else
            _pathIndex = 0;
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        var pos = Entity.Position;

        var p0 = pos + new Vector2(-Size, -Size);
        var p1 = pos + new Vector2( Size, -Size);
        var p2 = pos + new Vector2( Size,  Size);
        var p3 = pos + new Vector2(-Size,  Size);

        batcher.DrawLine(p0, p1, Color.Red);
        batcher.DrawLine(p1, p2, Color.Red);
        batcher.DrawLine(p2, p3, Color.Red);
        batcher.DrawLine(p3, p0, Color.Red);

        // optional: draw its current path for debugging
        if (_hexScene != null && _path is { Count: > 1 })
        {
            for (var i = 0; i < _path.Count - 1; i++)
            {
                var a = _hexScene.GridToWorld(_path[i]);
                var b = _hexScene.GridToWorld(_path[i + 1]);
                batcher.DrawLine(a, b, Color.OrangeRed);
            }
        }
        
    }
}
