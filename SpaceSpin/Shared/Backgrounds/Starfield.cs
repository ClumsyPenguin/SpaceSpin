using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace SpaceSpin.Shared.Backgrounds
{
    public class Starfield : RenderableComponent
    {
        private struct Star
        {
            public Vector2 Position;
            public Color Color;
            public float Size;
        }

        private readonly List<Star> _stars = new();
        private const int StarCount = 300;

        public override float Width => Core.GraphicsDevice.Viewport.Width;
        public override float Height => Core.GraphicsDevice.Viewport.Height;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            SetRenderLayer(100); 

            for (var i = 0; i < StarCount; i++)
            {
                _stars.Add(new Star
                {
                    Position = new Vector2(Random.NextFloat(Width), Random.NextFloat(Height)),
                    Color = new Color(
                        0.5f + Random.NextFloat() * 0.5f,
                        0.5f + Random.NextFloat() * 0.5f,
                        0.5f + Random.NextFloat() * 0.5f),
                    Size = 1f + Random.NextFloat() * 2f
                });
            }
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            foreach (var star in _stars)
            {
                batcher.DrawPixel(star.Position, star.Color, (int)star.Size);
            }
        }
    }
}
