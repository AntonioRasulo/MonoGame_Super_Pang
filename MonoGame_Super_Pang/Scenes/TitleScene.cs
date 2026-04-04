using System;
using System.IO;
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
    private Panel _loadGamePanel;
    private Panel _newGamePanel;

    // The options button used to open the options menu.
    private AnimatedButton _optionsButton;

    // The back button used to exit the options menu back to the title menu.
    private AnimatedButton _optionsBackButton;
    private AnimatedButton _loadBackButton;
    private AnimatedButton _newGameBackButton;
    private OptionsSlider sfxSlider;
    private OptionsSlider musicSlider;

    private SoundEffect _uiSoundEffect;

    private const string MONOGAME_TEXT = "MonoGame";
    private const string SUPER_PANG_TEXT = "Super";
    private const string PANG_TEXT = "Pang";

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

    // The position to draw the pang text at.
    private Vector2 _pangTextPos;

    // The origin to set for the pang text.
    private Vector2 _pangTextOrigin;

    // Reference to the texture atlas that we can pass to UI elements when they
    // are created.
    private TextureAtlas _GUIatlas;

    bool _isLastFocusedBackButton = false;
    bool _isLastFocusedLoadBackButton = false;

    private Background _levelBackground;

    private Random _backgroundRand;

    private TextureRegion _loadGamePaperRegion;

    private static string _saveDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MonoGame_Super_Pang",
        "saves"
    );

    private readonly string PATH1 = _saveDirectory + "/pStats1.json";
    private readonly string PATH2 = _saveDirectory + "/pStats2.json";
    private readonly string PATH3 = _saveDirectory + "/pStats3.json";

    private PlayerStats pStats1;
    private PlayerStats pStats2;
    private PlayerStats pStats3;

    TextureButton _loadButton1;
    TextureButton _loadButton2;
    TextureButton _loadButton3;
    TextureButton _newGameButton;

    TextBox _newGameNametextBox;

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

        // Load the sound effect to play when ui actions occur.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/Confirm 1");

        // Load the texture atlas from the xml configuration file.
        _GUIatlas = TextureAtlas.FromFile(Content, "images/GUI_atlas.xml");

        TextureAtlas book2Atlas = TextureAtlas.FromFile(Content, "images/UI/Book2_atlas.xml");

        _loadGamePaperRegion = book2Atlas.GetRegion("paper-tile-9");

        _backgroundRand = new Random();
        int backgroundIndex = _backgroundRand.Next(0, LevelRegistry.AllLevels.Count);

        List<string> backgroundList = LevelRegistry.AllLevels[backgroundIndex].backgroundStr;
        List<Texture2D> clouds = new List<Texture2D>();

        foreach(string backgroundStr in backgroundList)
        {
            clouds.Add(Content.Load<Texture2D>(backgroundStr));
        }

        _levelBackground = new Background(clouds);

        pStats1 = PlayerStats.LoadGame(PATH1);
        pStats2 = PlayerStats.LoadGame(PATH2);
        pStats3 = PlayerStats.LoadGame(PATH3);
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);

        _levelBackground.Update(gameTime);

        if(_optionsBackButton.IsFocused == false &&
        sfxSlider.IsFocused == false &&
        musicSlider.IsFocused == false &&
        _optionsPanel.IsVisible == true)
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

        if(_loadBackButton.IsFocused == false &&
        _loadButton1.IsFocused == false &&
        _loadButton2.IsFocused == false &&
        _loadButton3.IsFocused == false &&
        _loadGamePanel.IsVisible == true)
        {
            if (_isLastFocusedLoadBackButton)
            {
                _loadButton3.IsFocused = true;
            }
            else
            {
                _loadBackButton.IsFocused = true;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Draw the background
        _levelBackground.Draw();

        if (_titleScreenButtonsPanel.IsVisible)
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
        Directory.CreateDirectory(_saveDirectory);

        CreateTitlePanel();
        CreateOptionsPanel();
        CreateLoadGamePanel();
        CreateNewGamePanel();
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

    private void HandleLoadButton(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        PlayerStats pStats = null;

        //TODO load game implementation
        if(sender == _loadButton1)
        {
            if(_loadButton1.isNewGame)
            {
                HandleNewGameClicked(_loadButton1);
                return;
            }
            
            pStats = pStats1;
        }

        if(sender == _loadButton2)
        {
            if(_loadButton2.isNewGame)
            {
                HandleNewGameClicked(_loadButton2);
                return;
            }
            pStats = pStats2;
        }

        if(sender == _loadButton3)
        {
            if (_loadButton3.isNewGame)
            {
                HandleNewGameClicked(_loadButton3);
                return;
            }
            pStats = pStats3;
        }

        // Change to the game scene to start the game.
        Core.ChangeScene(new GameScene(STARTING_LEVEL, pStats));
    }

    private void HandleOptionsClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.IsVisible = false;

        _loadGamePanel.IsVisible = false;

        _newGamePanel.IsVisible = false;

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

    private void CreateNewGamePanel()
    {
        _newGamePanel = new Panel();
        _newGamePanel.Dock(Gum.Wireframe.Dock.Fill);
        _newGamePanel.IsVisible = false;
        _newGamePanel.AddToRoot();

        _newGameNametextBox = new TextBox();
        _newGameNametextBox.Width = 200;
        _newGameNametextBox.Anchor(Gum.Wireframe.Anchor.Center);
        _newGameNametextBox.Placeholder = "";
        _newGamePanel.AddChild(_newGameNametextBox);

        AnimatedButton confirmButton = new AnimatedButton(_GUIatlas);
        confirmButton.Text = "CONFIRM";
        confirmButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        confirmButton.X = 28f;
        confirmButton.Y = -10f;
        confirmButton.Click += handleConfirmNameClicked;
        _newGamePanel.AddChild(confirmButton);

        _newGameBackButton = new AnimatedButton(_GUIatlas);
        _newGameBackButton.Text = "BACK";
        _newGameBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _newGameBackButton.X = -28f;
        _newGameBackButton.Y = -10f;
        _newGameBackButton.Click += HandleStartClicked;
        _newGamePanel.AddChild(_newGameBackButton);
    }

    private void CreateLoadGamePanel()
    {
        _loadGamePanel = new Panel();
        _loadGamePanel.Dock(Gum.Wireframe.Dock.Fill);
        _loadGamePanel.IsVisible = false;
        _loadGamePanel.AddToRoot();

        float screenHeight = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        float screenWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;

        _loadButton1 = new(_loadGamePaperRegion.Texture, _loadGamePaperRegion.SourceRectangle);
        _loadButton1.Click += HandleLoadButton;
        _loadButton1.Anchor(Gum.Wireframe.Anchor.Left);
        _loadButton1.X = 25;
        _loadButton1.Y = 0;
        _loadButton1.SetScale(1.8f);

        if(pStats1 != null)
        {
            _loadButton1.Text = pStats1.Name;
            _loadButton1.setTextMoney(pStats1.Money.ToString());
            _loadButton1.isNewGame = false;
        }

        _loadButton2 = new(_loadGamePaperRegion.Texture, _loadGamePaperRegion.SourceRectangle);
        _loadButton2.Anchor(Gum.Wireframe.Anchor.Center);
        _loadButton2.Click += HandleLoadButton;
        _loadButton2.X = 0;
        _loadButton2.Y = 0;
        _loadButton2.SetScale(1.8f);

        if(pStats2 != null)
        {
            _loadButton2.Text = pStats2.Name;
            _loadButton2.setTextMoney(pStats2.Money.ToString());
            _loadButton2.isNewGame = false;
        }

        _loadButton3 = new(_loadGamePaperRegion.Texture, _loadGamePaperRegion.SourceRectangle);
        _loadButton3.Anchor(Gum.Wireframe.Anchor.Right);
        _loadButton3.Click += HandleLoadButton;
        _loadButton3.X = -25;
        _loadButton3.Y = 0;
        _loadButton3.SetScale(1.8f);
        _loadButton3.KeyDown += updateFlagLoadButton;

        if(pStats3 != null)
        {
            _loadButton3.Text = pStats3.Name;
            _loadButton3.setTextMoney(pStats3.Money.ToString());
            _loadButton3.isNewGame = false;
        }

        _loadBackButton = new AnimatedButton(_GUIatlas);
        _loadBackButton.Text = "BACK";
        _loadBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _loadBackButton.X = -28f;
        _loadBackButton.Y = -10f;
        _loadBackButton.Click += HandleOptionsButtonBack;
        _loadBackButton.KeyDown += updateFlagLoadButton;

        _loadGamePanel.AddChild(_loadBackButton);
        _loadGamePanel.AddChild(_loadButton1);
        _loadGamePanel.AddChild(_loadButton2);
        _loadGamePanel.AddChild(_loadButton3);
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

    private void updateFlagLoadButton(Object sender, KeyEventArgs e)
    {
        if(sender == _loadButton3)
        {
            if (e.Key == Keys.Down || e.Key == Keys.Right)
            {
                _isLastFocusedLoadBackButton = false;
            }
        }
        else if(sender == _loadBackButton)
        {
            if (e.Key == Keys.Up || e.Key == Keys.Right)
            {
                _isLastFocusedLoadBackButton = true;
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

        _loadGamePanel.IsVisible = false;

        _newGamePanel.IsVisible = false;

        // Give the options button on the title panel focus since we are coming
        // back from the options screen.
        _optionsButton.IsFocused = true;
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.IsVisible = false;

        _loadGamePanel.IsVisible = true;

        _newGamePanel.IsVisible = false;

        // Set the options panel to be visible.
        _optionsPanel.IsVisible = false;

        _newGameNametextBox.Text = "";
    }

    private void HandleNewGameClicked(object sender)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.IsVisible = false;

        _loadGamePanel.IsVisible = false;

        _newGamePanel.IsVisible = true;

        // Set the options panel to be visible.
        _optionsPanel.IsVisible = false;

        _newGameButton = (TextureButton)sender;
    }

    private void handleConfirmNameClicked(object sender, EventArgs e)
    {
        if(_newGameButton == _loadButton1)
        {
            pStats1 = new PlayerStats();
            pStats1.Name = _newGameNametextBox.Text;
            pStats1.Money = 0;
            pStats1.Path = PATH1;

            PlayerStats.SaveGame(pStats1);
            _loadButton1.isNewGame = false;

            Core.ChangeScene(new GameScene(STARTING_LEVEL, pStats1));
        }
        if(_newGameButton == _loadButton2)
        {
            pStats2 = new PlayerStats();
            pStats2.Name = _newGameNametextBox.Text;
            pStats2.Money = 0;
            pStats2.Path = PATH2;

            PlayerStats.SaveGame(pStats2);
            _loadButton2.isNewGame = false;

            Core.ChangeScene(new GameScene(STARTING_LEVEL, pStats2));
        }
        if(_newGameButton == _loadButton3)
        {
            pStats3 = new PlayerStats();
            pStats3.Name = _newGameNametextBox.Text;
            pStats3.Money = 0;
            pStats3.Path = PATH3;

            PlayerStats.SaveGame(pStats3);
            _loadButton3.isNewGame = false;

            Core.ChangeScene(new GameScene(STARTING_LEVEL, pStats3));
        }
        _newGameNametextBox.Text = "";
    }

}
