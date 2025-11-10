using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using SpaceSpin.Shared.Backgrounds;

namespace SpaceSpin.Scenes
{
    public class WelcomeScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Black;

            CreateEntity("starfield").AddComponent(new Starfield());

            var canvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());
            canvas.IsFullScreen = true;

            var table = canvas.Stage.AddElement(new Table());
            table.SetFillParent(true);

            var titleStyle = new LabelStyle(Color.White);
            var title = new Label("SpaceSpin", titleStyle);
            title.SetFontScale(3f);
            table.Add(title).SetPadTop(200).SetPadBottom(50).SetColspan(3);
            table.Row();

            var buttonStyle = TextButtonStyle.Create(Color.Black, Color.White, Color.Gray);
            var startButton = new TextButton("Start Game", buttonStyle);
            startButton.GetLabel().SetFontScale(2f);
            table.Add(startButton).SetColspan(3);

            table.Row();
            table.Add().SetExpandY();
            table.Row();

            var footerStyle = new LabelStyle(Color.White);
            var creatorLabel = new Label("Created by ClumsyPenguin", footerStyle);
            var versionLabel = new Label("v0.0.1", footerStyle);

            creatorLabel.SetFontScale(1.2f);
            versionLabel.SetFontScale(1.2f);
            
            table.Add(creatorLabel).Left().SetPadLeft(10).SetPadBottom(10);
            table.Add().SetExpandX();
            table.Add(versionLabel).Right().SetPadRight(10).SetPadBottom(10);

            startButton.OnClicked += button =>
            {
                Core.StartSceneTransition(new FadeTransition(() => new GameScene()));
            };
        }
    }
}
