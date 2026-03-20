using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using Microsoft.Xna.Framework.Media;
using Gum.Forms.Controls;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGame_Super_Pang.UI;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoGame_Super_Pang.Backgrounds;
using MonoGame_Super_Pang.Config;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Scenes;

public class TitleScene : Scene
{
    private Panel _titleScreenButtonsPanel;
    private Panel _optionsPanel;

    // The options button used to open the options menu.
    private AnimatedButton _optionsButton;

    // The back button used to exit the options menu back to the title menu.
    private AnimatedButton _optionsBackButton;
    private OptionsSlider sfxSlider;
    private OptionsSlider musicSlider;

    private SoundEffect _uiSoundEffect;

    private const string MONOGAME_TEXT = "MonoGame";
    private const string SUPER_PANG_TEXT = "Super Pang";

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

    // Reference to the texture atlas that we can pass to UI elements when they
    // are created.
    private TextureAtlas _GUIatlas;

    bool _isLastFocusedBackButton = false;

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

        // Load the sound effect to play when ui actions occur.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/Confirm 1");

        // Load the texture atlas from the xml configuration file.
        _GUIatlas = TextureAtlas.FromFile(Content, "images/GUI_atlas.xml");

        _backgroundRand = new Random();
        int backgroundIndex = _backgroundRand.Next(0, LevelRegistry.AllLevels.Count);

        List<string> backgroundList = LevelRegistry.AllLevels[backgroundIndex].backgroundStr;
        List<Texture2D> clouds = new List<Texture2D>();

        foreach(string backgroundStr in backgroundList)
        {
            clouds.Add(Content.Load<Texture2D>(backgroundStr));
        }

        _levelBackground = new Background(clouds);
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);

        _levelBackground.Update(gameTime);

        if(_optionsBackButton.IsFocused == false &&
        sfxSlider.IsFocused == false &&
        musicSlider.IsFocused == false && _optionsPanel.IsVisible == true)
        {
            if (_isLastFocusedBackButton)
            {
                musicSlider.IsFocused = true;
            }
            else
            {
                _optionsBackButton.IsFocused = true;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        if (_titleScreenButtonsPanel.IsVisible)
        {
            // Draw the background
            _levelBackground.Draw();

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

        CreateTitlePanel();
        CreateOptionsPanel();
    }

    private void CreateTitlePanel()
    {
        // Create a container to hold all of our buttons
        _titleScreenButtonsPanel = new Panel();
        _titleScreenButtonsPanel.Dock(Gum.Wireframe.Dock.Fill);
        _titleScreenButtonsPanel.AddToRoot();

        var startButton = new AnimatedButton(_GUIatlas);
        startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        startButton.X = 50;
        startButton.Y = -12;
        startButton.Width = 70;
        startButton.Text = "Start";
        startButton.Click += HandleStartClicked;
        _titleScreenButtonsPanel.AddChild(startButton);

        _optionsButton = new AnimatedButton(_GUIatlas);
        _optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsButton.X = -50;
        _optionsButton.Y = -12;
        _optionsButton.Width = 70;
        _optionsButton.Text = "Options";
        _optionsButton.Click += HandleOptionsClicked;
        _titleScreenButtonsPanel.AddChild(_optionsButton);

        startButton.IsFocused = true;
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Change to the game scene to start the game.
        Core.ChangeScene(new GameScene(STARTING_LEVEL));
    }

    private void HandleOptionsClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.IsVisible = false;

        // Set the options panel to be visible.
        _optionsPanel.IsVisible = true;

        // Give the back button on the options panel focus.
        _optionsBackButton.IsFocused = true;
    }

    private void CreateOptionsPanel()
    {
        _optionsPanel = new Panel();
        _optionsPanel.Dock(Gum.Wireframe.Dock.Fill);
        _optionsPanel.IsVisible = false;
        _optionsPanel.AddToRoot();

        TextRuntime optionsText = new TextRuntime();
        optionsText.X = 10;
        optionsText.Y = 10;
        optionsText.Text = "OPTIONS";
        optionsText.UseCustomFont = true;
        optionsText.FontScale = 0.5f;
        optionsText.CustomFontFile = @"fonts/04b_30.fnt";
        optionsText.IsEnabled = false;
        _optionsPanel.AddChild(optionsText);

        musicSlider = new OptionsSlider(_GUIatlas);
        musicSlider.Name = "MusicSlider";
        musicSlider.Text = "MUSIC";
        musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
        musicSlider.Y = 30f;
        musicSlider.Minimum = 0;
        musicSlider.Maximum = 1;
        musicSlider.Value = Core.Audio.SongVolume;
        musicSlider.SmallChange = .1;
        musicSlider.LargeChange = .2;
        musicSlider.ValueChanged += HandleMusicSliderValueChanged;
        musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
        musicSlider.KeyDown += updateFlagButton;
        _optionsPanel.AddChild(musicSlider);

        sfxSlider = new OptionsSlider(_GUIatlas);
        sfxSlider.Name = "SfxSlider";
        sfxSlider.Text = "SFX";
        sfxSlider.Anchor(Gum.Wireframe.Anchor.Top);
        sfxSlider.Y = 93;
        sfxSlider.Minimum = 0;
        sfxSlider.Maximum = 1;
        sfxSlider.Value = Core.Audio.SoundEffectVolume;
        sfxSlider.SmallChange = .1;
        sfxSlider.LargeChange = .2;
        sfxSlider.ValueChanged += HandleSfxSliderChanged;
        sfxSlider.ValueChangeCompleted += HandleSfxSliderChangeCompleted;
        _optionsPanel.AddChild(sfxSlider);

        _optionsBackButton = new AnimatedButton(_GUIatlas);
        _optionsBackButton.Text = "BACK";
        _optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsBackButton.X = -28f;
        _optionsBackButton.Y = -10f;
        _optionsBackButton.Click += HandleOptionsButtonBack;
        _optionsBackButton.KeyDown += updateFlagButton;
        _optionsPanel.AddChild(_optionsBackButton);
    }

    private void updateFlagButton(Object sender, KeyEventArgs e)
    {
        if(sender == musicSlider)
        {
            if (e.Key == Keys.Up)
            {
                _isLastFocusedBackButton = false;
            }
        }
        else if(sender == _optionsBackButton)
        {
            if (e.Key == Keys.Down || e.Key == Keys.Right)
            {
                _isLastFocusedBackButton = true;
            }
        }

    }

    private void HandleMusicSliderValueChanged(object sender, EventArgs args)
    {
        // Intentionally not playing the UI sound effect here so that it is not
        // constantly triggered as the user adjusts the slider's thumb on the
        // track.

        // Get a reference to the sender as a Slider.
        var slider = (Slider)sender;

        // Set the global song volume to the value of the slider.
        Core.Audio.SongVolume = (float)slider.Value;
    }

    private void HandleMusicSliderValueChangeCompleted(object sender, EventArgs args)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);
    }

    private void HandleSfxSliderChanged(object sender, EventArgs args)
    {
        // Intentionally not playing the UI sound effect here so that it is not
        // constantly triggered as the user adjusts the slider's thumb on the
        // track.

        // Get a reference to the sender as a Slider.
        var slider = (Slider)sender;

        // Set the global sound effect volume to the value of the slider.;
        Core.Audio.SoundEffectVolume = (float)slider.Value;
    }

    private void HandleSfxSliderChangeCompleted(object sender, EventArgs e)
    {
        // Play the UI Sound effect so the player can hear the difference in audio.
        Core.Audio.PlaySoundEffect(_uiSoundEffect);
    }

    private void HandleOptionsButtonBack(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be visible.
        _titleScreenButtonsPanel.IsVisible = true;

        // Set the options panel to be invisible.
        _optionsPanel.IsVisible = false;

        // Give the options button on the title panel focus since we are coming
        // back from the options screen.
        _optionsButton.IsFocused = true;
    }

}
