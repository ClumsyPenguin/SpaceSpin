using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using SpaceSpin.Scenes;

namespace SpaceSpin.Shared.UI;

public class WaveUI : RenderableComponent
{
    private readonly GameScene _scene;

    public override float Width => Screen.Width;
    public override float Height => Screen.Height;

    public WaveUI(GameScene scene)
    {
        _scene = scene;
        RenderLayer = 1000; 
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        if (_scene.IsIntermission())
        {
            var font = Core.Content.Load<BitmapFont>(ComponentRegister.DefaultFont);
            var currentWaveText = $"Current wave: {_scene.CurrentWave}";
            var currentWavePos = new Vector2((Screen.Width - font.MeasureString(currentWaveText).X) / 2, 25);
            
            var text = $"Next wave in: {_scene.GetWaveCountdownTimer():F1}s";
            var pos = new Vector2((Screen.Width - font.MeasureString(text).X) / 2, 50);
            
            batcher.DrawString(font, text, pos, Color.White);
            batcher.DrawString(font, currentWaveText, currentWavePos, Color.White);
        }
    }
}
