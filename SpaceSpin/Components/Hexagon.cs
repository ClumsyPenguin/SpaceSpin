using System;
using Microsoft.Xna.Framework;
using Nez;

namespace SpaceSpin.Components;

public class Hexagon : RenderableComponent, IUpdatable
{
    private readonly Vector2[] _points;
    private readonly float _radius;
    private float _rotation;

    private readonly Color _color = Color.White;

    public Hexagon(float radius)
    {
        _radius = radius;
        _points = new Vector2[6];

        for (var i = 0; i < 6; i++)
        {
            var angle = MathHelper.TwoPi / 6f * i - MathHelper.PiOver2;
            _points[i] = new Vector2(
                (float)Math.Cos(angle) * _radius,
                (float)Math.Sin(angle) * _radius
            );
        }
    }

    public override RectangleF Bounds =>
        new(Entity.Position.X - _radius,
            Entity.Position.Y - _radius,
            _radius * 2f,
            _radius * 2f
        );

    public void Update()
    {
        var center = Entity.Position;
        var mousePos = Input.ScaledMousePosition;
        var dir = mousePos - center;
        if (dir.LengthSquared() > 0.0001f)
            _rotation = (float)Math.Atan2(dir.Y, dir.X);

        if (Input.LeftMouseButtonPressed && dir.LengthSquared() > 0.0001f)
        {
            var dirNorm = Vector2.Normalize(dir);

            var spawnPos = center + dirNorm * _radius;

            SpawnProjectile(spawnPos, dirNorm);
        }
    }

    private void SpawnProjectile(Vector2 spawnPos, Vector2 direction)
    {
        const float speed = 400f;

        var projEntity = Entity.Scene.CreateEntity("projectile");
        projEntity.Position = spawnPos;
        
        projEntity.AddComponent(new Projectile(direction * speed, 20f));
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        var center = Entity.Position;

        var cos = (float)Math.Cos(_rotation);
        var sin = (float)Math.Sin(_rotation);

        for (var i = 0; i < 6; i++)
        {
            var p0 = _points[i];
            var p1 = _points[(i + 1) % 6];

            var a = new Vector2(
                p0.X * cos - p0.Y * sin,
                p0.X * sin + p0.Y * cos
            );

            var b = new Vector2(
                p1.X * cos - p1.Y * sin,
                p1.X * sin + p1.Y * cos
            );

            a += center;
            b += center;

            batcher.DrawLine(a, b, _color);
        }
    }
}