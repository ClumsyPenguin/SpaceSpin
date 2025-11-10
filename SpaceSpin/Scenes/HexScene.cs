using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using SpaceSpin.Components;

namespace SpaceSpin.Scenes;

public class HexScene : Scene
{
    public override void Initialize()
    {
        base.Initialize();

        ClearColor = Color.CornflowerBlue;

        var center = new Vector2(Screen.Width / 2f, Screen.Height / 2f);

        var font = Core.Content.Load<BitmapFont>("NezDefaultBMFont");

        var hexEntity = CreateEntity("hexagon");
        hexEntity.Position = center;

        hexEntity.AddComponent(new HealthComponent(500));

        var hex = hexEntity.AddComponent(new Hexagon(50f, font));
        hex.Color = Color.Yellow;
    }
}