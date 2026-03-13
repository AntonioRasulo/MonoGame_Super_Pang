using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using Microsoft.Xna.Framework.Media;

namespace MonoGame_Super_Pang.Scenes;

public class TitleScene : Scene
{
    private const string MONOGAME_TEXT = "MonoGame";
    private const string SUPER_PANG_TEXT = "Super Pang";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";

    private const int STARTING_LEVEL = 0;

    // The font to use to render normal text.
    private SpriteFont _font;

    // The font used to render the title text.
    private SpriteFont _font5x;

    // The position to draw the monogame text at.
    private Vector2 _monogameTextPos;

    // The origin to set for the monogame text.
    private Vector2 _monogameTextOrigin;

    // The position to draw the super pang text at.
    private Vector2 _superpangTextPos;

    // The origin to set for the super pang text.
    private Vector2 _superpangTextOrigin;

    // The position to draw the press enter text at.
    private Vector2 _pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Set the position and origin for the Monogame text.
        Vector2 size = _font5x.MeasureString(MONOGAME_TEXT);
        _monogameTextPos = new Vector2(640, 100);
        _monogameTextOrigin = size * 0.5f;

        // Set the position and origin for the Super Pang text.
        size = _font5x.MeasureString(SUPER_PANG_TEXT);
        _superpangTextPos = new Vector2(757, 207);
        _superpangTextOrigin = size * 0.5f;

        // Set the position and origin for the press enter text.
        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _pressEnterPos = new Vector2(640, 620);
        _pressEnterOrigin = size * 0.5f;
    }

    public override void LoadContent()
    {
        // Load the font for the standard text.
        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

        // Load the font for the title text.
        _font5x = Content.Load<SpriteFont>("fonts/04B_30_5x");

        // Load the background theme music
        Song theme = Content.Load<Song>("audio/14. Traveling the Sky");

        // Ensure media player is not already playing on device, if so, stop it
        if (MediaPlayer.State == MediaState.Playing)
        {
            MediaPlayer.Stop();
        }

        // Play the background theme music.
        MediaPlayer.Play(theme);

        // Set the theme music to repeat.
        MediaPlayer.IsRepeating = true;
    }

    public override void Update(GameTime gameTime)
    {
        // If the user presses enter, switch to the game scene.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene(STARTING_LEVEL));
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // The color to use for the drop shadow text.
        Color dropShadowColor = Color.Black * 0.5f;

        // Draw the Dungeon text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(_font5x, MONOGAME_TEXT, _monogameTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _monogameTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the Dungeon text on top of that at its original position.
        Core.SpriteBatch.DrawString(_font5x, MONOGAME_TEXT, _monogameTextPos, Color.White, 0.0f, _monogameTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the Slime text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(_font5x, SUPER_PANG_TEXT, _superpangTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _superpangTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the Slime text on top of that at its original position.
        Core.SpriteBatch.DrawString(_font5x, SUPER_PANG_TEXT, _superpangTextPos, Color.White, 0.0f, _superpangTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the press enter text.
        Core.SpriteBatch.DrawString(_font, PRESS_ENTER_TEXT, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }

}
