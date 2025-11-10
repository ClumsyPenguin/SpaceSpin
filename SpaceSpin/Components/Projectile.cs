using Microsoft.Xna.Framework;
using Nez;
using RectangleF = Nez.RectangleF;

namespace SpaceSpin.Components;


public class Projectile : RenderableComponent, IUpdatable
{
    private Vector2 _velocity;
    private readonly float _length;
    private readonly float _lifeTime;
    private readonly int _damage;
    private float _age;
    private readonly Entity _owner;

    public Projectile(Vector2 velocity, float length = 40f, float lifeTime = 2f, int damage = 25, Entity owner = null)
    {
        _velocity = velocity;
        _length = length;
        _lifeTime = lifeTime;
        _damage = damage;
        _owner = owner;
    }
    
    public override void OnAddedToEntity()
    {
        base.OnAddedToEntity();

        Entity.AddComponent(new CircleCollider(_length * 0.25f));
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
        {
            Entity.Destroy();
            return;
        }

        var collider = Entity.GetComponent<Collider>();
        if (collider == null)
            return;

        var neighbors = Physics.BoxcastBroadphaseExcludingSelf(collider);

        foreach (var other in neighbors)
        {
            if (other.Entity == Entity)
                continue;

            if (_owner != null && other.Entity == _owner)
                continue;

            if (!collider.Overlaps(other))
                continue;

            var health = other.Entity.GetComponent<HealthComponent>();
            if (health is not null)
            {
                health.TakeDamage(_damage);
                Entity.Destroy();
                break;
            }
        }
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