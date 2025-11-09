using Microsoft.Xna.Framework;
using Nez;
using RectangleF = Nez.RectangleF;

namespace SpaceSpin.Components;


public class Projectile : RenderableComponent, IUpdatable
{
    private Vector2 _velocity;
    private readonly float _length;
    private readonly float _lifeTime;
    private float _age;

    public Projectile(Vector2 velocity, float length = 40f, float lifeTime = 2f)
    {
        _velocity = velocity;
        _length = length;
        _lifeTime = lifeTime;
    }

    public override RectangleF Bounds =>
        new(
            Entity.Position.X - _length,
            Entity.Position.Y - _length,
            _length * 2f,
            _length * 2f
        );

    public void Update()
    {
        Entity.Position += _velocity * Time.DeltaTime;

        _age += Time.DeltaTime;
        if (_age > _lifeTime)
            Entity.Destroy();
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        var start = Entity.Position;

        if (_velocity.LengthSquared() < 0.0001f)
            return;

        var dir = Vector2.Normalize(_velocity);
        var end = start + dir * _length;

        batcher.DrawLine(start, end, Color);
    }
}