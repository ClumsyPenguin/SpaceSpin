using Microsoft.Xna.Framework;
using Nez;
using SpaceSpin.Components;
using Vector2 = System.Numerics.Vector2;

namespace SpaceSpin.Scenes;

public class HexScene : Scene
{
    public override void OnStart()
    {
        base.OnStart();

        ClearColor = Color.Black;
        var center = new Vector2(Screen.Width / 2f, Screen.Height / 2f);

        var hexEntity = CreateEntity("hexagon");
        hexEntity.Position = center;

        hexEntity.AddComponent(new Hexagon(50f));
    }
}