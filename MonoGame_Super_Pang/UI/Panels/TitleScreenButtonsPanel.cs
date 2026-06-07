using Gum.DataTypes.Variables;
using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameGum.GueDeriving;
using Gum.Wireframe;

namespace MonoGame_Super_Pang.UI;

public class TitleScreenButtonsPanel : PangPanel
{

    private const string MONOGAME_TEXT = "MonoGame";
    private const string SUPER_PANG_TEXT = "Super";
    private const string PANG_TEXT = "Pang";

    // The position to draw the monogame text at.
    private Vector2 _monogameTextPos;

    // The origin to set for the monogame text.
    private Vector2 _monogameTextOrigin;

    // The position to draw the super pang text at.
    private Vector2 _superpangTextPos;

    // The origin to set for the super pang text.
    private Vector2 _superpangTextOrigin;

    // The position to draw the pang text at.
    private Vector2 _pangTextPos;

    // The origin to set for the pang text.
    private Vector2 _pangTextOrigin;

    private AnimatedButton _optionsButton;
    private AnimatedButton _startButton;
    private AnimatedButton _creditsButton;

    // The font used to render the title text.
    private SpriteFont _font5x;

    public TitleScreenButtonsPanel()
    {
        // Create a container to hold all of our buttons
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.AddToRoot();

        _volumeButton.Anchor(Gum.Wireframe.Anchor.TopRight);
        _panel.AddChild(_volumeButton);

        _creditsButton = new AnimatedButton(_GUIatlas);
        _creditsButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _creditsButton.Width = 50;
        _creditsButton.Text = "Credits";
        _creditsButton.Y = -12;
        _creditsButton.Click += TitlePanelManager.HandleCreditsClicked;
        _panel.AddChild(_creditsButton);

        _startButton = new AnimatedButton(_GUIatlas);
        _startButton.Anchor(Gum.Wireframe.Anchor.Bottom);
        _startButton.Y = -12;
        _startButton.Width = 50;
        _startButton.Text = "Start";
        _startButton.Click += TitlePanelManager.HandleStartClicked;
        _panel.AddChild(_startButton);

        _optionsButton = new AnimatedButton(_GUIatlas);
        _optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsButton.Y = -12;
        _optionsButton.Width = 50;
        _optionsButton.Text = "Options";
        _optionsButton.Click += TitlePanelManager.HandleOptionsClicked;
        _panel.AddChild(_optionsButton);

        // Load the font for the title text.
        _font5x = Core.Content.Load<SpriteFont>("fonts/04B_30_5x");

        // Set the position and origin for the Monogame text.
        Vector2 size = _font5x.MeasureString(MONOGAME_TEXT);
        _monogameTextPos = new Vector2(640, 100);
        _monogameTextOrigin = size * 0.5f;

        // Set the position and origin for the Super Pang text.
        size = _font5x.MeasureString(SUPER_PANG_TEXT);
        _superpangTextPos = new Vector2(757, 207);
        _superpangTextOrigin = size * 0.5f;

        size = _font5x.MeasureString(PANG_TEXT);
        _pangTextPos = new Vector2(874, 314);
        _pangTextOrigin = size * 0.5f;
    }

    public void SetStartButtonFocus(bool IsFocused)
    {
        _startButton.IsFocused = IsFocused;
    }

    public void Draw()
    {
        if(_panel.IsVisible)
        {
            // The color to use for the drop shadow text.
            Color dropShadowColor = Color.Black * 0.5f;

            // Draw the MONOGAME_TEXT text slightly offset from it is original position and
            // with a transparent color to give it a drop shadow.
            Core.SpriteBatch.DrawString(_font5x, MONOGAME_TEXT, _monogameTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _monogameTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the MONOGAME_TEXT text on top of that at its original position.
            Core.SpriteBatch.DrawString(_font5x, MONOGAME_TEXT, _monogameTextPos, Color.White, 0.0f, _monogameTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the SUPER_PANG_TEXT text slightly offset from it is original position and
            // with a transparent color to give it a drop shadow.
            Core.SpriteBatch.DrawString(_font5x, SUPER_PANG_TEXT, _superpangTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _superpangTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the SUPER_PANG_TEXT text on top of that at its original position.
            Core.SpriteBatch.DrawString(_font5x, SUPER_PANG_TEXT, _superpangTextPos, Color.White, 0.0f, _superpangTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the PANG_TEXT text slightly offset from it is original position and
            // with a transparent color to give it a drop shadow.
            Core.SpriteBatch.DrawString(_font5x, PANG_TEXT, _pangTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _pangTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the PANG_TEXT text on top of that at its original position.
            Core.SpriteBatch.DrawString(_font5x, PANG_TEXT, _pangTextPos, Color.White, 0.0f, _pangTextOrigin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }

    public new void SetIsVisible(bool isVisible)
    {
        base.SetIsVisible(isVisible);

        _creditsButton.IsFocused = !isVisible;
        _optionsButton.IsFocused = !isVisible;
        _startButton.IsFocused = isVisible;
    }

}