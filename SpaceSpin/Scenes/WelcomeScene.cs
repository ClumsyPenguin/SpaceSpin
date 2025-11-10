using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace SpaceSpin.Scenes
{
    public class WelcomeScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Black;

            var canvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());
            canvas.IsFullScreen = true;

            var table = canvas.Stage.AddElement(new Table());
            table.SetFillParent(true);

            var titleStyle = new LabelStyle(Color.White);
            var title = new Label("SpaceSpin", titleStyle);
            table.Add(title).SetPadBottom(50);
            table.Row();

            var buttonStyle = TextButtonStyle.Create(Color.Black, Color.White, Color.Gray);
            var startButton = new TextButton("Start Game", buttonStyle);
            table.Add(startButton);

            startButton.OnClicked += button =>
            {
                Core.StartSceneTransition(new FadeTransition(() => new GameScene()));
            };
        }
    }
}
