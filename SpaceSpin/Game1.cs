using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using SpaceSpin.Scenes;

namespace SpaceSpin;

public class Game1: Nez.Core
{
    public Game1() : base()
    {

    }
    
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        
        Scene = new GameScene();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }
}