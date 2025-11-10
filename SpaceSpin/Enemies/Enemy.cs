using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using SpaceSpin.Shared;
using SpaceSpin.Shared.Components;
using RectangleF = Nez.RectangleF;

namespace SpaceSpin.Enemies
{
    public class Enemy : RenderableComponent, IUpdatable
    {
        private Entity _target;
        private readonly BitmapFont _font;
        private readonly double _healthModifer;

        private const float Speed = 80f;
        private const float Size = 16f;
        
        private HealthComponent _health;
        
        public Enemy(BitmapFont font, double healthModifer = 1d)
        {
            _font = font;
            _healthModifer = healthModifer;
        }
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _target = Entity.Scene.FindEntity(ComponentRegister.Player);
            _health = Entity.GetComponent<HealthComponent>() ?? Entity.AddComponent(new HealthComponent((int)(100 * _healthModifer)));
            _health.OnDeath += _ => Entity.Destroy();
            
            Entity.AddComponent(new CircleCollider(Size * 0.6f));
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
            if (_target is null)
                return;

            var to = _target.Position - Entity.Position;
            var dist = to.Length();

            if (dist > 1f)
            {
                var dir = to / dist;
                Entity.Position += dir * Speed * Time.DeltaTime;
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

        public override void Render(Batcher batcher, Camera camera)
        {
            var pos = Entity.Position;

            var p0 = pos + new Vector2(-Size, -Size);
            var p1 = pos + new Vector2(Size, -Size);
            var p2 = pos + new Vector2(Size, Size);
            var p3 = pos + new Vector2(-Size, Size);

            batcher.DrawLine(p0, p1, Color.Red);
            batcher.DrawLine(p1, p2, Color.Red);
            batcher.DrawLine(p2, p3, Color.Red);
            batcher.DrawLine(p3, p0, Color.Red);
            
            if (_font is null || _health is null) 
                return;
        
            var text = _health.CurrentHealth.ToString();
            var size = _font.MeasureString(text);
            var textPos = pos - size / 2f;
            
            batcher.DrawString(_font, text, textPos, Color.White);
        }
    }
}