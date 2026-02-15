using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.GameObjects;

namespace MonoGame_Super_Pang;

public class Game1 : Core
{

    TextureAtlas characterAtlas;

    private Character _character;
    public Game1() : base("Monogame Super Pang", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        _character.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);

    }

    protected override void LoadContent()
    {

        // Create the texture atlas from the XML configuration file
        characterAtlas = TextureAtlas.FromFile(Content, "images/character_atlas.xml");

        // Retrieve regions and animations from the atlas
        Sprite idleRegion = characterAtlas.CreateSprite("characterStanding");
        AnimatedSprite walkAnimation = characterAtlas.CreateAnimatedSprite("walk-animation");
        AnimatedSprite shootAnimation = characterAtlas.CreateAnimatedSprite("shooting-animation");

        _character = new Character(idleRegion, walkAnimation, shootAnimation);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _character.Update(gameTime);

        // Create a bounding rectangle for the screen.
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Getting the bounding rectangle for the character
        Rectangle characterBounds = _character.getBounds();

        Vector2 newCharPosition = _character._characterPosition;

        // Use distance based checks to determine if the character is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // move it back inside.
        if (characterBounds.Left < screenBounds.Left)
        {
            newCharPosition.X = screenBounds.Left;
            _character._characterPosition = newCharPosition;
        }
        else if (characterBounds.Right > screenBounds.Right)
        {
            newCharPosition.X = screenBounds.Right - _character.getWidth();
            _character._characterPosition = newCharPosition;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the character
        _character.Draw(SpriteBatch);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
