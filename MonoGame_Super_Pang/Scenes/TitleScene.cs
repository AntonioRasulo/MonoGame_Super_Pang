using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGame_Super_Pang.UI;
using MonoGame_Super_Pang.Backgrounds;
using MonoGame_Super_Pang.Config;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Scenes;

public class TitleScene : Scene
{
    private const string MONOGAME_TEXT = "MonoGame";
    private const string SUPER_PANG_TEXT = "Super";
    private const string PANG_TEXT = "Pang";

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

    // The position to draw the pang text at.
    private Vector2 _pangTextPos;

    // The origin to set for the pang text.
    private Vector2 _pangTextOrigin;

    private Background _levelBackground;

    private Random _backgroundRand;

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

        size = _font5x.MeasureString(PANG_TEXT);
        _pangTextPos = new Vector2(874, 314);
        _pangTextOrigin = size * 0.5f;

        Core.Audio.SongVolume = 0.5f;
        Core.Audio.SoundEffectVolume = 0.5f;

        InitializeUI();
    }

    public override void LoadContent()
    {
        // Load the font for the standard text.
        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

        // Load the font for the title text.
        _font5x = Content.Load<SpriteFont>("fonts/04B_30_5x");

        // Load the background theme music
        Song theme = Content.Load<Song>("audio/14. Traveling the Sky");
        Core.Audio.PlaySong(theme);

        _backgroundRand = new Random();
        int backgroundIndex = _backgroundRand.Next(0, LevelRegistry.AllLevels.Count);

        List<string> backgroundList = LevelRegistry.AllLevels[backgroundIndex].backgroundStr;
        List<Texture2D> clouds = new List<Texture2D>();

        foreach(string backgroundStr in backgroundList)
        {
            clouds.Add(Content.Load<Texture2D>(backgroundStr));
        }

        _levelBackground = new Background(clouds);

        PlayerStatsManager.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);

        _levelBackground.Update(gameTime);

        TitlePanelManager.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Draw the background
        _levelBackground.Draw();

        if (TitlePanelManager.IsTitlePanelVisible())
        {
            // Begin the sprite batch to prepare for rendering.
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

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

            // Always end the sprite batch when finished.
            Core.SpriteBatch.End();
        }

        GumService.Default.Draw();
    }

    private void InitializeUI()
    {
        // Clear out any previous UI in case we came here from
        // a different screen:
        GumService.Default.Root.Children.Clear();

        // Create the directory if it doesn't exist yet
        Directory.CreateDirectory(PlayerStatsManager.saveDirectory);

        TitlePanelManager.LoadContent();
    }

}