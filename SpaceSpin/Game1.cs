using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using SpaceSpin.Scenes;

namespace SpaceSpin;

public class Game1: Core
{
    public Game1() : base()
    {

    }
    
    protected override void Initialize()
    {
        base.Initialize();
        
        Scene = new GameScene();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}