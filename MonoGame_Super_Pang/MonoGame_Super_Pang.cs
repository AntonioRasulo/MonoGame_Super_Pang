using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang;

public class Game1 : Core
{
    private Sprite _character;

    // Tracks the position of the character.
    private Vector2 _characterPosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    public Game1() : base("Monogame Super Pang", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {

        // Create the texture atlas from the XML configuration file
        TextureAtlas characterAtlas = TextureAtlas.FromFile(Content, "images/character_atlas.xml");

        // retrieve the slime region from the atlas.
        _character = characterAtlas.CreateSprite("characterShootingUp");
        _character.Scale = new Vector2(4.0f, 4.0f);

        _characterPosition = new Vector2(    // position
            Window.ClientBounds.Width* 0.5f, 
            Window.ClientBounds.Height-_character.Height);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Check for keyboard input and handle it.
        CheckKeyboardInput();

        // Check for gamepad input and handle it.
        CheckGamePadInput();

        base.Update(gameTime);
    }

    private void CheckKeyboardInput()
    {
        // Get the state of keyboard input
        KeyboardState keyboardState = Keyboard.GetState();

        // If the space key is held down, the movement speed increases by 1.5
        float speed = MOVEMENT_SPEED;
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        // If the A or Left keys are down, move the character left on the screen.
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            _characterPosition.X -= speed;
        }

        // If the D or Right keys are down, move the character right on the screen.
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            _characterPosition.X += speed;
        }
    }

    private void CheckGamePadInput()
    {
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

        // If the A button is held down, the movement speed increases by 1.5
        // and the gamepad vibrates as feedback to the player.
        float speed = MOVEMENT_SPEED;
        if (gamePadState.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        // Check thumbstick first since it has priority over which gamepad input
        // is movement.  It has priority since the thumbstick values provide a
        // more granular analog value that can be used for movement.
        if (gamePadState.ThumbSticks.Left != Vector2.Zero)
        {
            _characterPosition.X += gamePadState.ThumbSticks.Left.X * speed;
        }
        else
        {

            // If DPapLeft is down, move the character left on the screen.
            if (gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                _characterPosition.X -= speed;
            }

            // If DPadRight is down, move the character right on the screen.
            if (gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                _characterPosition.X += speed;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Vector2 initialPosition = new Vector2(    // position
            Window.ClientBounds.Width* 0.5f, 
            Window.ClientBounds.Height-_character.Height);

        // Draw the slime texture region at a scale of 4.0
        //_character.Draw(SpriteBatch, initialPosition);
        _character.Draw(SpriteBatch, _characterPosition);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
